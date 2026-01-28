using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private float speed;
    [SerializeField] private float returnDelay;

    private Vector3 startPos;
    private bool isPlayerOn;
    private Coroutine returnCoroutine;

    void Awake()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isPlayerOn)
        {
            MoveTo(startPos + moveOffset);
        }
    }

    void MoveTo(Vector3 targetPos)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        collision.transform.SetParent(transform);
        isPlayerOn = true;

        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        collision.transform.SetParent(null);
        isPlayerOn = false;

        returnCoroutine = StartCoroutine(ReturnAfterDelay());
    }

    IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSeconds(returnDelay);

        while (Vector3.Distance(transform.position, startPos) > 0.01f)
        {
            MoveTo(startPos);
            yield return null;
        }

        returnCoroutine = null;
    }
}