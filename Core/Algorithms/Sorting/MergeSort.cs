namespace Core.Algorithms.Sorting;

using Interfaces;
using Models;


public class MergeSort : ISortingAlgorithm {
    public string Name => "Merge Sort";
    public string Description => "A divide-and-conquer algorithm that recursively breaks down a problem into smaller, more manageable subproblems.";

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
            "Starting Merge Sort"
        );
        
        foreach (var state in MergeSortHelper(arr, 0, arr.Length - 1, activeIndices, swappedIndices)) {
            yield return state;
        }
    }

    private IEnumerable<AnimationState> MergeSortHelper(int[] arr, int left, int right, HashSet<int> activeIndices, HashSet<int> swappedIndices) {
        if (left < right) {
            var mid = (left + right) / 2;
            
            activeIndices.Clear();
            activeIndices.Add(mid);
            
            yield return new AnimationState(
                (int[])arr.Clone(),
                activeIndices,
                swappedIndices,
                $"Dividing array from index {left} to {right} at {mid}"
            );
            
            foreach (var state in MergeSortHelper(arr, left, mid, activeIndices, swappedIndices)) {
                yield return state;
            }
            foreach (var state in MergeSortHelper(arr, mid + 1, right, activeIndices, swappedIndices)) {
                yield return state;
            }
            
            foreach (var state in Merge(arr, left, mid, right, activeIndices, swappedIndices)) {
                yield return state;
            }
        }
    }

    private IEnumerable<AnimationState> Merge(int[] arr, int left, int mid, int right, HashSet<int> activeIndices, HashSet<int> swappedIndices) {
        var n1 = mid - left + 1;
        var n2 = right - mid;
        
        var leftArr = new int[n1];
        var rightArr = new int[n2];
        
        for (var i = 0; i < n1; i++)
            leftArr[i] = arr[left + i];
        for (var j = 0; j < n2; j++)
            rightArr[j] = arr[mid + 1 + j];
            
        var i1 = 0;
        var j1 = 0;
        var k = left;
        
        while (i1 < n1 && j1 < n2) {
            activeIndices.Clear();
            activeIndices.Add(k);
            
            if (leftArr[i1] <= rightArr[j1]) {
                arr[k] = leftArr[i1];
                swappedIndices.Add(k);
                i1++;
                
                yield return new AnimationState(
                    (int[])arr.Clone(),
                    activeIndices,
                    swappedIndices,
                    $"Merging: placed {arr[k]} from left subarray at position {k}"
                );
            }
            else {
                arr[k] = rightArr[j1];
                swappedIndices.Add(k);
                j1++;
                
                yield return new AnimationState(
                    (int[])arr.Clone(),
                    activeIndices,
                    swappedIndices,
                    $"Merging: placed {arr[k]} from right subarray at position {k}"
                );
            }
            k++;
        }
        
        while (i1 < n1) {
            activeIndices.Clear();
            activeIndices.Add(k);
            arr[k] = leftArr[i1];
            swappedIndices.Add(k);
            i1++;
            k++;
            
            yield return new AnimationState(
                (int[])arr.Clone(),
                activeIndices,
                swappedIndices,
                $"Merging: placed remaining element {arr[k-1]} from left subarray at position {k-1}"
            );
        }
        
        while (j1 < n2) {
            activeIndices.Clear();
            activeIndices.Add(k);
            arr[k] = rightArr[j1];
            swappedIndices.Add(k);
            j1++;
            k++;
            
            yield return new AnimationState(
                (int[])arr.Clone(),
                activeIndices,
                swappedIndices,
                $"Merging: placed remaining element {arr[k-1]} from right subarray at position {k-1}"
            );
        }
    }
}