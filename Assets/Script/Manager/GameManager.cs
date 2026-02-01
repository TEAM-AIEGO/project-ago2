using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

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
    public GameObject StageTarget;
    public List<EnemySpawnData> Enemies;
}

public class GameManager : MonoBehaviour
{

    public List<Stage> Stages;

    #region Warping Stages
    /// <summary>
    /// 0. ��Ʋ�� 1�ܰ�
    /// 1. ��Ʋ�� �߰� �ܰ�
    /// 2. ��Ʋ�� 2�ܰ�
    /// </summary>
    public int WarpStage = 0;
    #endregion

    //
    //
    //

    [SerializeField] private ObjectPool objectPool;
    private int currentStageIndex = 0;
    private int StageEnemyLeft = 0;

    void Start()
    {
        StartStage(0);
    }

    void StartStage(int stageIndex)
    {
        if (Stages[stageIndex] == default(Stage)) return;
        var currentStage = Stages[stageIndex];
        currentStage.StageState = StageState.Loading;
        currentStageIndex = stageIndex;
        StageEnemyLeft = currentStage.Enemies.Count;
        for (int i = 0; i < currentStage.Enemies.Count; i++)
        {
            var currentEnemySpawnData = currentStage.Enemies[i];
            //GameObject currentEnemy = Instantiate(currentEnemySpawnData.EnemyPrefab, currentEnemySpawnData.EnemySpawnPosition, quaternion.identity);
            //if (currentEnemy.TryGetComponent(out EnemyBase enemyBase))
            //{
            //    enemyBase.Initialize(enemyBase);
            //}
            //currentEnemy.GetComponent<Unit>().Died.AddListener(OnStageEnemyKilled);

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
        if (StageEnemyLeft <= 0)
        {
            Stages[currentStageIndex].StageState = StageState.Ended;
            Stages[currentStageIndex].StageTarget.SetActive(false);
            StartStage(++currentStageIndex);
        }
    }

    void Update()
    {
        
    }
}
