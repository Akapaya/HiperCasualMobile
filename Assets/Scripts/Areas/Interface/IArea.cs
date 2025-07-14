using System;

/// <summary>
/// Interface for scripts that are areas, they have area activation methods and observer records.
/// </summary>
public interface IArea
{
    void ActivateArea();
    void DeactivateArea();
    void RegisterObserver(Action<int> action);
    void UnregisterObserver(Action<int> action);
}
