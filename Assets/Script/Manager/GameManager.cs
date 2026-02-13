using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public bool IsGameOver { get; private set; }
    #region Warping Stages    
    [SerializeField] private int warpStage = 0;
    private int catsFound = 0;
    public int WarpStage
    {
        get => warpStage;
        set 
        {
            if (warpStage != value)
            {
                warpStage = value;
                WarpStageChanged?.Invoke(warpStage);
            }
        }
    }
    public UnityEvent<int> WarpStageChanged;
    public UnityEvent CatFound;
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

    public void OnCatFound()
    {
        catsFound++;
        Debug.Log("gamemanager cat");
        CatFound?.Invoke();
    }
}
