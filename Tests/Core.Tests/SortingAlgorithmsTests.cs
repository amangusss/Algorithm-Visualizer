using Xunit;

namespace Algorithm_Visualizer.Tests.Core.Tests;

using Algorithm_Visualizer.Core.Interfaces;
using Algorithm_Visualizer.Core.Algorithms.Sorting;

public class SortingAlgorithmsTests {
    private static readonly int[][] TestArrays = {
        new[] { 5, 3, 8, 4, 2 },
        new[] { 1, 2, 3, 4, 5 },
        new[] { 5, 4, 3, 2, 1 },
        new[] { 1, 1, 1, 1, 1 },
        new[] { 42 },
        Array.Empty<int>()
    };

    public static IEnumerable<object[]> GetTestData() {
        var algorithms = new ISortingAlgorithm[] {
            new BubbleSort(),
            new QuickSort(),
            new MergeSort(),
            new InsertionSort(),
            new SelectionSort()
        };

        foreach (var algorithm in algorithms) {
            foreach (var array in TestArrays) {
                yield return new object[] { algorithm, array, array.OrderBy(x => x).ToArray() };
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void Algorithm_Should_Sort_Array_Correctly(ISortingAlgorithm sorter, int[] input, int[] expected) {
        var result = sorter.Execute(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void Algorithm_Should_Not_Modify_Input_Array(ISortingAlgorithm sorter, int[] input, int[] _) {
        var originalArray = (int[])input.Clone();
        sorter.Execute(input);
        Assert.Equal(originalArray, input);
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void Algorithm_Steps_Should_Lead_To_Sorted_Array(ISortingAlgorithm sorter, int[] input, int[] expected) {
        var steps = sorter.ExecuteWithSteps(input).ToList();
        
        Assert.NotEmpty(steps);
        Assert.Equal(input.Length, steps.First().Length);
        Assert.Equal(expected, steps.Last());
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void Algorithm_Steps_Should_Preserve_Array_Elements(ISortingAlgorithm sorter, int[] input, int[] _) {
        var steps = sorter.ExecuteWithSteps(input).ToList();
        
        foreach (var step in steps) {
            Assert.Equal(input.OrderBy(x => x), step.OrderBy(x => x));
        }
    }

    [Theory]
    [InlineData(typeof(BubbleSort), "Bubble Sort")]
    [InlineData(typeof(QuickSort), "Quick Sort")]
    [InlineData(typeof(MergeSort), "Merge Sort")]
    [InlineData(typeof(InsertionSort), "Insertion Sort")]
    [InlineData(typeof(SelectionSort), "Selection Sort")]
    public void Algorithm_Should_Have_Correct_Name(Type algorithmType, string expectedName) {
        var algorithm = (ISortingAlgorithm)Activator.CreateInstance(algorithmType)!;
        Assert.Equal(expectedName, algorithm.Name);
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void Algorithm_Speed_Should_Be_In_Valid_Range(ISortingAlgorithm sorter, int[] input, int[] _) {
        Assert.InRange(sorter.Speed, 1, 100);
        
        sorter.Speed = 75;
        Assert.Equal(75, sorter.Speed);
    }

    private static bool IsSorted(int[] array) {
        for (int i = 0; i < array.Length - 1; i++) {
            if (array[i] > array[i + 1]) return false;
        }
        return true;
    }
}