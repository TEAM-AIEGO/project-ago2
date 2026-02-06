using UnityEngine;

public class InDoorCheck : MonoBehaviour
{
    [HideInInspector] public event System.Action OnDoorCheck;

    private bool isChecked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isChecked)
            return;

        if (other.CompareTag("Player"))
        {
            OnDoorCheck.Invoke();
            isChecked = true;
        }
    }
}
