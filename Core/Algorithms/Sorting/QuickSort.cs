namespace Algorithm_Visualizer.Core.Algorithms.Sorting;

using Interfaces;

public sealed class QuickSort : ISortingAlgorithm {
    public string Name => "Quick Sort";
    public int Speed { get; set; } = 30;
    private List<int[]> _steps = new();
    
    public int[] Execute(int[] array) {
        return ExecuteWithSteps(array).Last();
    }
    
    public IEnumerable<int[]> ExecuteWithSteps(int[] array) {
        _steps.Clear();
        _steps.Add((int[])array.Clone());
        QuickSortInternal(array, 0, array.Length - 1);
        return _steps;
    }

    private void QuickSortInternal(int[] array, int low, int high) {
        if (low < high) {
            int pivot = Partition(array, low, high);
            QuickSortInternal(array, low, pivot - 1);
            QuickSortInternal(array, pivot + 1, high);
        }
    }

    private int Partition(int[] array, int low, int high) {
        int pivot = array[high];
        int i = low - 1;

        for (int j = low; j < high; j++) {
            if (array[j] < pivot) {
                i++;
                Swap(ref array[i], ref array[j]);
                _steps.Add((int[])array.Clone());
            }
        }
        Swap(ref array[i + 1], ref array[high]);
        _steps.Add((int[])array.Clone());
        return i + 1;
    }

    private static void Swap(ref int a, ref int b) => (a, b) = (b, a);

    public object Execute(object input) => ExecuteWithSteps((int[])input);
    
    IEnumerable<object> IAlgorithm.ExecuteWithSteps(object input) {
        return ExecuteWithSteps((int[])input).Cast<object>();
    }
}