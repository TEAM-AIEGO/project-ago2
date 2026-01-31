using Unity.Mathematics;
using UnityEngine;

public class PlayerGroundPoundAction : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField] private float maxHeight;
    [SerializeField] private float groundPoundForce;
    [SerializeField] private float upwardForce;
    [SerializeField] private float damage;

    private float groundPoundStartHeight;

    private Rigidbody rb;

    public void Initialized(Rigidbody rb)
    {
        this.rb = rb;
    }

    public void GroundPound()
    {
        groundPoundStartHeight = transform.position.y;
        rb.linearVelocity = Vector3.down * groundPoundForce; //gpspeed
    }

    public void GroundPoundKaboom()
    {
        float kaboomPower = Mathf.Clamp((groundPoundStartHeight - transform.position.y) / maxHeight, 0, 1);
        Debug.Log($"GP KABOOM POWER: {kaboomPower}");
        Collider[] hitColliders = Physics.OverlapBox(transform.position - Vector3.down * 0.5f, new Vector3(7.5f, 10f, 7.5f), quaternion.identity, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject currentObject = hitColliders[i].gameObject;
            Debug.Log(currentObject);
            if (!currentObject.TryGetComponent(out Unit unit) || !currentObject.TryGetComponent(out Rigidbody rb)) continue;
            rb.AddForce(Vector3.up * upwardForce * kaboomPower);
            unit.TakeDamage(damage * kaboomPower);
        }
    }
}
