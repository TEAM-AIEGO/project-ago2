using UnityEngine;

enum MeleeState
{
    idle,
    moving,
    attacking
}

[RequireComponent(typeof(Rigidbody))]
public class MeleeEnemy : Unit
{
    private GameObject player;
    private MeleeState meleeState;
    private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        switch (meleeState)
        {
            case MeleeState.idle:
                Idle();
                break;
            case MeleeState.moving:
                Moving();
                break;
            case MeleeState.attacking:
                Attacking();
                break;
        }
    }

    void Idle()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 10)
        {
            meleeState = MeleeState.moving;
        }
    }

    void Moving()
    {
        var direction = player.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        rb.linearVelocity = direction;
        if (Vector3.Distance(player.transform.position, transform.position) < 2)
        {
            meleeState = MeleeState.attacking;
        }
    }

    void Attacking()
    {
        print("atk");
        meleeState = MeleeState.moving;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        Debug.Log($"Melee Enemy took {damage} damage. Remaining Health: {Health}");
    }
}
