namespace Core.Algorithms.Sorting;

using Interfaces;

public sealed class SelectionSort : ISortingAlgorithm {
    public string Name => "Selection Sort";
    public int Speed { get; set; } = 50;
    
    object IAlgorithm.Execute(object input) {
        if (input is not int[] array) {
            throw new ArgumentException("Input must be an integer array", nameof(input));
        }
        return Execute(array);
    }
    
    IEnumerable<object> IAlgorithm.ExecuteWithSteps(object input) {
        if (input is not int[] array) {
            throw new ArgumentException("Input must be an integer array", nameof(input));
        }
        return ExecuteWithSteps(array);
    }
    
    public int[] Execute(int[] array) {
        var result = (int[])array.Clone();
        
        for (int i = 0; i < result.Length - 1; i++) {
            var minIndex = i;
            
            for (int j = i + 1; j < result.Length; j++) {
                if (result[j] < result[minIndex]) {
                    minIndex = j;
                }
            }
            
            if (minIndex != i) {
                (result[i], result[minIndex]) = (result[minIndex], result[i]);
            }
        }
        
        return result;
    }
    
    public IEnumerable<int[]> ExecuteWithSteps(int[] array) {
        var result = (int[])array.Clone();
        yield return (int[])result.Clone();
        
        for (int i = 0; i < result.Length - 1; i++) {
            var minIndex = i;
            
            for (int j = i + 1; j < result.Length; j++) {
                if (result[j] < result[minIndex]) {
                    minIndex = j;
                }
                yield return (int[])result.Clone();
            }
            
            if (minIndex != i) {
                (result[i], result[minIndex]) = (result[minIndex], result[i]);
                yield return (int[])result.Clone();
            }
        }
    }
} 