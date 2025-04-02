namespace WinFormsUI;

using System.Drawing;
using Core.Algorithms.Sorting;
using Core.Interfaces;
using Visualization.Services;

public sealed partial class MainForm : Form {
    private readonly AlgorithmVisualizer _visualizer;
    private readonly Bitmap _buffer;
    private readonly Graphics _bufferGraphics;
    private ComboBox _algorithmComboBox;
    private TrackBar _speedTrackBar;
    private Button _startButton;
    private Button _pauseButton;
    private Button _resetButton;
    private NumericUpDown _arraySizeInput;
    private Button _generateButton;
    
    private readonly ISortingAlgorithm[] _algorithms;
    
    public MainForm() {
        InitializeComponent();
        
        _algorithms = new ISortingAlgorithm[] {
            new BubbleSort(),
            new QuickSort(),
            new MergeSort()
        };
        
        _buffer = new Bitmap(800, 600);
        _bufferGraphics = Graphics.FromImage(_buffer);
        
        _visualizer = new AlgorithmVisualizer();
        _visualizer.OnFrameReady += OnFrameReady;
        
        InitializeControls();
        GenerateNewArray();
    }
    
    private void InitializeControls()
    {
        _algorithmComboBox = new ComboBox {
            Location = new Point(10, 10),
            Width = 150,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _algorithmComboBox.Items.AddRange(_algorithms.Select(a => a.Name).ToArray());
        _algorithmComboBox.SelectedIndex = 0;
        
        _speedTrackBar = new TrackBar {
            Location = new Point(170, 10),
            Width = 150,
            Minimum = 1,
            Maximum = 100,
            Value = 50
        };
        
        _arraySizeInput = new NumericUpDown {
            Location = new Point(330, 10),
            Width = 80,
            Minimum = 5,
            Maximum = 50,
            Value = 10
        };
        
        _generateButton = new Button {
            Location = new Point(420, 10),
            Width = 80,
            Text = "Generate"
        };
        _generateButton.Click += (s, e) => GenerateNewArray();
        
        _startButton = new Button {
            Location = new Point(510, 10),
            Width = 80,
            Text = "Start"
        };
        _startButton.Click += (s, e) => _visualizer.Start();
        
        _pauseButton = new Button {
            Location = new Point(600, 10),
            Width = 80,
            Text = "Pause"
        };
        _pauseButton.Click += (s, e) => _visualizer.Pause();
        
        _resetButton = new Button {
            Location = new Point(690, 10),
            Width = 80,
            Text = "Reset"
        };
        _resetButton.Click += (s, e) => _visualizer.Reset();
        
        Controls.AddRange(new Control[] {
            _algorithmComboBox,
            _speedTrackBar,
            _arraySizeInput,
            _generateButton,
            _startButton,
            _pauseButton,
            _resetButton
        });
        
        ClientSize = new Size(800, 600);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Text = "Algorithm Visualizer";
    }
    
    private void GenerateNewArray() {
        var size = (int)_arraySizeInput.Value;
        var array = new int[size];
        var random = new Random();
        
        for (int i = 0; i < size; i++) {
            array[i] = random.Next(1, 100);
        }
        
        var selectedAlgorithm = _algorithms[_algorithmComboBox.SelectedIndex];
        selectedAlgorithm.Speed = _speedTrackBar.Value;
        _visualizer.Initialize(selectedAlgorithm, array);
        _visualizer.Reset();
    }
    
    private void OnFrameReady(Graphics g) {
        _bufferGraphics.Clear(Color.White);
        g.DrawImage(_buffer, 0, 0);
        Invalidate();
    }
    
    protected override void OnPaint(PaintEventArgs e) {
        e.Graphics.DrawImage(_buffer, 0, 0);
    }
}