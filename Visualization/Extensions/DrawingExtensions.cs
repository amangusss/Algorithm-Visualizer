namespace Visualization.Extensions;

using Renderers;
using System.Drawing;

public static class DrawingExtensions {
    public static void DrawAlgorithmState(this Graphics g, int[] data) {
        var renderer = new ArrayRenderer();
        renderer.Render(g, data);
    }
}