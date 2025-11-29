namespace DriverMatching.Core.Models;
public sealed class Order
{
    public int X { get; init; }
    public int Y { get; init; }
    public Order(int x, int y) { X = x; Y = y; }
}