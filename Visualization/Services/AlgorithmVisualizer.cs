namespace Visualization.Services;

using Core.Interfaces;
using Core.Models;
using System.Drawing;
using System.Drawing.Drawing2D;

public sealed class AlgorithmVisualizer {
    private const int BottomPadding = 50;
    private const int TopPadding = 50;
    private const int MinBarWidth = 20;
    private const int MaxBarWidth = 50;
    private const int MinBarSpacing = 5;
    private const int MaxBarSpacing = 15;
    private const int SideMargin = 60;
    
    private readonly List<AnimationState> _steps = new();
    private readonly List<AnimationState> _animationStates = new();
    private readonly System.Windows.Forms.Timer _timer = new();
    private int _currentStep;
    private float _animationProgress;
    private bool _isRunning;
    private Size _canvasSize = new(800, 600);
    private IAlgorithm _algorithm = null!;
    private int[] _array = null!;
    private int _comparisons;
    private int _swaps;
    private DateTime _startTime;
    private TimeSpan _executionTime;
    
    public event Action<Bitmap>? OnFrameReady;
    public event Action<int>? OnStepChanged;
    public event Action<bool>? OnAnimationCompleted;
    
    public int CurrentStep {
        get => _currentStep;
        private set {
            if (_currentStep != value) {
                _currentStep = value;
                OnStepChanged?.Invoke(value);
            }
        }
    }
    
    public int TotalSteps => _steps.Count;
    public bool IsRunning => _isRunning;
    public int Comparisons => _comparisons;
    public int Swaps => _swaps;
    public TimeSpan ExecutionTime => _executionTime;
    
    public AlgorithmVisualizer() {
        _timer.Interval = 16;
        _timer.Tick += OnTimerTick;
    }
    
    public void Initialize(IAlgorithm algorithm, int[] array) {
        _algorithm = algorithm;
        _array = (int[])array.Clone();
        _steps.Clear();
        _animationStates.Clear();
        _comparisons = 0;
        _swaps = 0;
        _startTime = DateTime.Now;
        
        var steps = algorithm.ExecuteWithSteps(_array);
        foreach (var step in steps) {
            _steps.Add(step);
        }
        
        _executionTime = DateTime.Now - _startTime;
        OnAnimationCompleted?.Invoke(true);
    }
    
    public void Start() {
        if (_steps.Count == 0) return;
        
        _isRunning = true;
        _timer.Start();
        _startTime = DateTime.Now;
    }
    
    public void Pause() {
        _isRunning = false;
        _timer.Stop();
    }
    
    public void Reset() {
        _isRunning = false;
        _timer.Stop();
        CurrentStep = 0;
        _animationProgress = 0f;
        OnAnimationCompleted?.Invoke(false);
    }
    
    public void StepForward() {
        if (CurrentStep < _steps.Count - 1) {
            CurrentStep++;
            _animationProgress = 0f;
            _timer.Stop();
            _isRunning = false;
        }
    }
    
    public void StepBackward() {
        if (CurrentStep > 0) {
            CurrentStep--;
            _animationProgress = 0f;
            _timer.Stop();
            _isRunning = false;
        }
    }
    
    public void SetSpeed(float speed) {
        _timer.Interval = (int)(16 / speed);
    }
    
    public void UpdateCanvasSize(int width, int height) {
        _canvasSize = new Size(width, height);
    }
    
    private void OnTimerTick(object? sender, EventArgs e) {
        _animationProgress += 0.05f;
        
        if (_animationProgress >= 1.0f) {
            _animationProgress = 0f;
            
            if (CurrentStep >= _steps.Count - 1) {
                _timer.Stop();
                _isRunning = false;
                OnAnimationCompleted?.Invoke(true);
                return;
            }
            
            CurrentStep++;
        }
        
        using (var bitmap = CreateGraphics()) {
            OnFrameReady?.Invoke(bitmap);
        }
    }
    
