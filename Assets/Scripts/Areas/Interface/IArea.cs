using System;
using UnityEngine;

public interface IArea
{
    void ActivateArea();
    void DeactivateArea();
    void RegisterObserver(Action<int> action);
    void UnregisterObserver(Action<int> action);
}
