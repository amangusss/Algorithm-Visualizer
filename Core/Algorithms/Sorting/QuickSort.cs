namespace Core.Algorithms.Sorting;

using Interfaces;

public sealed class QuickSort : ISortingAlgorithm {
    public string Name => "Quick Sort";
    public int Speed { get; set; } = 50;
    
    public int[] Execute(int[] array) {
        return ExecuteWithSteps(array).Last();
    }
    
    public IEnumerable<int[]> ExecuteWithSteps(int[] array) {
        var steps = new List<int[]> { (int[])array.Clone() };
        QuickSortInternal(array, 0, array.Length - 1, steps);
        return steps;
    }

    private void QuickSortInternal(int[] array, int low, int high, List<int[]> steps) {
        if (low < high) {
            var pivotIndex = Partition(array, low, high, steps);
            QuickSortInternal(array, low, pivotIndex - 1, steps);
            QuickSortInternal(array, pivotIndex + 1, high, steps);
        }
    }

    private int Partition(int[] array, int low, int high, List<int[]> steps) {
        var pivot = array[high];
        var i = low - 1;

        for (var j = low; j < high; j++) {
            if (array[j] <= pivot) {
                i++;
                Swap(array, i, j);
                steps.Add((int[])array.Clone());
            }
        }

        Swap(array, i + 1, high);
        steps.Add((int[])array.Clone());
        return i + 1;
    }

    private static void Swap(int[] array, int i, int j) {
        (array[i], array[j]) = (array[j], array[i]);
    }

    public object Execute(object input) => Execute((int[])input);
    
    IEnumerable<object> IAlgorithm.ExecuteWithSteps(object input) {
        return ExecuteWithSteps((int[])input);
    }
}