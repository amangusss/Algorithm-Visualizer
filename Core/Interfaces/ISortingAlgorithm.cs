namespace Core.Interfaces;

public interface ISortingAlgorithm : IAlgorithm
{
    new int[] Execute(int[] array);
    new IEnumerable<int[]> ExecuteWithSteps(int[] array);
}  