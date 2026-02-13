using UnityEngine;
using UnityEngine.Events;

public class CollectableCat : MonoBehaviour, IHittable
{
    private GameManager gameManager;
    private SFXEmitter emitter;
    bool hasFound;

    public void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (!gameManager)
        {
            Debug.LogWarning("cat couldn't find gamemanager :sad:");
        }
        emitter = FindFirstObjectByType<SFXEmitter>();
        if (!emitter)
        {
            Debug.Log("no cat sound");
        }
    }

    public void TakeDamage(float BroWeDontNeedDamageForCat)
    {
        if (hasFound) return;
        hasFound = true;
        emitter.Play("Found");
        transform.rotation *= Quaternion.Euler(0, 0, 180);
        Debug.Log("cat got found");
        gameManager.OnCatFound();
    }
}
