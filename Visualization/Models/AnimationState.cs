namespace Visualization.Models;

public class AnimationState {
    public int[] Array { get; }
    public HashSet<int> SwappedIndices { get; }
    public HashSet<int> ActiveIndices { get; }

    public AnimationState(int[] array, HashSet<int> swappedIndices, HashSet<int> activeIndices) {
        Array = array;
        SwappedIndices = swappedIndices;
        ActiveIndices = activeIndices;
    }
} 