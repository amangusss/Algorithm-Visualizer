namespace Algorithm_Visualizer.Tests.Core.Tests;

using Algorithm_Visualizer.Core.Interfaces;

public class InterfaceTests {
    [Fact]
    public void ISortingAlgorithm_Should_Inherit_IAlgorithm() {
        Assert.True(typeof(ISortingAlgorithm).IsAssignableTo(typeof(IAlgorithm)));
    }

    [Fact]
    public void IAlgorithm_Should_Require_Name_Property() {
        var prop = typeof(IAlgorithm).GetProperty("Name");
        Assert.NotNull(prop);
        Assert.Equal(typeof(string), prop.PropertyType);
    }
}