using Shouldly;
using Xunit.Abstractions;

namespace CSharpBasicsTest;

public class UnitTest1
{
    private readonly ITestOutputHelper _output;

    public UnitTest1(ITestOutputHelper output)
    {
        _output = output;    
    }

    [Fact]
    public void Test1()
    {
        var num1 = 1;
        var num2 = 2;

        int sum = num1 + num2;

        sum.ShouldBe(3);    
    }
}
