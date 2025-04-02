namespace Core.Algorithms.Sorting;

using Interfaces;

public sealed class MergeSort : ISortingAlgorithm {
    public string Name => "Merge Sort";
    public int Speed { get; set; } = 50;

    public int[] Execute(int[] array) => ExecuteWithSteps(array).Last();

    public IEnumerable<int[]> ExecuteWithSteps(int[] array) {
        var steps = new List<int[]> { (int[])array.Clone() };
        MergeSortInternal(array, 0, array.Length - 1, steps);
        return steps;
    }

    private void MergeSortInternal(int[] array, int left, int right, List<int[]> steps) {
        if (left < right) {
            var mid = left + (right - left) / 2;
            MergeSortInternal(array, left, mid, steps);
            MergeSortInternal(array, mid + 1, right, steps);
            Merge(array, left, mid, right, steps);
        }
    }

    private void Merge(int[] array, int left, int mid, int right, List<int[]> steps) {
        var leftLength = mid - left + 1;
        var rightLength = right - mid;
        
        var leftArray = new int[leftLength];
        var rightArray = new int[rightLength];
        
        for (var i = 0; i < leftLength; i++)
            leftArray[i] = array[left + i];
            
        for (var j = 0; j < rightLength; j++)
            rightArray[j] = array[mid + 1 + j];
            
        var l = 0;
        var o = 0;
        var k = left;
        
        while (l < leftLength && o < rightLength)
        {
            if (leftArray[l] <= rightArray[o])
            {
                array[k] = leftArray[l];
                l++;
            }
            else
            {
                array[k] = rightArray[o];
                o++;
            }
            k++;
            steps.Add((int[])array.Clone());
        }
        
        while (l < leftLength)
        {
            array[k] = leftArray[l];
            l++;
            k++;
            steps.Add((int[])array.Clone());
        }
        
        while (o < rightLength)
        {
            array[k] = rightArray[o];
            o++;
            k++;
            steps.Add((int[])array.Clone());
        }
    }

    public object Execute(object input) => Execute((int[])input);
    
    IEnumerable<object> IAlgorithm.ExecuteWithSteps(object input) => 
        ExecuteWithSteps((int[])input).Cast<object>();
}