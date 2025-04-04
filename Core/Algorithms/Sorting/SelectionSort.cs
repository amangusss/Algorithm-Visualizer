namespace Core.Algorithms.Sorting;

using Interfaces;
using Models;

public class SelectionSort : ISortingAlgorithm {
    public string Name => "Selection Sort";
    public string Description => "A sorting algorithm that divides the input list into a sorted and an unsorted region, and iteratively selects the smallest element from the unsorted region to add to the sorted region.";
    public int Speed { get; set; } = 50;
    
    public AnimationState Execute(int[] array) {
        var steps = ExecuteWithSteps(array);
        return steps.Last();
    }
    
    public IEnumerable<AnimationState> ExecuteWithSteps(int[] array) {
        var n = array.Length;
        var arr = (int[])array.Clone();
        
        for (var i = 0; i < n - 1; i++) {
            var minIdx = i;
            var activeIndices = new HashSet<int> { i };
            var swappedIndices = new HashSet<int>();
            var description = $"Finding minimum element from index {i}";
            
            for (var j = i + 1; j < n; j++) {
                activeIndices.Add(j);
                
                if (arr[j] < arr[minIdx]) {
                    minIdx = j;
                    description = $"Found new minimum at position {j}";
                }
                
                yield return new AnimationState(
                    (int[])arr.Clone(),
                    activeIndices,
                    swappedIndices,
                    description
                );
            }
            
            if (minIdx != i) {
                (arr[i], arr[minIdx]) = (arr[minIdx], arr[i]);
                swappedIndices.Add(i);
                swappedIndices.Add(minIdx);
                
                yield return new AnimationState(
                    (int[])arr.Clone(),
                    activeIndices,
                    swappedIndices,
                    $"Swapped elements at positions {i} and {minIdx}"
                );
            }
        }
    }
} 