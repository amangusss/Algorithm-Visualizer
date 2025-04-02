namespace Algorithm_Visualizer.Core.Algorithms.Sorting;

using Interfaces;

public sealed class BubbleSort : ISortingAlgorithm {
    public string Name => "Bubble Sort";
    public int Speed { get; set; } = 50;

    public int[] Execute(int[] array) => ExecuteWithSteps(array).Last();

    public IEnumerable<int[]> ExecuteWithSteps(int[] array) {
        var steps = new List<int[]> { (int[])array.Clone() };
        for (int i = 0; i < array.Length - 1; i++) {
            for (int j = 0; j < array.Length - i - 1; j++) {
                if (array[j] > array[j + 1]) {
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    steps.Add((int[])array.Clone());
                }
            }
        }
        return steps;
    }

    object IAlgorithm.Execute(object input) => Execute((int[])input);

    IEnumerable<object> IAlgorithm.ExecuteWithSteps(object input) => 
        ExecuteWithSteps((int[])input).Cast<object>();
}