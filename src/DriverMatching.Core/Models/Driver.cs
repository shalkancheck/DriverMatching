namespace DriverMatching.Core.Models;
public sealed class Driver
{
    public int Id { get; init; }
    public int X { get; set; }
    public int Y { get; set; }

    public Driver(int id, int x, int y)
    {
        Id = id;
        X = x;
        Y = y;
    }
}