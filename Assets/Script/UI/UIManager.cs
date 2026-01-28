using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private HitMarker hitMarker;
    public void ShowHitMarker()
    {
        hitMarker.OnHit();
    }
}