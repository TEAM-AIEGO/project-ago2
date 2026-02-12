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
    [SerializeField] private GameObject[] frames;
    [SerializeField] private GameObject[] statusBarFrames;
    [SerializeField] private float transitionTime;
    private float currentTransitionTime;
    private bool isWarping;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager)
        {
            gameManager.WarpStageChanged.AddListener(GetWarped);
            gameManager.WarpStageChanged.AddListener(SetFrame);
        }
        SetFrame(gameManager.WarpStage);
        
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

    public void SetFrame(int stage)
    {
        stage = gameManager.WarpStage;
        for (int i = 0; i < frames.Length; i++)
        {
            frames[i].SetActive(false);
            statusBarFrames[i].SetActive(false);
        }
        if (stage == 1)
        {
            frames[0].SetActive(true);
            statusBarFrames[0].SetActive(true);
        }
        else if (stage == 2)
        {
            frames[1].SetActive(true);
            statusBarFrames[1].SetActive(true);
        }
        else
        {
            statusBarFrames[0].SetActive(true);
        }
    }
}