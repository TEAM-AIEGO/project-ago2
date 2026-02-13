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
    public Transform EnemySpawnPosition;
}

[Serializable]
public class Stage
{
    public StageState StageState;
    public GameObject StageObject;
    public BoxCollider WallCollider;
    public Transform StageSpawnPosition;
    public GameObject WarpStageObject;
    public StageDoor StageInDoor;
    public StageDoor StageOutDoor;
    public GameObject StageFakeDoor;
    public List<EnemySpawnData> Enemies;
}

public class StageManager : MonoBehaviour, IWarpObserver
{
    public List<Stage> Stages;
    public bool IsGameOver { get; private set; }

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private BossCore bossCore;
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private WarpSystemManager warpSystemManager;
    [SerializeField] private Playthething ptt;
    [SerializeField] private int currentStageIndex = 0;
    private int StageEnemyLeft = 0;
    private Stage currentStage;

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

        Stages[stageIndex].StageFakeDoor?.SetActive(true);
        currentStage?.StageObject.SetActive(false);

        currentStage = Stages[stageIndex];
        currentStage.StageState = StageState.Loading;
        currentStageIndex = stageIndex;
        StageEnemyLeft = currentStage.Enemies.Count;
        Debug.Log("Enemy Count : " + currentStage.Enemies.Count);
        for (int i = 0; i < currentStage.Enemies.Count; i++)
        {
            var currentEnemySpawnData = currentStage.Enemies[i];

            EnemyBase currentEnemy = objectPool.SpawnEnemy(currentEnemySpawnData.EnemyPrefab.GetComponent<EnemyBase>(), currentEnemySpawnData.EnemySpawnPosition.position);
            RegisterEnemy(currentEnemy, false);
        }
        currentStage.StageState = StageState.Fighting;

        if (currentStage.StageSpawnPosition != null)
            playerManager.transform.position = currentStage.StageSpawnPosition.position;
    }

    public void BossStageStart(int stageIndex)
    {
        print("vossifuhad");
        //currentStage?.StageObject.SetActive(false);

        currentStage = Stages[^1];
        currentStage.StageState = StageState.Loading;
        currentStageIndex = Stages.Count - 1;

        StageEnemyLeft = 1;
        bossCore.gameObject.SetActive(true);
        bossCore.Initialize(warpSystemManager.GetWarpStage());
        RegisterEnemy(bossCore, false);

        currentStage.StageState = StageState.Fighting;
        currentStage.StageObject.SetActive(true);
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
            Debug.Log("$tage C;ear");
            Stages[currentStageIndex].StageState = StageState.Ended;
            Stages[currentStageIndex].WallCollider.enabled = false;

            if (Stages[currentStageIndex].StageOutDoor)
                Stages[currentStageIndex].StageOutDoor.SetScan();

            if (currentStageIndex >= 0 && currentStageIndex < Stages.Count)
            {
                if (currentStageIndex + 1 == Stages.Count - 1)
                {
                    Debug.Log("Last $tage");
                    int next = currentStageIndex + 1;
                    Stages[next].StageObject.SetActive(true);
                    BossStageStart(warpSystemManager.GetWarpStage());
                    //Stages[next].StageInDoor.OnDoorClose += BossStageStart;
                    Stages[next].StageInDoor.nextStageIndex = next;
                }
                else
                {
                    int next = currentStageIndex + 1;
                    Stages[next].StageObject.SetActive(true);
                    Stages[next].StageInDoor.OnDoorClose += StartStage; 
                    Stages[next].StageInDoor.nextStageIndex = next;
                }
            }
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
