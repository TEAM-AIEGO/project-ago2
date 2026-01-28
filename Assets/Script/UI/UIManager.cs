using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private HitMarker hitMarker;
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
}