namespace Algorithm_Visualizer.Tests.Core.Tests;

using Xunit;

using Algorithm_Visualizer.Core.Interfaces;

public class ArchitectureTests {
    [Fact]
    public void Core_Should_Reference_NoExternalProjects() {
        var assembly = typeof(IAlgorithm).Assembly;
        var referencedAssemblies = assembly.GetReferencedAssemblies();
        
        Assert.DoesNotContain(referencedAssemblies, 
            a => a.Name == "Visualization" || a.Name == "WinFormsUI");
    }
}