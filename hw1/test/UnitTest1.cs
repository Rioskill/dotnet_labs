namespace test;

public class UnitTest1
{
    [Fact]
    public void TestAddition()
    {
        CalculatorNS.Calculator calc = new();

        Assert.Equal(2, calc.Calculate("1 + 1"));
    }

    [Fact]
    public void TestSubstraction()
    {
        CalculatorNS.Calculator calc = new();

        Assert.Equal(3, calc.Calculate("4 - 1"));
    }

    [Fact]
    public void TestMultiplication()
    {
        CalculatorNS.Calculator calc = new();

        Assert.Equal(8, calc.Calculate("4 * 2"));
    }

    [Fact]
    public void TestDivision()
    {
        CalculatorNS.Calculator calc = new();

        Assert.Equal(7, calc.Calculate("56 / 8"));
    }

    [Fact]
    public void TestOrderOfOperations()
    {
        CalculatorNS.Calculator calc = new();

        Assert.Equal(13, calc.Calculate("1 + 4 * 3"));
    }

    [Fact]
    public void TestsParantheses()
    {
        CalculatorNS.Calculator calc = new();

        Assert.Equal(15, calc.Calculate("3 * (1 + 4)"));
    }

    [Fact]
    public void TestParanthesesError()
    {
        CalculatorNS.Calculator calc = new();

        Assert.Throws<System.Exception>(() => calc.Calculate("(3 + 4))"));
    }

    [Fact]
    public void DivisionByZeroError()
    {
        CalculatorNS.Calculator calc = new();

        Assert.Throws<System.Exception>(() => calc.Calculate("2 / 0"));
    }

    
    [Fact]
    public void UnknownOperationError()
    {
        CalculatorNS.Calculator calc = new();

        Assert.Throws<System.Exception>(() => calc.Calculate("1 k 2"));
    }
}
