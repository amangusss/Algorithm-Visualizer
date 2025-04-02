namespace Algorithm_Visualizer.Core.Interfaces;

public interface ISortingAlgorithm : IAlgorithm {
    IEnumerable<int[]> ExecuteWithSteps(int[] array);
}   