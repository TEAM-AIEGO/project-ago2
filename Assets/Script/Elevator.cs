using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private float speed = 2f;

    private Vector3 startPos;
    private bool isPlayerOn;

    void Awake()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (!isPlayerOn)
            return;

        Vector3 targetPos = startPos + moveOffset;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            isPlayerOn = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            isPlayerOn = false;
        }
    }
}