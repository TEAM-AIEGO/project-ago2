using System;
using System.Collections.Generic;
using UnityEngine;

public class WarpSystemManager : MonoBehaviour
{
    private int currentWarpStage;

    private List<IWarpObserver> warpObserver = new();

    private event Action <int> OnWarpChange;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        currentWarpStage = 1;
        OnWarpChange += WarpStageChanged;
    }


    private void OnDisable()
    {
        OnWarpChange -= WarpStageChanged;
    }

    public int GetWarpStage() => currentWarpStage;

    private void WarpStageChanged(int warp)
    {
        currentWarpStage = warp;
        NotifyWarpPointChanged(currentWarpStage);
    }

    public void RegisterWarpObserver(IWarpObserver observer)
    {
        //오브젝트 풀이 적을 생성 할 때 대신 등록.
        if (!warpObserver.Contains(observer))
            warpObserver.Add(observer);
    }

    public void UnregisterWarpObserver(IWarpObserver observer)
    {
        //오브젝트 풀이 적을 비활성화 할 때 대신 해제.
        if (warpObserver.Contains(observer))
            warpObserver.Remove(observer);
    }

    public void NotifyWarpPointChanged(int currentWarpStage)
    {
        foreach (var observer in warpObserver)
        {
            observer.OnWarpStageChanged(currentWarpStage);
        }
    }
}
