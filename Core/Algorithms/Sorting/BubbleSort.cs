namespace Core.Algorithms.Sorting;

using Interfaces;

public sealed class BubbleSort : ISortingAlgorithm {
    public string Name => "Bubble Sort";
    public int Speed { get; set; } = 50;

    public int[] Execute(int[] array) => ExecuteWithSteps(array).Last();

    public IEnumerable<int[]> ExecuteWithSteps(int[] array) {
        var steps = new List<int[]> { (int[])array.Clone() };
        var n = array.Length;
        var swapped = true;

        for (var i = 0; i < n - 1 && swapped; i++) {
            swapped = false;
            for (var j = 0; j < n - i - 1; j++) {
                if (array[j] > array[j + 1]) {
                    Swap(array, j, j + 1);
                    swapped = true;
                    steps.Add((int[])array.Clone());
                }
            }
        }

        return steps;
    }

    private static void Swap(int[] array, int i, int j) {
        (array[i], array[j]) = (array[j], array[i]);
    }

    object IAlgorithm.Execute(object input) => Execute((int[])input);

    IEnumerable<object> IAlgorithm.ExecuteWithSteps(object input) => 
        ExecuteWithSteps((int[])input).Cast<object>();
}