    private Bitmap CreateGraphics() {
        var bitmap = new Bitmap(_canvasSize.Width, _canvasSize.Height);
        using (var graphics = Graphics.FromImage(bitmap)) {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            graphics.Clear(Color.FromArgb(240, 240, 240));
            
            if (_steps.Count == 0) {
                return bitmap;
            }
            
            var currentState = _steps[CurrentStep];
            var nextState = CurrentStep < _steps.Count - 1 ? _steps[CurrentStep + 1] : currentState;
            
            var maxValue = Math.Max(currentState.Array.Max(), nextState.Array.Max());
            var availableHeight = bitmap.Height - BottomPadding - TopPadding - 30;
            
            var scale = Math.Min(availableHeight / (float)maxValue, availableHeight / 100.0f);
            
            var arrayLength = currentState.Array.Length;
            var availableWidth = bitmap.Width - (2 * SideMargin);
            var barWidth = Math.Min(MaxBarWidth, Math.Max(MinBarWidth, availableWidth / arrayLength - MinBarSpacing));
            var spacing = Math.Min(MaxBarSpacing, Math.Max(MinBarSpacing, (availableWidth - (barWidth * arrayLength)) / (arrayLength - 1)));
            
            var totalWidth = arrayLength * (barWidth + spacing) + spacing;
            var startX = (bitmap.Width - totalWidth) / 2;
            
            DrawBars(graphics, currentState, nextState, scale, startX, barWidth, spacing);
            
            using (var metricsFont = new Font("Segoe UI", 10)) {
                var metrics = $"Comparisons: {_comparisons} | Swaps: {_swaps} | Time: {_executionTime.TotalMilliseconds:F0}ms";
                var metricsSize = graphics.MeasureString(metrics, metricsFont);
                using (var metricsBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0))) {
                    graphics.DrawString(metrics,
                        metricsFont,
                        metricsBrush,
                        (bitmap.Width - metricsSize.Width) / 2,
                        bitmap.Height - 20);
                }
            }
        }
        
        return bitmap;
    }
    
    private void DrawBars(Graphics graphics, AnimationState currentState, AnimationState nextState, float scale, float startX, float barWidth, float spacing) {
        using (var gridPen = new Pen(Color.FromArgb(20, 0, 0, 0))) {
            for (int i = 0; i < graphics.VisibleClipBounds.Width; i += 50) {
                graphics.DrawLine(gridPen, i, 0, i, graphics.VisibleClipBounds.Height);
            }
            for (int i = 0; i < graphics.VisibleClipBounds.Height; i += 50) {
                graphics.DrawLine(gridPen, 0, i, graphics.VisibleClipBounds.Width, i);
            }
        }
        
        using (var titleFont = new Font("Segoe UI", 16, FontStyle.Bold)) {
            var title = $"{_algorithm.GetType().Name} - Step {CurrentStep + 1}/{TotalSteps}";
            var titleSize = graphics.MeasureString(title, titleFont);
            using (var titleBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0))) {
                graphics.DrawString(title,
                    titleFont,
                    titleBrush,
                    (graphics.VisibleClipBounds.Width - titleSize.Width) / 2,
                    TopPadding / 2);
            }
        }
        
        for (int i = 0; i < currentState.Array.Length; i++) {
            var currentHeight = currentState.Array[i] * scale;
            var nextHeight = currentState.Array[i] * scale;
            
            if (_currentStep < _steps.Count - 1 && nextState.SwappedIndices.Contains(i)) {
                nextHeight = nextState.Array[i] * scale;
            }
            
            var interpolatedHeight = Lerp(currentHeight, nextHeight, _animationProgress);
            var x = startX + i * (barWidth + spacing) + spacing;
            var y = graphics.VisibleClipBounds.Height - BottomPadding - interpolatedHeight;
            
            using (var path = CreateRoundedRectangle(x, y, barWidth, interpolatedHeight, 3)) {
                using (var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0))) {
                    graphics.FillPath(shadowBrush, CreateRoundedRectangle(x + 2, y + 2, barWidth, interpolatedHeight, 3));
                }

                Color startColor, endColor;
                if (currentState.ActiveIndices.Contains(i)) {
                    startColor = Color.FromArgb(255, 255, 215, 0);
                    endColor = Color.FromArgb(255, 255, 165, 0);  
                } else if (currentState.SwappedIndices.Contains(i)) {
                    startColor = Color.FromArgb(255, 220, 53, 69);
                    endColor = Color.FromArgb(255, 200, 35, 51);
                } else {
                    startColor = Color.FromArgb(255, 65, 105, 225);
                    endColor = Color.FromArgb(255, 100, 149, 237); 
                }

                using (var gradient = new LinearGradientBrush(
                    new Point((int)x, (int)y),
                    new Point((int)(x + barWidth), (int)(y + interpolatedHeight)),
                    startColor,
                    endColor))
                {
                    graphics.FillPath(gradient, path);
                }

                using (var pen = new Pen(Color.FromArgb(50, 0, 0, 0), 1)) {
                    graphics.DrawPath(pen, path);
                }

                using (var font = new Font("Segoe UI", Math.Min(10, barWidth / 3), FontStyle.Bold)) {
                    var text = currentState.Array[i].ToString();
                    var textSize = graphics.MeasureString(text, font);
                    var textX = x + (barWidth - textSize.Width) / 2;
                    var textY = y - textSize.Height - 2;

                    using (var shadowBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0))) {
                        graphics.DrawString(text, font, shadowBrush, textX + 1, textY + 1);
                    }
                    graphics.DrawString(text, font, Brushes.Black, textX, textY);
                }
            }
        }
        
        using (var axisFont = new Font("Segoe UI", 8)) {
            var maxValue = currentState.Array.Max();
            var maxValueText = maxValue.ToString();
            var maxValueSize = graphics.MeasureString(maxValueText, axisFont);
            graphics.DrawString(maxValueText,
                axisFont,
                Brushes.Black,
                startX - maxValueSize.Width - 5,
                TopPadding);
                
            graphics.DrawString("0",
                axisFont,
                Brushes.Black,
                startX - 10,
                graphics.VisibleClipBounds.Height - BottomPadding + 5);
        }
    }
    
    private static float Lerp(float start, float end, float progress) {
        return start + (end - start) * progress;
    }
    
    private static GraphicsPath CreateRoundedRectangle(float x, float y, float width, float height, float radius) {
        var path = new GraphicsPath();
        var diameter = radius * 2;
        var arc = new RectangleF(x, y, diameter, diameter);
        
        path.AddArc(arc, 180, 90);
        
        arc.X = x + width - diameter;
        path.AddArc(arc, 270, 90);
        
        arc.Y = y + height - diameter;
        path.AddArc(arc, 0, 90);
        
        arc.X = x;
        path.AddArc(arc, 90, 90);
        
        path.CloseFigure();
        return path;
    }
}