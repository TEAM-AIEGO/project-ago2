using UnityEngine;

public class ScanDoorPoint : MonoBehaviour
{
    [HideInInspector] public event System.Action OnScanComplete;

    [SerializeField] private float scanDuration = 3.0f;
    private float scanTimer = 0.0f;

    private bool isScanning = false;

    public void OnCanScanning() => gameObject.SetActive(true);

    private void Update()
    {
        if (isScanning)
        {
            scanTimer += Time.deltaTime;
            if (scanTimer >= scanDuration)
            {
                OnScanComplete?.Invoke();
                isScanning = false;
                scanTimer = 0.0f;

                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isScanning = true;
            scanTimer = 0.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isScanning = false;
            scanTimer = 0.0f;
        }
    }
}
