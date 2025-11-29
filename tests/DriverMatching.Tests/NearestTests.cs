using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Interfaces;
using DriverMatching.Core.Models;
using NUnit.Framework;

namespace DriverMatching.Tests;
[TestFixture]
public class NearestTests
{
    private List<IDriverFinder> finders = null!;

    [SetUp]
    public void SetUp() =>
        finders = new List<IDriverFinder> { new BruteForceFinder(), new HeapFinder(), new KdTreeFinder() };

    [TestCase(5)]
    [TestCase(1)]
    public void SmallFixedSet_ShouldReturnExpected(int k)
    {
        var drivers = new List<Driver>
        {
            new Driver(1,0,0),
            new Driver(2,5,0),
            new Driver(3,0,5),
            new Driver(4,3,3),
            new Driver(5,10,10)
        };
        var order = new Order(2,2);

        foreach (var f in finders)
        {
            var res = f.FindNearest(order, drivers, k);
            Assert.That(res.Count, Is.EqualTo(Math.Min(k, drivers.Count)));
            var d0 = res.First();
            var best = drivers.OrderBy(d => (long)(d.X - order.X)*(d.X - order.X) + (long)(d.Y - order.Y)*(d.Y - order.Y)).First();
            Assert.That(d0.Id, Is.EqualTo(best.Id));
        }
    }

    [Test]
    public void EmptyDrivers_ShouldReturnEmpty()
    {
        var order = new Order(0,0);
        foreach (var f in finders)
            Assert.That(f.FindNearest(order, Enumerable.Empty<Driver>(), 5), Is.Empty);
    }
}