using BenchmarkDotNet.Attributes;
using DriverMatching.Core.Algorithms;
using DriverMatching.Core.Models;

namespace DriverMatching.Benchmarks;
[MemoryDiagnoser]
public class NearestBenchmarks
{
    private List<Driver> drivers = null!;
    private Order order = null!;
    private const int k = 5;

    [GlobalSetup]
    public void Setup()
    {
        var rnd = new Random(123);
        const int N = 2000;
        const int driversCount = 20000;
        drivers = Enumerable.Range(1, driversCount).Select(i => new Driver(i, rnd.Next(0, N), rnd.Next(0, N))).ToList();
        order = new Order(N / 2, N / 2);
    }

    [Benchmark]
    public void BruteForce() => _ = new BruteForceFinder().FindNearest(order, drivers, k);

    [Benchmark]
    public void Heap() => _ = new HeapFinder().FindNearest(order, drivers, k);

    [Benchmark]
    public void KdTree() => _ = new KdTreeFinder().FindNearest(order, drivers, k);
}