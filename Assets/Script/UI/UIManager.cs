using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private HitMaker hitMarker;

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