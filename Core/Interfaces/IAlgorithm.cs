namespace Core.Interfaces;

using Models;

public interface IAlgorithm {
    AnimationState Execute(int[] array);
    IEnumerable<AnimationState> ExecuteWithSteps(int[] array);
}