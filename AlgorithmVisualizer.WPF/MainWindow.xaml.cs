namespace AlgorithmVisualizer.WPF;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Core.Algorithms.Sorting;
using Core.Interfaces;
using Core.Models;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
    
    private readonly List<ISortingAlgorithm> _algorithms;
    private IEnumerator<AnimationState>? _currentAnimation;
    private int[]? _array;
    private readonly Random _random = new Random();
    private bool _isPlaying;
    private readonly System.Windows.Threading.DispatcherTimer _timer;

    public MainWindow() {
        InitializeComponent();
        
        _algorithms = new List<ISortingAlgorithm> {
            new BubbleSort(),
            new SelectionSort(),
            new InsertionSort(),
            new MergeSort(),
            new QuickSort()
        };

        AlgorithmComboBox.ItemsSource = _algorithms;
        AlgorithmComboBox.DisplayMemberPath = "Name";
        
        _timer = new System.Windows.Threading.DispatcherTimer {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _timer.Tick += Timer_Tick;

        GenerateButton.Click += GenerateButton_Click;
        PlayButton.Click += PlayButton_Click;
        PauseButton.Click += PauseButton_Click;
        StepButton.Click += StepButton_Click;
        SpeedSlider.ValueChanged += SpeedSlider_ValueChanged;
    }

    private void GenerateButton_Click(object sender, RoutedEventArgs e) {
        if (!int.TryParse(ArraySizeTextBox.Text, out int size) || size <= 0) {
            MessageBox.Show("Please enter a valid positive number for array size.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        
        size = Math.Min(size, 100);
        
        _array = new int[size];
        for (int i = 0; i < _array.Length; i++) {
            _array[i] = _random.Next(1, 100);
        }
        
        var algorithm = (ISortingAlgorithm)AlgorithmComboBox.SelectedItem;
        if (algorithm != null) {
            _currentAnimation = algorithm.ExecuteWithSteps(_array).GetEnumerator();
            DrawArray(_array);
            DescriptionText.Text = algorithm.Description;
        } else {
            MessageBox.Show("Please select an algorithm first.", "No Algorithm Selected", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void PlayButton_Click(object sender, RoutedEventArgs e) {
        if (_currentAnimation != null) {
            _isPlaying = true;
            _timer.Start();
        }
    }

    private void PauseButton_Click(object sender, RoutedEventArgs e) {
        _isPlaying = false;
        _timer.Stop();
    }

    private void StepButton_Click(object sender, RoutedEventArgs e) {
        if (_currentAnimation != null) {
            StepAnimation();
        }
    }

    private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        if (_timer != null) {
            _timer.Interval = TimeSpan.FromMilliseconds(200 - e.NewValue * 1.9);
        }
    }

    private void Timer_Tick(object? sender, EventArgs e) {
        StepAnimation();
    }

    private void StepAnimation() {
        if (_currentAnimation?.MoveNext() == true) {
            var state = _currentAnimation.Current;
            DrawArray(state.Array, state.ActiveIndices, state.SwappedIndices);
            DescriptionText.Text = state.Description;
        } else {
            _isPlaying = false;
            _timer.Stop();
        }
    }

    private void DrawArray(int[] array, HashSet<int>? activeIndices = null, HashSet<int>? swappedIndices = null) {
        VisualizationCanvas.Children.Clear();
        
        var maxValue = array.Max();
        var canvasWidth = VisualizationCanvas.ActualWidth;
        var canvasHeight = VisualizationCanvas.ActualHeight;
        
        var barWidth = Math.Min(canvasWidth / array.Length * 0.8, 30);
        var spacing = Math.Min(barWidth * 0.2, 5);
        var scale = (canvasHeight - 40) / maxValue;
        
        for (int i = 0; i < array.Length; i++) {
            var value = array[i];
            var height = value * scale;
            var x = i * (barWidth + spacing);
            var y = canvasHeight - height;
            
            var rect = new Rectangle {
                Width = barWidth,
                Height = height,
                Fill = GetBarColor(i, activeIndices, swappedIndices),
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            
            var text = new TextBlock {
                Text = value.ToString(),
                Foreground = Brushes.Black,
                FontSize = 12
            };
            
            Canvas.SetLeft(text, x + barWidth/2 - 10);
            Canvas.SetTop(text, y - 20);
            
            VisualizationCanvas.Children.Add(rect);
            VisualizationCanvas.Children.Add(text);
        }
    }

    private Brush GetBarColor(int index, HashSet<int>? activeIndices, HashSet<int>? swappedIndices) {
        if (swappedIndices?.Contains(index) == true) return Brushes.Red;
        if (activeIndices?.Contains(index) == true) return Brushes.Yellow;
        return Brushes.Blue;
    }
}