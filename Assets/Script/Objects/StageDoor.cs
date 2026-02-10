using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StageDoor : MonoBehaviour
{
    [HideInInspector] public event Action<int> OnDoorOpen;
    public int nextStageIndex;

    [SerializeField] private BoxCollider col;
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
    }

    public void OpenDoor()
    {
        emitter.PlayFollow("airlock door open and close", transform);
        doorAnimator.SetTrigger("Open");
        uiManager.ActiveDoorAlarm(false);
        OnDoorOpen.Invoke(nextStageIndex);
        col.enabled = false;
    }

    public void CloseDoor()
    {
        emitter.PlayFollow("airlock door open and close", transform);
        doorAnimator.SetTrigger("Close");
        uiManager.ActiveDoorAlarm(true);
        col.enabled = true;
    }

    public void SetScan()
    {
        scanDoorPoint.OnCanScanning();
    }

    public void PassOverDoor()
    {
        CloseDoor();

        if (scanDoorPoint != null)
            scanDoorPoint.OnDoorPassed();
    }
}
