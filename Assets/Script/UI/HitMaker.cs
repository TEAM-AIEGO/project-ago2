using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitMaker : MonoBehaviour
{
    [SerializeField] private Image hitMarker;

    private Coroutine hitMakerCoroutine;

    public void OnHit()
    {
        if (hitMakerCoroutine != null)
            StopCoroutine(hitMakerCoroutine);

        hitMakerCoroutine = StartCoroutine(EnableHitMaker());
    }

    private IEnumerator EnableHitMaker()
    {
        hitMarker.enabled = true;
        yield return new WaitForSeconds(0.2f);
        hitMarker.enabled = false;
    }
}