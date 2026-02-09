using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public enum StageState
{
    Loading,
    Fighting,
    Ended
}

[Serializable]
public struct EnemySpawnData
{
    public GameObject EnemyPrefab;
    public Vector3 EnemySpawnPosition;
}

[Serializable]
public class Stage
{
    public StageState StageState;
    public GameObject StageObject;
    public GameObject WarpStageObject;
    public StageDoor StageDoor;
    public List<EnemySpawnData> Enemies;
}

public class StageManager : MonoBehaviour, IWarpObserver
{
    public List<Stage> Stages;
    public bool IsGameOver { get; private set; }

    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private WarpSystemManager warpSystemManager;
    [SerializeField] private Playthething ptt;
    private int currentStageIndex = 0;
    private int StageEnemyLeft = 0;

    public void Initialize()
    {
        currentStageIndex = 0;
        StageEnemyLeft = 0;

        if (warpSystemManager == null)
        {
            Debug.LogError("WarpSystemManager is not assigned in StageManager.");
            return;
        }

        if (objectPool == null)
        {
            Debug.LogError("ObjectPool is not assigned in StageManager.");
            return;
        }

        warpSystemManager.RegisterWarpObserver(this);
        StartStage(currentStageIndex);
    }

    private void OnDisable()
    {
        warpSystemManager.UnregisterWarpObserver(this);
    }

    public void StartStage(int stageIndex)
    {
        if (Stages[stageIndex] == default(Stage)) return;
        var currentStage = Stages[stageIndex];
        currentStage.StageState = StageState.Loading;
        currentStageIndex = stageIndex;
        StageEnemyLeft = currentStage.Enemies.Count;
        Debug.Log("Enemy Count : " + currentStage.Enemies.Count);
        for (int i = 0; i < currentStage.Enemies.Count; i++)
        {
            var currentEnemySpawnData = currentStage.Enemies[i];

            EnemyBase currentEnemy = objectPool.SpawnEnemy(currentEnemySpawnData.EnemyPrefab.GetComponent<EnemyBase>(), currentEnemySpawnData.EnemySpawnPosition);
            RegisterEnemy(currentEnemy, false);
        }
        currentStage.StageState = StageState.Fighting;
    }

    private void RegisterEnemy(EnemyBase enemy, bool countForStage)
    {
        if (enemy == null)
        {
            return;
        }

        enemy.GetComponent<Unit>().Died.AddListener(OnStageEnemyKilled);

        if (countForStage)
        {
            StageEnemyLeft++;
        }

        if (enemy is NegromancyEnemy negromancyEnemy)
        {
            negromancyEnemy.SpawnRequested += HandleNegromancySpawnRequest;
        }
    }

    private void HandleNegromancySpawnRequest(EnemySpawnRequest request)
    {
        if (request.Prefab == null)
        {
            return;
        }

        EnemyBase spawnedEnemy = objectPool.SpawnEnemy(request.Prefab, request.Position);
        RegisterEnemy(spawnedEnemy, true);
    }

    void OnStageEnemyKilled()
    {
        StageEnemyLeft--;
        Debug.Log("Enemy Killed. Enemies Remaining : " + StageEnemyLeft);
        if (StageEnemyLeft <= 0)
        {
            Debug.Log("Stage C;ear");
            Stages[currentStageIndex].StageState = StageState.Ended;

            if (Stages[currentStageIndex].StageDoor.IsScanDoor)
                Stages[currentStageIndex].StageDoor.SetScan();
            else
                Stages[currentStageIndex].StageDoor.OpenDoor();

            if (++currentStageIndex != Stages.Count)
                StartStage(currentStageIndex);
            else
            {
                Debug.Log("All Stages Cleared!");
                // Ending Cutscene Play
                if (ptt != null)
                    ptt.PlaythethingPlz();
            }
        }
    }

    public void OnWarpStageChanged(int newStage)
    {
        StageChange(newStage);
    }

    private void StageChange(int newStageIndex)
    {
        //
    }
}
