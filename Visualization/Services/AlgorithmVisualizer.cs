namespace Visualization.Services;

using Core.Interfaces;
using System.Drawing;

public sealed class AlgorithmVisualizer {
    private const int BarWidth = 30;
    private const int BarSpacing = 5;
    private const int BottomPadding = 50;
    private const int TopPadding = 50;
    
    private IAlgorithm _algorithm;
    private int[] _array;
    private readonly System.Windows.Forms.Timer _timer;
    private readonly List<int[]> _steps;
    private int _currentStep;
    private bool _isRunning;
    
    public event Action<Graphics> OnFrameReady;
    
    public AlgorithmVisualizer()
    {
        _timer = new System.Windows.Forms.Timer();
        _timer.Interval = 50;
        _timer.Tick += OnTimerTick;
        _steps = new List<int[]>();
    }
    
    public void Initialize(IAlgorithm algorithm, int[] array)
    {
        _algorithm = algorithm;
        _array = (int[])array.Clone();
        _steps.Clear();
        _currentStep = 0;
        _isRunning = false;
        
        foreach (var step in algorithm.ExecuteWithSteps(_array))
        {
            _steps.Add((int[])step);
        }
    }
    
    public void Start()
    {
        if (_isRunning) return;
        
        _isRunning = true;
        _timer.Interval = _algorithm.Speed;
        _timer.Start();
    }
    
    public void Pause()
    {
        _isRunning = false;
        _timer.Stop();
    }
    
    public void Reset()
    {
        _currentStep = 0;
        _isRunning = false;
        _timer.Stop();
        OnFrameReady?.Invoke(CreateGraphics());
    }
    
    private void OnTimerTick(object sender, EventArgs e)
    {
        if (!_isRunning || _currentStep >= _steps.Count - 1)
        {
            _timer.Stop();
            _isRunning = false;
            return;
        }
        
        _currentStep++;
        OnFrameReady?.Invoke(CreateGraphics());
    }
    
    private Graphics CreateGraphics()
    {
        var bitmap = new Bitmap(800, 600);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        
        if (_steps.Count == 0) return graphics;
        
        var currentArray = _steps[_currentStep];
        var maxValue = currentArray.Max();
        var scale = (bitmap.Height - BottomPadding - TopPadding) / (float)maxValue;
        
        for (int i = 0; i < currentArray.Length; i++)
        {
            var barHeight = currentArray[i] * scale;
            var x = i * (BarWidth + BarSpacing) + BarSpacing;
            var y = bitmap.Height - BottomPadding - barHeight;
            
            graphics.FillRectangle(Brushes.Blue, x, y, BarWidth, barHeight);
            graphics.DrawRectangle(Pens.Black, x, y, BarWidth, barHeight);
            
            graphics.DrawString(currentArray[i].ToString(), 
                new Font("Arial", 8), 
                Brushes.Black, 
                x + 5, 
                y - 15);
        }
        
        return graphics;
    }
}