namespace Algorithm_Visualizer.Tests.Core.Tests;

public class ArchitectureTests {
    [Fact]
    public void Core_Should_Reference_NoExternalProjects()
    {
        var assembly = typeof(Core.Interfaces.IAlgorithm).Assembly;
        var referencedAssemblies = assembly.GetReferencedAssemblies();
        
        Assert.DoesNotContain(referencedAssemblies, 
            a => a.Name == "Visualization" || a.Name == "WinFormsUI");
    }
}