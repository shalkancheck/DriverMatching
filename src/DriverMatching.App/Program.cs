using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Interfaces;
using DriverMatching.Core.Models;

const int N = 2000;
const int driversCount = 20000;
const int k = 5;

var rnd = new Random(0);
var drivers = new List<Driver>(driversCount);
for (int i = 0; i < driversCount; i++)
    drivers.Add(new Driver(i + 1, rnd.Next(0, N), rnd.Next(0, N)));

var order = new Order(N / 2, N / 2);

List<IDriverFinder> finders = new()
{
    new BruteForceFinder(),
    new HeapFinder(),
    new KdTreeFinder()
};

foreach (var f in finders)
{
    var name = f.GetType().Name;
    var sw = System.Diagnostics.Stopwatch.StartNew();
    var res = f.FindNearest(order, drivers, k);
    sw.Stop();
    Console.WriteLine($"{name}: {sw.ElapsedMilliseconds} ms -> {string.Join(", ", res.Select(d => d.Id))}");
}