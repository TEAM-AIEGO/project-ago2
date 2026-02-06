using UnityEngine;

public class StageDoor : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private bool isScanDoor = false;
    public bool IsScanDoor => isScanDoor;
    [SerializeField] private ScanDoorPoint scanDoorPoint;

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
            scanDoorPoint.OnScanComplete += ScanOpenDoor;
        }
    }

    public void OpenDoor()
    {
        doorAnimator.SetTrigger("Open");
    }

    public void CloseDoor()
    {
        doorAnimator.SetTrigger("Close");
    }

    public void SetScan()
    {
        scanDoorPoint.OnCanScanning();
    }

    public void ScanOpenDoor()
    {
        if (!isScanDoor)
            return;

        doorAnimator.SetTrigger("ScanOpen");
    }
}
