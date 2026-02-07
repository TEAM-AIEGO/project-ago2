using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private HitMarker hitMarker;
    [SerializeField] private Image DoorAlarm;

    private void Start()
    {
        if (hitMarker == null)
        {
            Debug.LogError("hitMarker is NUll");
        }
    }
    public void ShowHitMarker()
    {
        hitMarker.OnHit();
    }

    public void ActiveDoorAlarm(bool isActive)
    {
        if (isActive)
            DoorAlarm.gameObject.SetActive(false);
        else
            DoorAlarm.gameObject.SetActive(true);
    }
}