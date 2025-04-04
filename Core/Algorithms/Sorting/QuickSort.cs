namespace Core.Algorithms.Sorting;

using Interfaces;
using Models;


public class QuickSort : ISortingAlgorithm {
    public string Name => "Quick Sort";
    public string Description => "A divide-and-conquer algorithm that picks a pivot element and partitions the array around it.";

    public AnimationState Execute(int[] array) {
        var steps = ExecuteWithSteps(array);
        return steps.Last();
    }

    public IEnumerable<AnimationState> ExecuteWithSteps(int[] array) {
        var arr = (int[])array.Clone();
        var activeIndices = new HashSet<int>();
        var swappedIndices = new HashSet<int>();
        
        yield return new AnimationState(
            (int[])arr.Clone(),
            activeIndices,
            swappedIndices,
            "Starting Quick Sort"
        );
        
        foreach (var state in QuickSortHelper(arr, 0, arr.Length - 1, activeIndices, swappedIndices)) {
            yield return state;
        }
    }

    private IEnumerable<AnimationState> QuickSortHelper(int[] arr, int low, int high, HashSet<int> activeIndices, HashSet<int> swappedIndices) {
        if (low < high) {
            var (states, pivotIndex) = Partition(arr, low, high, activeIndices, swappedIndices);
            foreach (var state in states) {
                yield return state;
            }
            
            activeIndices.Clear();
            activeIndices.Add(pivotIndex);
            
            yield return new AnimationState(
                (int[])arr.Clone(),
                activeIndices,
                swappedIndices,
                $"Pivot {arr[pivotIndex]} is in its final position at index {pivotIndex}"
            );
            
            foreach (var state in QuickSortHelper(arr, low, pivotIndex - 1, activeIndices, swappedIndices)) {
                yield return state;
            }
            foreach (var state in QuickSortHelper(arr, pivotIndex + 1, high, activeIndices, swappedIndices)) {
                yield return state;
            }
        }
    }

    private (IEnumerable<AnimationState> states, int pivotIndex) Partition(int[] arr, int low, int high, HashSet<int> activeIndices, HashSet<int> swappedIndices) {
        var pivot = arr[high];
        var i = low - 1;
        var states = new List<AnimationState>();
        
        activeIndices.Clear();
        activeIndices.Add(high);
        
        states.Add(new AnimationState(
            (int[])arr.Clone(),
            activeIndices,
            swappedIndices,
            $"Selected pivot element {pivot} at index {high}"
        ));
        
        for (var j = low; j < high; j++) {
            activeIndices.Clear();
            activeIndices.Add(j);
            activeIndices.Add(high);
            
            states.Add(new AnimationState(
                (int[])arr.Clone(),
                activeIndices,
                swappedIndices,
                $"Comparing element {arr[j]} with pivot {pivot}"
            ));
            
            if (arr[j] < pivot) {
                i++;
                
                if (i != j) {
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                    swappedIndices.Add(i);
                    swappedIndices.Add(j);
                    
                    states.Add(new AnimationState(
                        (int[])arr.Clone(),
                        activeIndices,
                        swappedIndices,
                        $"Swapped elements at positions {i} and {j}"
                    ));
                }
            }
        }
        
        (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
        swappedIndices.Add(i + 1);
        swappedIndices.Add(high);
        
        states.Add(new AnimationState(
            (int[])arr.Clone(),
            activeIndices,
            swappedIndices,
            $"Placed pivot {pivot} in its final position at index {i + 1}"
        ));
        
        states.Add(new AnimationState(
            (int[])arr.Clone(),
            activeIndices,
            swappedIndices,
            $"Partition complete. Pivot {pivot} is at index {i + 1}"
        ));
        
        return (states, i + 1);
    }
}