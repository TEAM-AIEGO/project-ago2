using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    [SerializeField] private Image hitMarker;

    private Coroutine hitMakerCoroutine;

    public void OnHit()
    {
        if (hitMakerCoroutine != null)
            StopCoroutine(hitMakerCoroutine);

        hitMakerCoroutine = StartCoroutine(EnableHitMarker());
    }

    private IEnumerator EnableHitMarker()
    {
        hitMarker.enabled = true;
        yield return new WaitForSeconds(0.2f);
        hitMarker.enabled = false;
    }
}