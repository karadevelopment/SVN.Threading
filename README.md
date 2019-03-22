# Documentation
comming soon

# Example
```
public class ExampleThread1 : Thread
{
    private int Limit { get; set; }
    private int ExampleProperty { get; set; }

    protected override void OnCreate()
    {
        this.Limit = 1000;
    }

    protected override void OnStart()
    {
        this.Limit += 1;
    }

    protected override void OnUpdate()
    {
        if (this.Limit <= ++this.ExampleProperty)
        {
            this.Active = false;
        }
    }

    public override string ToString()
    {
        return $"{this.Time:N3} / {this.DeltaTime:N3} / {this.ExampleProperty}";
    }
}
public class ExampleThread2 : Thread
{
    private int Limit { get; set; }
    private int ExampleProperty { get; set; }

    protected override void OnCreate()
    {
        this.Limit = 2000;
    }

    protected override void OnStart()
    {
        this.Limit += 1;
    }

    protected override void OnUpdate()
    {
        if (this.Limit <= ++this.ExampleProperty)
        {
            this.Active = false;
        }
    }

    public override string ToString()
    {
        return $"{this.Time:N3} / {this.DeltaTime:N3} / {this.ExampleProperty}";
    }
}
public class ExampleThread3 : Thread
{
    private int Limit { get; set; }
    private int ExampleProperty { get; set; }

    protected override void OnCreate()
    {
        this.Limit = int.MaxValue;
    }

    protected override void OnUpdate()
    {
        if (this.Limit <= ++this.ExampleProperty)
        {
            this.Active = false;
        }
    }

    public override string ToString()
    {
        return $"{this.Time:N3} / {this.DeltaTime:N3} / {this.ExampleProperty}";
    }
}
public static class Program
{
    public static void Main(string[] args)
    {
        var thread1 = Thread.Register<ExampleThread1>();
        var thread2 = Thread.Register<ExampleThread2>();
        var thread3 = Thread.Register<ExampleThread3>();

        while (Thread.Running)
        {
            Console.Clear();
            Console.WriteLine(thread1);
            Console.WriteLine(thread2);
            Console.WriteLine(thread3);
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
    }
}
```

# NuGet
https://www.nuget.org/packages/SVN.Threading/
