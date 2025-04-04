using System.Drawing;
using Core.Algorithms;
using Core.Interfaces;
using Moq;
using Xunit;

namespace Visualization.Tests.Services;

public class AlgorithmVisualizerTests {
    private readonly Mock<IAlgorithm> _algorithmMock;
    private readonly AlgorithmVisualizer _visualizer;
    private readonly List<Bitmap> _receivedFrames;
    private readonly List<int> _receivedSteps;
    private readonly List<bool> _receivedCompletionStates;
    
    public AlgorithmVisualizerTests() {
        _algorithmMock = new Mock<IAlgorithm>();
        _visualizer = new AlgorithmVisualizer();
        _receivedFrames = new List<Bitmap>();
        _receivedSteps = new List<int>();
        _receivedCompletionStates = new List<bool>();
        
        _visualizer.OnFrameReady += frame => _receivedFrames.Add(frame);
        _visualizer.OnStepChanged += step => _receivedSteps.Add(step);
        _visualizer.OnAnimationCompleted += completed => _receivedCompletionStates.Add(completed);
    }
    
    [Fact]
    public void Initialize_WithValidInput_ShouldCreateInitialFrame() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = new[] { 5, 3, 1, 4, 2 };
        
        // Act
        _visualizer.Initialize(algorithm, array);
        
        // Assert
        Assert.Single(_receivedFrames);
        Assert.Empty(_receivedSteps);
        Assert.Empty(_receivedCompletionStates);
    }
    
    [Fact]
    public void Start_ShouldBeginAnimation() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = new[] { 5, 3, 1, 4, 2 };
        _visualizer.Initialize(algorithm, array);
        
        // Act
        _visualizer.Start();
        
        // Assert
        Assert.True(_visualizer.IsRunning);
    }
    
    [Fact]
    public void Pause_ShouldStopAnimation() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = new[] { 5, 3, 1, 4, 2 };
        _visualizer.Initialize(algorithm, array);
        _visualizer.Start();
        
        // Act
        _visualizer.Pause();
        
        // Assert
        Assert.False(_visualizer.IsRunning);
    }
    
    [Fact]
    public void Reset_ShouldResetToInitialState() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = new[] { 5, 3, 1, 4, 2 };
        _visualizer.Initialize(algorithm, array);
        _visualizer.Start();
        
        // Act
        _visualizer.Reset();
        
        // Assert
        Assert.False(_visualizer.IsRunning);
        Assert.Equal(0, _visualizer.CurrentStep);
        Assert.Contains(false, _receivedCompletionStates);
    }
    
    [Fact]
    public void StepForward_ShouldAdvanceOneStep() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = new[] { 5, 3, 1, 4, 2 };
        _visualizer.Initialize(algorithm, array);
        var initialStep = _visualizer.CurrentStep;
        
        // Act
        _visualizer.StepForward();
        
        // Assert
        Assert.Equal(initialStep + 1, _visualizer.CurrentStep);
        Assert.Contains(initialStep + 1, _receivedSteps);
    }
    
    [Fact]
    public void StepBackward_ShouldRetreatOneStep() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = new[] { 5, 3, 1, 4, 2 };
        _visualizer.Initialize(algorithm, array);
        _visualizer.StepForward();
        var currentStep = _visualizer.CurrentStep;
        
        // Act
        _visualizer.StepBackward();
        
        // Assert
        Assert.Equal(currentStep - 1, _visualizer.CurrentStep);
        Assert.Contains(currentStep - 1, _receivedSteps);
    }
    
    [Fact]
    public void SetSpeed_ShouldUpdateAnimationSpeed() {
        // Arrange
        var newSpeed = 0.5f;
        
        // Act
        _visualizer.SetSpeed(newSpeed);
        
        // Assert
        Assert.Equal(newSpeed, _visualizer.AnimationSpeed);
    }
    
    [Fact]
    public void SetSpeed_ShouldClampValues() {
        // Arrange
        var tooFast = 2.0f;
        var tooSlow = -0.1f;
        
        // Act
        _visualizer.SetSpeed(tooFast);
        Assert.Equal(1.0f, _visualizer.AnimationSpeed);
        
        _visualizer.SetSpeed(tooSlow);
        Assert.Equal(0.01f, _visualizer.AnimationSpeed);
    }
    
    [Fact]
    public void UpdateCanvasSize_ShouldCreateNewFrame() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = new[] { 5, 3, 1, 4, 2 };
        _visualizer.Initialize(algorithm, array);
        var initialFrameCount = _receivedFrames.Count;
        
        // Act
        _visualizer.UpdateCanvasSize(1000, 800);
        
        // Assert
        Assert.Equal(initialFrameCount + 1, _receivedFrames.Count);
    }
    
    [Fact]
    public void Animation_ShouldCompleteSuccessfully() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = new[] { 5, 3, 1, 4, 2 };
        _visualizer.Initialize(algorithm, array);
        
        // Act
        _visualizer.Start();
        
        // Wait for animation to complete
        Thread.Sleep(1000); // Adjust based on your animation speed
        
        // Assert
        Assert.Contains(true, _receivedCompletionStates);
        Assert.Equal(_visualizer.TotalSteps - 1, _visualizer.CurrentStep);
    }
    
    [Fact]
    public void Initialize_WithEmptyArray_ShouldHandleGracefully() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = Array.Empty<int>();
        
        // Act & Assert
        var exception = Record.Exception(() => _visualizer.Initialize(algorithm, array));
        Assert.Null(exception);
        Assert.Single(_receivedFrames);
    }
    
    [Fact]
    public void Initialize_WithSingleElement_ShouldHandleGracefully() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = new[] { 1 };
        
        // Act & Assert
        var exception = Record.Exception(() => _visualizer.Initialize(algorithm, array));
        Assert.Null(exception);
        Assert.Single(_receivedFrames);
    }
    
    [Fact]
    public void Initialize_WithDuplicateElements_ShouldHandleGracefully() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = new[] { 1, 1, 1, 1, 1 };
        
        // Act & Assert
        var exception = Record.Exception(() => _visualizer.Initialize(algorithm, array));
        Assert.Null(exception);
        Assert.Single(_receivedFrames);
    }
    
    [Fact]
    public void Initialize_WithNegativeNumbers_ShouldHandleGracefully() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = new[] { -5, 3, -1, 4, -2 };
        
        // Act & Assert
        var exception = Record.Exception(() => _visualizer.Initialize(algorithm, array));
        Assert.Null(exception);
        Assert.Single(_receivedFrames);
    }
    
    [Fact]
    public void Initialize_WithLargeNumbers_ShouldHandleGracefully() {
        // Arrange
        var algorithm = new BubbleSort();
        var array = new[] { 1000, 500, 2000, 1500, 3000 };
        
        // Act & Assert
        var exception = Record.Exception(() => _visualizer.Initialize(algorithm, array));
        Assert.Null(exception);
        Assert.Single(_receivedFrames);
    }
} 