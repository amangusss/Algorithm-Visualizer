namespace Algorithm_Visualizer.Core.Interfaces;

public interface IAlgorithm {
    string Name { get; }
    int Speed { get; set; }
    object Execute(object input);
    IEnumerable<object> ExecuteWithSteps(object input);
}