# Driver Matching Service

Приложение для подбора ближайших водителей к заказу на прямоугольной сетке координат.

## Описание

Проект реализует несколько алгоритмов поиска K ближайших водителей к точке заказа:
- **BruteForceFinder** - полный перебор всех водителей
- **HeapFinder** - использование приоритетной очереди (heap)
- **KdTreeFinder** - использование KD-дерева для оптимизации

## Структура проекта

```
src/
  ├── DriverMatching.Core/        # Основная бизнес-логика
  │   ├── Models/                 # Driver, Order модели
  │   ├── Interfaces/             # IDriverFinder интерфейс
  │   └── Algorithms/             # Три реализации алгоритмов
  └── DriverMatching.App/         # Консольное приложение для демонстрации

tests/
  └── DriverMatching.Tests/       # NUnit тесты для всех алгоритмов

benchmarks/
  └── DriverMatching.Benchmarks/  # BenchmarkDotNet сравнение производительности
```

## Установка и запуск

### Требования
- .NET 10 SDK

### Сборка проекта
```bash
dotnet build
```

### Запуск тестов
```bash
dotnet test tests\DriverMatching.Tests -c Release
```

### Запуск приложения (демонстрация)
```bash
dotnet run --project src\DriverMatching.App -c Release
```

### Запуск бенчмарков
```bash
dotnet run --project benchmarks\DriverMatching.Benchmarks -c Release
```

## Результаты сравнения алгоритмов

### Таблица производительности

| Алгоритм    | Среднее время | StdDev    | Память      | Лучший для      |
|-------------|---------------|-----------|-------------|-----------------|
| BruteForce  | 508.2 μs      | 14.56 μs  | 625.8 KB    | Малые наборы    |
| Heap        | 127.6 μs      | 4.92 μs   | 1.95 KB     | **Рекомендуется** |
| KdTree      | 22,629.6 μs   | 440.51 μs | 6933.55 KB  | Очень большие   |

### Вывод

**HeapFinder показывает лучший результат** для типичных сценариев:
- ✅ **В 4 раза быстрее** BruteForce (127.6 μs vs 508.2 μs)
- ✅ **Минимальное потребление памяти** (1.95 KB)
- ✅ Стабильная производительность

**KdTree неэффективен** для данного случая:
- ❌ Время построения дерева замораживает результат
- ❌ Превосходит ожидания по памяти

## Примеры использования

```csharp
// Создание списка водителей
var drivers = new List<Driver>
{
    new Driver(1, 10, 20),
    new Driver(2, 15, 25),
    new Driver(3, 5, 10)
};

// Создание заказа в точке (12, 22)
var order = new Order(12, 22);

// Поиск 5 ближайших водителей
var finder = new HeapFinder();
var nearest = finder.FindNearest(order, drivers, 5);

foreach (var driver in nearest)
    Console.WriteLine($"Водитель {driver.Id} на координатах ({driver.X}, {driver.Y})");
```

## Git Workflow

- `main` - стабильная версия с рабочим кодом
- `feature/*` - ветки для новых функций
- Каждое задание в отдельной ветке с merge request

## Тестирование

Все алгоритмы покрыты NUnit тестами:
- Проверка корректности на маленьких наборах данных
- Проверка обработки пустых списков
- Валидация первого элемента (должен быть ближайший)

Запуск тестов: `dotnet test`

## Результаты бенчмарков

![benchmark-results](./docs/benchmark-results.png)
Подробные результаты в: `BenchmarkDotNet.Artifacts/results/`