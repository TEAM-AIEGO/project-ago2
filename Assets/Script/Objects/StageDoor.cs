using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StageDoor : MonoBehaviour
{
    [SerializeField] private BoxCollider col;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private bool isScanDoor = false;
    public bool IsScanDoor => isScanDoor;
    [SerializeField] private ScanDoorPoint scanDoorPoint;
    [SerializeField] private InDoorCheck inDoorCheck;

    private void Start()
    {
        if (doorAnimator == null)
        {
            Debug.LogError("Door Animator is not assigned in StageDoor.");
        }

        if (isScanDoor && scanDoorPoint == null)
        {
            Debug.LogError("ScanDoorPoint is not assigned for a scan door in StageDoor.");
            return;
        }
        else if (scanDoorPoint != null)
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
        doorAnimator.SetTrigger("Open");
        col.enabled = false;
    }

    public void CloseDoor()
    {
        doorAnimator.SetTrigger("Close");
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
