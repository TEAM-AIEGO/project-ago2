using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private HitMarker hitMarker;
    [SerializeField] private Image DoorAlarm;
    [SerializeField] private Image SubWeaponFrame;
    [SerializeField] private GameObject subtitleRoot;
    [SerializeField] private TMP_Text subtitleText;

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

    public void ActiveSubWeapon()
    {
        if (SubWeaponFrame.gameObject.activeSelf)
            SubWeaponFrame.gameObject.SetActive(false);
        else
            SubWeaponFrame.gameObject.SetActive(true);
    }

    public void SetSubtitleActive(bool isActive)
    {
        if (subtitleRoot == null)
        {
            return;
        }

        subtitleRoot.SetActive(isActive);
    }

    public void SetSubtitleText(string text)
    {
        if (subtitleText == null)
        {
            return;
        }

        subtitleText.text = $"{text}";
    }
}