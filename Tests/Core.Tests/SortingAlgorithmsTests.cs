using Xunit;

namespace Algorithm_Visualizer.Tests.Core.Tests;

using Algorithm_Visualizer.Core.Interfaces;
using Algorithm_Visualizer.Core.Algorithms.Sorting;

public class SortingAlgorithmsTests {
    private readonly int[] _testArray = { 5, 3, 8, 4, 2 };
    
    [Fact]
    public void BubbleSort_Should_Implement_ISortingAlgorithm() {
        var sorter = new BubbleSort();
        Assert.IsAssignableFrom<ISortingAlgorithm>(sorter);
        Assert.IsAssignableFrom<IAlgorithm>(sorter);
    }

    [Fact]
    public void BubbleSort_Should_Sort_Array() {
        var sorter = new BubbleSort();
        var steps = sorter.ExecuteWithSteps(_testArray).ToList();
        
        Assert.Equal(_testArray.Length, steps.First().Length);
        Assert.True(IsSorted(steps.Last()));
    }

    [Fact]
    public void QuickSort_Should_Produce_Correct_Steps() {
        var sorter = new QuickSort();
        var steps = sorter.ExecuteWithSteps(_testArray).ToList();
        
        Assert.Equal(2, steps.Count(s => s.SequenceEqual(new[] { 5, 3, 4, 2, 8 })));
    }

    private static bool IsSorted(int[] array) {
        for (int i = 0; i < array.Length - 1; i++) {
            if (array[i] > array[i + 1]) return false;
        }
        return true;
    }
}