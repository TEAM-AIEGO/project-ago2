using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 50f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }
}