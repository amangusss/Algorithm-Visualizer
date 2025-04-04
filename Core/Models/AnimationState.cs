namespace Core.Models;

public class AnimationState {
    public int[] Array { get; }
    public HashSet<int> ActiveIndices { get; }
    public HashSet<int> SwappedIndices { get; }
    public string Description { get; }
    
    public AnimationState(int[] array, HashSet<int> activeIndices, HashSet<int> swappedIndices, string description) {
        Array = array;
        ActiveIndices = activeIndices;
        SwappedIndices = swappedIndices;
        Description = description;
    }
}
