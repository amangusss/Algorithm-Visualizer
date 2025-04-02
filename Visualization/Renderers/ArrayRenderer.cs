namespace Visualization.Renderers;

using System.Drawing;

public class ArrayRenderer {
    private const int ColumnWidth = 40;
    private const int Padding = 10;
    
    private readonly Brush _defaultFill = new SolidBrush(Color.SteelBlue);
    private readonly Pen _outlinePen = new(Color.Black, 2);
    private readonly Font _font = new("Arial", 12);

    public void Render(Graphics g, int[] data) {
        g.Clear(Color.White);
        
        float maxHeight = g.VisibleClipBounds.Height - Padding * 2;
        float maxValue = data.Max();
        
        for (int i = 0; i < data.Length; i++) {
            float height = (data[i] / maxValue) * maxHeight;
            float x = Padding + i * ColumnWidth;
            float y = g.VisibleClipBounds.Height - height - Padding;
            
            g.FillRectangle(_defaultFill, x, y, ColumnWidth - 5, height);
            g.DrawRectangle(_outlinePen, x, y, ColumnWidth - 5, height);
            
            string text = data[i].ToString();
            var textSize = g.MeasureString(text, _font);
            g.DrawString(text, _font, Brushes.Black, 
                x + (ColumnWidth - 5 - textSize.Width) / 2, 
                y - textSize.Height);
        }
    }
}