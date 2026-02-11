using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public bool IsGameOver { get; private set; }

    #region Warping Stages    
    private int warpStage = 0;
    public int WarpStage
    {
        get => warpStage;
        set 
        {
            if (warpStage != value)
            {
                WarpStageChanged?.Invoke(warpStage);
            }
            warpStage = value;
        }
    }
    public UnityEvent<int> WarpStageChanged;
    #endregion
    [SerializeField] private StageManager stageManager;


    private void Awake()
    {
        stageManager.Initialize();

    }

    public void SetGameOver()
    {
        IsGameOver = true;
    }
}
