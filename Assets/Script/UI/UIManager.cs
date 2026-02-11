using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private HitMarker hitMarker;
    [SerializeField] private Image DoorAlarm;
    [SerializeField] private Image SubWeaponFrame;
    [SerializeField] private GameObject subtitleRoot;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private Image transitionImage;

    [SerializeField] private float transitionTime;
    private float currentTransitionTime;
    private bool isWarping;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager)
        {
            gameManager.WarpStageChanged.AddListener(GetWarped);
        }
        
        if (hitMarker == null)
        {
            Debug.LogError("hitMarker is NUll");
        }
    }

    void Update()
    {
        currentTransitionTime -= Time.unscaledDeltaTime;
        if (currentTransitionTime <= 0 && isWarping)
        {
            WarpEnd();
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

    public void GetWarped(int stage)
    {
        Debug.Log("omg it dark");
        isWarping = true;
        transitionImage.enabled = true;
        currentTransitionTime = transitionTime;
        Time.timeScale = 0;
    }

    private void WarpEnd()
    {
        Debug.Log("omg it no dark");
        isWarping = false;
        transitionImage.enabled = false;
        Time.timeScale = 1;
    }
}