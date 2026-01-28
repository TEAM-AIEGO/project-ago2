using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private HitMarker hitMarker;

    private void Awake()
    {

    }

    private void Init()
    {

    }

    public void ShowHitMarker()
    {
        hitMarker.OnHit();
    }
}