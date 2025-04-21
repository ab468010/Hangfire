using System;
using System.Threading.Tasks;

namespace ConsoleSample;

public class TestMethod : ITestMethod
{
    public int Age = 0;

    public TestMethod()
    {
        
    }

    public TestMethod(AgeProvider provider)
    {
        this.Age = provider.Age;
    }

    public Task<string> Output(string message)
    {
        Console.WriteLine($"Hello {message}!,{Age}");
        return Task.FromResult(message);
    }
}

public class AgeProvider
{
    public int Age = 10;
}