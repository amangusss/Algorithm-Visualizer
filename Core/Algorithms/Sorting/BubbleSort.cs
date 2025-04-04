namespace Core.Algorithms.Sorting;

using Models;
using Interfaces;

public class BubbleSort : ISortingAlgorithm {
    public string Name => "Bubble Sort";
    public string Description => "A simple sorting algorithm that repeatedly steps through the list, compares adjacent elements and swaps them if they are in the wrong order.";
    public int Speed { get; set; } = 50;

    public AnimationState Execute(int[] array) {
        var steps = ExecuteWithSteps(array);
        return steps.Last();
    }

    public IEnumerable<AnimationState> ExecuteWithSteps(int[] array) {
        var n = array.Length;
        var arr = (int[])array.Clone();
        
        for (var i = 0; i < n - 1; i++) {
            for (var j = 0; j < n - i - 1; j++) {
                var activeIndices = new HashSet<int> { j, j + 1 };
                var swappedIndices = new HashSet<int>();
                var description = $"Comparing elements at positions {j} and {j + 1}";
                
                if (arr[j] > arr[j + 1]) {
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    swappedIndices.Add(j);
                    swappedIndices.Add(j + 1);
                    description = $"Swapped elements at positions {j} and {j + 1}";
                }
                
                yield return new AnimationState(
                    (int[])arr.Clone(),
                    activeIndices,
                    swappedIndices,
                    description
                );
            }
        }
    }
}