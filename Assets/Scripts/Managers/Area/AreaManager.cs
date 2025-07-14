using System;
using System.Collections.Generic;
using UnityEngine;
using static AreaEnum;

/// <summary>
/// This script is used to manage several areas in the game, the areas are committed to 
/// entering the list so that other scripts can use them without direct reference.
/// </summary>
public class AreaManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] public static AreaManager Instance;

    [Header("Temp Data")]
    [SerializeField] private Dictionary<AreasTypes, IArea> _areaDict = new();

    #region Start Methods
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    #region Manager Methods
    /// <summary>
    /// Register Area in Dictionary.
    /// </summary>
    /// <param name="type">Type of area</param>
    /// <param name="area">Script of area that use IArea</param>
    public void RegisterInDictionary(AreasTypes type, IArea area)
    {
        _areaDict.Add(type, area);
    }

    /// <summary>
    /// Register an Observer to especific area in dictionary.
    /// </summary>
    /// <param name="area">Area type to observer</param>
    /// <param name="action">Action to invoke</param>
    public void RegisterObserver(AreasTypes area, Action<int> action)
    {
        if (_areaDict.TryGetValue(area, out var areaHandler))
        {
            areaHandler.RegisterObserver(action);
        }
        else
        {
            Debug.LogWarning($"[RegisterObserver] Area '{area}' not found in dictionary.");
        }
    }

    /// <summary>
    /// Unregister an Observer to especific area in dictionary.
    /// </summary>
    /// <param name="area">Area type to observer</param>
    /// <param name="action">Action to invoke</param>
    public void UnregisterObserver(AreasTypes area, Action<int> action)
    {
        if (_areaDict.TryGetValue(area, out var areaHandler))
        {
            areaHandler.UnregisterObserver(action);
        }
        else
        {
            Debug.LogWarning($"[UnregisterObserver] Area '{area}' not found in dictionary.");
        }
    }
    #endregion
}
