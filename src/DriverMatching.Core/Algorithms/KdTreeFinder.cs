using DriverMatching.Core.Interfaces;
using DriverMatching.Core.Models;

namespace DriverMatching.Core.Algorithms;
public sealed class KdTreeFinder : IDriverFinder
{
    private sealed class Node
    {
        public Driver D;
        public Node? Left, Right;
        public int Depth;
        public Node(Driver d, int depth) { D = d; Depth = depth; }
    }

    public List<Driver> FindNearest(Order order, IEnumerable<Driver> drivers, int k)
    {
        var list = drivers.ToList();
        if (!list.Any() || k <= 0) return new List<Driver>();

        var root = Build(list, 0);
        var comparer = Comparer<(long Dist, int Id, Driver D)>.Create((a, b) =>
        {
            var c = b.Dist.CompareTo(a.Dist);
            if (c != 0) return c;
            return a.Id.CompareTo(b.Id);
        });
        var best = new SortedSet<(long Dist, int Id, Driver D)>(comparer);

        void Search(Node? node)
        {
            if (node == null) return;
            long dist = SquaredDistance(node.D, order);
            var entry = (Dist: dist, Id: node.D.Id, D: node.D);
            if (best.Count < k) best.Add(entry);
            else
            {
                var worst = best.Max!;
                if (entry.Dist < worst.Dist || (entry.Dist == worst.Dist && entry.Id < worst.Id))
                {
                    best.Remove(worst);
                    best.Add(entry);
                }
            }

            int axis = node.Depth % 2;
            int coordNode = axis == 0 ? node.D.X : node.D.Y;
            int coordPoint = axis == 0 ? order.X : order.Y;

            Node? first = coordPoint <= coordNode ? node.Left : node.Right;
            Node? second = coordPoint <= coordNode ? node.Right : node.Left;

            Search(first);

            long diff = (long)coordPoint - coordNode;
            long diff2 = diff * diff;
            if (best.Count < k || diff2 <= best.Max!.Dist)
            {
                Search(second);
            }
        }

        Search(root);

        return best.OrderBy(x => x.Dist).ThenBy(x => x.Id).Select(x => x.D).ToList();
    }

    private static Node? Build(List<Driver> points, int depth)
    {
        if (points.Count == 0) return null;
        int axis = depth % 2;
        points.Sort((a, b) => axis == 0 ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));
        int mid = points.Count / 2;
        var node = new Node(points[mid], depth);
        var left = points.GetRange(0, mid);
        var right = points.GetRange(mid + 1, points.Count - mid - 1);
        node.Left = Build(left, depth + 1);
        node.Right = Build(right, depth + 1);
        return node;
    }

    private static long SquaredDistance(Driver d, Order o)
    {
        long dx = (long)d.X - o.X;
        long dy = (long)d.Y - o.Y;
        return dx * dx + dy * dy;
    }
}