using DriverMatching.Core.Models;

namespace DriverMatching.Core.Interfaces;
public interface IDriverFinder
{
    /// Возвращает список из k ближайших водителей в порядке увеличения расстояния.
    List<Driver> FindNearest(Order order, IEnumerable<Driver> drivers, int k);
}