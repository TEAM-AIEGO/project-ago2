using Unity.Mathematics;
using UnityEngine;

public class PlayerGroundPoundAction : MonoBehaviour
{

    [SerializeField] private float maxHeight;
    [SerializeField] private float groundPoundForce;
    [SerializeField] private float upwardForce;
    [SerializeField] private float damage;

    private float groundPoundStartHeight;

    private Rigidbody rb;
    private SFXEmitter emitter;

    public void Initialized(Rigidbody rb, SFXEmitter emitter)
    {
        this.rb = rb;
        this.emitter = emitter;
    }

    public void GroundPound()
    {
        groundPoundStartHeight = transform.position.y;
        rb.linearVelocity = Vector3.down * groundPoundForce; //gpspeed
        emitter.Play("Ground_Pound_Start");
    }

    public void GroundPoundKaboom()
    {
        float kaboomPower = Mathf.Clamp((groundPoundStartHeight - transform.position.y) / maxHeight, 0, 1);
        emitter.Play("Ground_Pound_Land");
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
