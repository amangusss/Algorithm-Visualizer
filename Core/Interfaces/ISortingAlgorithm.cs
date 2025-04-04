namespace Core.Interfaces;

using Models;

public interface ISortingAlgorithm : IAlgorithm {
    new AnimationState Execute(int[] array);
    new IEnumerable<AnimationState> ExecuteWithSteps(int[] array);
    string Name { get; }
    string Description { get; }
}  