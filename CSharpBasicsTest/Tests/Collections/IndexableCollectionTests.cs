using Shouldly;
using Xunit.Abstractions;

namespace CSharpBasicsTest.Tests.Collections;

// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/collections
public class IndexableCollectionTests
{
    private readonly ITestOutputHelper _output;

    public IndexableCollectionTests(ITestOutputHelper output)
    {
        this._output = output;   
    }

    [Fact]
    public void RemoveAtOddNumberTest()
    {
        // Arrange
        List<int> numbers = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

        // Act
        for (var index = numbers.Count - 1; index >= 0; index--)
        {
            if (numbers[index] % 2 == 1)
            {
                // Remove the element by specifying
                // the zero-based index in the list.
                numbers.RemoveAt(index);
            }
        }

        // Assert
        numbers.ShouldBe(new[] {0, 2, 4, 6, 8});
    }

    [Fact]
    public void RemoveAllOddNumberTest()
    {
        // Arrange
        List<int> numbers = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

        // Act
        numbers.RemoveAll(i => i % 2 == 1);

        // Assert
        numbers.ShouldBe(new[] {0, 2, 4, 6, 8});
    }

    public class Galaxy
    {
        public string Name { get; set; }
        public int MegaLightYears { get; set; }
    }

    [Fact]
    public void ObjectListInitTest()
    {
        // Arrange
        var theGalaxies = new List<Galaxy>
        {
            new (){ Name="Tadpole", MegaLightYears=400},
            new (){ Name="Pinwheel", MegaLightYears=25},
            new (){ Name="Milky Way", MegaLightYears=0},
            new (){ Name="Andromeda", MegaLightYears=3}
        };

        // Act, Assert
        theGalaxies.Count.ShouldBe(4);
        
        theGalaxies[0].Name.ShouldBe("Tadpole");
        theGalaxies[0].MegaLightYears.ShouldBe(400);
        
        theGalaxies.ShouldContain(g => g.Name == "Milky Way");
        theGalaxies.Single(g => g.Name == "Milky Way")
            .MegaLightYears.ShouldBe(0);

        theGalaxies.Count(g => g.MegaLightYears > 0).ShouldBe(3);
        theGalaxies.All(g => g.MegaLightYears >= 0).ShouldBeTrue();
    }

    public class Element
    {
        public required string Symbol { get; init; }
        public required string Name { get; init; }
        public required int AtomicNumber { get; init; }    
    }

    [Fact]
    public void KeyValuePairTest()
    {
        // Arrange
        Dictionary<string, Element> elements = new ()
        {
            { "K", new () { Symbol = "K", Name = "Potassium", AtomicNumber = 19}},
            {"Ca", new (){ Symbol = "Ca", Name = "Calcium", AtomicNumber = 20}},
            {"Sc", new (){ Symbol = "Sc", Name = "Scandium", AtomicNumber = 21}},
            {"Ti", new (){ Symbol = "Ti", Name = "Titanium", AtomicNumber = 22}}    
        };

        // Act & Assert
        elements.Count.ShouldBe(4);
        elements.ShouldContainKey("K");

        elements["K"].Name.ShouldBe("Potassium");
        elements["K"].AtomicNumber.ShouldBe(19);

        elements.TryGetValue("K", out Element? value).ShouldBeTrue();
        value.Symbol.ShouldBe("K");

        elements.TryGetValue("Na", out _).ShouldBeFalse();

        Should.Throw<KeyNotFoundException>(() => { var _ = elements["Na"]; });

        elements.Keys.ShouldBe(new[] { "K", "Ca", "Sc", "Ti" }); // 주의: 순서는 보장 X
        elements.Values.ShouldContain(e => e.Name == "Titanium");

        elements["K"].AtomicNumber.ShouldBe(19);
        elements["K"] = new Element { Symbol="K", Name="Potassium", AtomicNumber=999 };
        elements["K"].AtomicNumber.ShouldBe(999);

        elements.Remove("Sc").ShouldBeTrue();
        elements.ShouldNotContainKey("Sc");
    }

    [Fact]
    public void LinqCollectionTest()
    {
        // Arrange
        List<Element> elements = BuildList();

        // Act
        var subset = from theElement in elements
            where theElement.AtomicNumber < 22
            orderby theElement.Name
            select theElement;
        var actual = subset.ToList();

        // Assert
        actual.Count.ShouldBe(3);
      
        actual.All(e => e.AtomicNumber < 22).ShouldBeTrue();
      
        actual.Select(e => e.Name)
            .ShouldBe(new [] { "Calcium", "Potassium", "Scandium" });
        
        // 오름차순인지 검증
        actual.Select(e => e.Name)
            .SequenceEqual(actual.Select(e => e.Name).Order()) 
            .ShouldBeTrue();             

        // 순서 중요
        actual.Select(e => e.Symbol)
            .ShouldBe(new[] { "Ca", "K", "Sc" });    
    }

    private static List<Element> BuildList() => new()
    {
        { new(){ Symbol="K", Name="Potassium", AtomicNumber=19}},
        { new(){ Symbol="Ca", Name="Calcium", AtomicNumber=20}},
        { new(){ Symbol="Sc", Name="Scandium", AtomicNumber=21}},
        { new(){ Symbol="Ti", Name="Titanium", AtomicNumber=22}}
    };
}