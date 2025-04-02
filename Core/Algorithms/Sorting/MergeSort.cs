namespace Algorithm_Visualizer.Core.Algorithms.Sorting;

using Interfaces;

public sealed class MergeSort : ISortingAlgorithm {
    public string Name => "Merge Sort";
    public int Speed { get; set; } = 40;
    private List<int[]> _steps = new();
    
    public int[] Execute(int[] array) => ExecuteWithSteps(array).Last();

    public IEnumerable<int[]> ExecuteWithSteps(int[] array) {
        _steps.Clear();
        _steps.Add((int[])array.Clone());
        MergeSortInternal(array, 0, array.Length - 1);
        return _steps;
    }

    private void MergeSortInternal(int[] array, int left, int right) {
        if (left < right) {
            int mid = left + (right - left) / 2;
            MergeSortInternal(array, left, mid);
            MergeSortInternal(array, mid + 1, right);
            Merge(array, left, mid, right);
        }
    }

    private void Merge(int[] array, int left, int mid, int right) {
        int[] temp = new int[right - left + 1];
        int i = left, j = mid + 1, k = 0;

        while (i <= mid && j <= right) {
            temp[k++] = array[i] <= array[j] ? array[i++] : array[j++];
        }

        while (i <= mid) temp[k++] = array[i++];
        while (j <= right) temp[k++] = array[j++];

        for (i = left; i <= right; i++) {
            array[i] = temp[i - left];
            _steps.Add((int[])array.Clone());
        }
    }

    public object Execute(object input) => ExecuteWithSteps((int[])input);
    
    IEnumerable<object> IAlgorithm.ExecuteWithSteps(object input) {
        return ExecuteWithSteps((int[])input).Cast<object>();
    }
}