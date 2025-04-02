namespace Algorithm_Visualizer.Core.Interfaces;

public interface IAlgorithm {
    string Name { get; }
    int Speed { get; set; }
    object execute(object input);
}