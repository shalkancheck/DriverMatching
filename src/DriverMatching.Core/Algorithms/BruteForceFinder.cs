using DriverMatching.Core.Interfaces;
using DriverMatching.Core.Models;

namespace DriverMatching.Core.Algorithms;
public sealed class BruteForceFinder : IDriverFinder
{
    public List<Driver> FindNearest(Order order, IEnumerable<Driver> drivers, int k)
    {
        return drivers
            .Select(d => (Driver: d, Dist2: SquaredDistance(d, order)))
            .OrderBy(x => x.Dist2)
            .ThenBy(x => x.Driver.Id)
            .Take(k)
            .Select(x => x.Driver)
            .ToList();
    }

    private static long SquaredDistance(Driver d, Order o)
    {
        long dx = (long)d.X - o.X;
        long dy = (long)d.Y - o.Y;
        return dx * dx + dy * dy;
    }
}