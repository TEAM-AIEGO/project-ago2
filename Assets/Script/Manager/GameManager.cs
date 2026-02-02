using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public bool IsGameOver { get; private set; }

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

    [SerializeField] private StageManager stageManager;


    private void Awake()
    {
        stageManager.Initialize();

    }

    void Update()
    {
        
    }

    public void SetGameOver()
    {
        IsGameOver = true;
    }
}
