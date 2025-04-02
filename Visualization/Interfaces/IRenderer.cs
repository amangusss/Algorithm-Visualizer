namespace Algorithm_Visualizer.Visualization.Interfaces;

public interface IRenderer {
    void Render(int[] data);
    void HighlightElements(params int[] indices);
}