/// <summary>
/// Interface for stackable objects.
/// </summary>
public interface IStackable
{
    public string StackFamily { get;}
    public bool OnStack { get; }

    public void ActiveOnStackState();
}
