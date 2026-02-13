using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StageDoor : MonoBehaviour
{
    [HideInInspector] public event Action<int> OnDoorClose;
    public int nextStageIndex;

    [SerializeField] private BoxCollider doorCol;
    [SerializeField] private BoxCollider[] wallCols;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private bool isScanDoor = false;
    public bool IsScanDoor => isScanDoor;
    [SerializeField] private ScanDoorPoint scanDoorPoint;
    [SerializeField] private InDoorCheck inDoorCheck;

    private SFXEmitter emitter;
    
    private UIManager uiManager;

    private void Awake()
    {
        emitter = GetComponent<SFXEmitter>();
    }

    private void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        if (!uiManager)
        {
            Debug.LogWarning("UIManager not found! Hitmarker will not show.");
        }

        if (doorAnimator == null)
        {
            Debug.LogError("Door Animator is not assigned in StageDoor.");
        }

        if (scanDoorPoint != null)
        {
            scanDoorPoint.OnScanStart += OpenDoor;
            scanDoorPoint.OnScanComplete += CloseDoor;
        }

        if (inDoorCheck != null)
        {
            inDoorCheck.OnDoorCheck += PassOverDoor;
        }

        doorCol.enabled = true;

        if (wallCols != null)
        {
            for (int i = 0; i < wallCols.Length; i++)
            {
                wallCols[i].enabled = true;
            }
        }
    }

    public void OpenDoor()
    {
        emitter.PlayFollow(AudioIds.AirlockDoorOpenAndClose, transform);
        doorAnimator.SetTrigger("Open");
        uiManager.ActiveDoorAlarm(false);
        doorCol.enabled = false;
    }

    public void TPNextStage()
    {
        OnDoorClose.Invoke(nextStageIndex);
        //doorAnimator.SetTrigger("Close");
        uiManager.ActiveDoorAlarm(true);
    }

    public void CloseDoor()
    {
        emitter.PlayFollow(AudioIds.AirlockDoorOpenAndClose, transform);
        doorAnimator.SetTrigger("Close");
        uiManager.ActiveDoorAlarm(true);
        OnDoorClose?.Invoke(nextStageIndex);
        doorCol.enabled = true;
    }

    public void SetScan()
    {
        scanDoorPoint.OnCanScanning();

        for (int i = 0; i < wallCols.Length; i++)
        {
            wallCols[i].enabled = true;
        }
    }

    public void PassOverDoor()
    {
        CloseDoor();

        if (scanDoorPoint != null)
            scanDoorPoint.OnDoorPassed();
    }
}
