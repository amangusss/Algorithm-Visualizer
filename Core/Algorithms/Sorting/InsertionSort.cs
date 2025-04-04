namespace Core.Algorithms.Sorting;

using Interfaces;
using Models;

public class InsertionSort : ISortingAlgorithm {
    public string Name => "Insertion Sort";
    public string Description => "A simple sorting algorithm that builds the final sorted array one item at a time.";

    public AnimationState Execute(int[] array) {
        var steps = ExecuteWithSteps(array);
        return steps.Last();
    }

    public IEnumerable<AnimationState> ExecuteWithSteps(int[] array) {
        var n = array.Length;
        var arr = (int[])array.Clone();
        
        for (var i = 1; i < n; i++) {
            var key = arr[i];
            var j = i - 1;
            var activeIndices = new HashSet<int> { i };
            var swappedIndices = new HashSet<int>();
            
            yield return new AnimationState(
                (int[])arr.Clone(),
                activeIndices,
                swappedIndices,
                $"Looking for insertion position for element {key} at index {i}"
            );
            
            while (j >= 0 && arr[j] > key) {
                arr[j + 1] = arr[j];
                activeIndices.Add(j);
                swappedIndices.Add(j + 1);
                
                yield return new AnimationState(
                    (int[])arr.Clone(),
                    activeIndices,
                    swappedIndices,
                    $"Moving element {arr[j]} from position {j} to {j + 1}"
                );
                
                j--;
            }
            
            arr[j + 1] = key;
            swappedIndices.Add(j + 1);
            
            yield return new AnimationState(
                (int[])arr.Clone(),
                activeIndices,
                swappedIndices,
                $"Inserted element {key} at position {j + 1}"
            );
        }
    }
} 