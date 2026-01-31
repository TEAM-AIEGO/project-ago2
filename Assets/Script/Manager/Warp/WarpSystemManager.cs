using System;
using System.Collections.Generic;
using UnityEngine;

public class WarpSystemManager : MonoBehaviour
{
    private int currentWarpStage;

    private List<IWarpObserver> warpObservers = new();

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        currentWarpStage = 0;
    }


    private void OnDisable()
    {

    }

    public int GetWarpStage() => currentWarpStage;
    public void SetChageWarpStage(int warp) => WarpStageChanged(warp);

    private void WarpStageChanged(int warp)
    {
        currentWarpStage = warp;
        NotifyWarpPointChanged(currentWarpStage);
    }

    public void RegisterWarpObserver(IWarpObserver observer)
    {
        //오브젝트 풀이 적을 생성 할 때 대신 등록.
        if (!warpObservers.Contains(observer))
            warpObservers.Add(observer);
    }

    public void UnregisterWarpObserver(IWarpObserver observer)
    {
        //오브젝트 풀이 적을 비활성화 할 때 대신 해제.
        if (warpObservers.Contains(observer))
            warpObservers.Remove(observer);
    }

    public void NotifyWarpPointChanged(int currentWarpStage)
    {
        foreach (var observer in warpObservers)
        {
            observer.OnWarpStageChanged(currentWarpStage);
        }
    }
}
