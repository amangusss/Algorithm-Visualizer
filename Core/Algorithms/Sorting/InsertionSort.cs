namespace Core.Algorithms.Sorting;

using Interfaces;

public sealed class InsertionSort : ISortingAlgorithm {
    public string Name => "Insertion Sort";
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
        
        for (int i = 1; i < result.Length; i++) {
            var key = result[i];
            var j = i - 1;
            
            while (j >= 0 && result[j] > key) {
                result[j + 1] = result[j];
                j--;
            }
            
            result[j + 1] = key;
        }
        
        return result;
    }
    
    public IEnumerable<int[]> ExecuteWithSteps(int[] array) {
        var result = (int[])array.Clone();
        yield return (int[])result.Clone();
        
        for (int i = 1; i < result.Length; i++) {
            var key = result[i];
            var j = i - 1;
            
            while (j >= 0 && result[j] > key) {
                result[j + 1] = result[j];
                j--;
                yield return (int[])result.Clone();
            }
            
            result[j + 1] = key;
            yield return (int[])result.Clone();
        }
    }
} 