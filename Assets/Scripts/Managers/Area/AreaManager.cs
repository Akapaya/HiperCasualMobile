using System;
using System.Collections.Generic;
using UnityEngine;
using static AreaEnum;

public class AreaManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] public static AreaManager Instance;

    [Header("Temp Data")]
    [SerializeField] private Dictionary<AreasTypes, IArea> _areaDict = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterInDictionary(AreasTypes type, IArea area)
    {
        _areaDict.Add(type, area);
    }

    public void RegisterObserver(AreasTypes area, Action<int> action)
    {
        _areaDict[area].RegisterObserver(action);
    }

    public void UnregisterObserver(AreasTypes area, Action<int> action)
    {
        _areaDict[area].UnregisterObserver(action);
    }
}
