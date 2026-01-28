using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    [SerializeField] private Image hitMarker;

    private float remainingTime;
    [SerializeField] private float displayDuration;

    public void OnHit()
    {
        hitMarker.enabled = true;
        remainingTime = displayDuration;
    }

    private void Update()
    {
        if (!hitMarker.enabled) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            hitMarker.enabled = false;
        }
    }
}