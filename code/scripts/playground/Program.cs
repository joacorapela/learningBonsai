using System.Reactive.Linq;

IObservable<long> ticks = Observable.Timer(
    dueTime: TimeSpan.Zero,
    period:  TimeSpan.FromSeconds(1));
ticks.Subscribe(
    tick => Console.WriteLine($"Tick: {tick}"));
Console.ReadLine();

// IObservable<int> numbers = Observable.Range(1, 100);
// var windows = numbers.Buffer(5);
// var averages = windows.Select(x => x.Average());
// averages.Subscribe(x => Console.WriteLine(x));

