using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private Renderer[] targetRenderer;
    [SerializeField] private float flashDuration = 0.1f;

    private List<Material> materials = new();
    private List<Color> originalColors = new();

    private Coroutine flashCoroutine;

    void Awake()
    {
        for (int i = 0; i < targetRenderer.Length; i++)
        {
            materials.Add(targetRenderer[i].material);
        }

        for (int i = 0; i < materials.Count; i++)
        {
            originalColors.Add(materials[i].color);
        }
        //material = targetRenderer.material;
        //originalColor = material.color;
    }

    public void Flash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        for( int i = 0; i < materials.Count; i++)
        {
            materials[i].color = Color.red;
        }

        yield return new WaitForSeconds(flashDuration);

        for ( int i = 0 ; i < materials.Count; i++)
        {
            materials[i].color = originalColors[i];
        }
    }
}
