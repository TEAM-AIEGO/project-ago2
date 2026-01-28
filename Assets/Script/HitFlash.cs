using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private float flashDuration = 0.1f;

    private Material material;
    private Color originalColor;

    void Awake()
    {
        material = targetRenderer.material;
        originalColor = material.color;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        material.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        material.color = originalColor;
    }
}
