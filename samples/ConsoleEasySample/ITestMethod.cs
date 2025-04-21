using System.Threading.Tasks;

namespace ConsoleSample;

public interface ITestMethod
{
    Task<string> Output(string message);
}