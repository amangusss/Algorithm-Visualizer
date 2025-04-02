namespace Algorithm_Visualizer.Visualization.Interfaces;

public interface IVisualizer {
    void Initialize(int[] data);
    void Update(int[] currentState);
    void Delay(int milliseconds);
}