using UnityEngine;

public class StageDoor : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;

    public void OpenDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }
    }

    public void CloseDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Close");
        }
    }
}
