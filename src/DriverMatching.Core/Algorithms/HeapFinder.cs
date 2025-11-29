using DriverMatching.Core.Interfaces;
using DriverMatching.Core.Models;

namespace DriverMatching.Core.Algorithms;
public sealed class HeapFinder : IDriverFinder
{
    private sealed class EntryComparer : IComparer<(long Dist, int Id, Driver D)>
    {
        public int Compare((long Dist, int Id, Driver D) x, (long Dist, int Id, Driver D) y)
        {
            var c = y.Dist.CompareTo(x.Dist); // reverse => max-heap
            if (c != 0) return c;
            return x.Id.CompareTo(y.Id);
        }
    }

    public List<Driver> FindNearest(Order order, IEnumerable<Driver> drivers, int k)
    {
        if (k <= 0) return new List<Driver>();

        var set = new SortedSet<(long Dist, int Id, Driver D)>(new EntryComparer());
        foreach (var d in drivers)
        {
            long dist = SquaredDistance(d, order);
            var entry = (Dist: dist, Id: d.Id, D: d);
            if (set.Count < k) set.Add(entry);
            else
            {
                var worst = set.Max!;
                if (entry.Dist < worst.Dist || (entry.Dist == worst.Dist && entry.Id < worst.Id))
                {
                    set.Remove(worst);
                    set.Add(entry);
                }
            }
        }

        return set
            .OrderBy(x => x.Dist)
            .ThenBy(x => x.Id)
            .Select(x => x.D)
            .ToList();
    }

    private static long SquaredDistance(Driver d, Order o)
    {
        long dx = (long)d.X - o.X;
        long dy = (long)d.Y - o.Y;
        return dx * dx + dy * dy;
    }
}