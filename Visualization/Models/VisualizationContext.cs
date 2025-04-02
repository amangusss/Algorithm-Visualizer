namespace Visualization.Models;

using System.Drawing;
using Core.Interfaces;

public class VisualizationContext {
    public IAlgorithm ? Algorithm { get; set; }
    public int[]? Data { get; set; }
    public int AnimationDelay { get; set; } = 100;
    public Size CanvasSize { get; set; }
}