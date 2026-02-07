using UnityEngine;
using System;

public class ScanDoorPoint : MonoBehaviour
{
    [HideInInspector] public event Action OnScanComplete;
    [HideInInspector] public event Action OnScanStart;

    [SerializeField] private float scanDuration = 5.0f;
    private float scanTimer = 0.0f;
    private int scanCount = 0;

    private bool isScanning = false;

    public void OnCanScanning() => gameObject.SetActive(true);

    public void OnDoorPassed() => gameObject.SetActive(false);

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
                scanDuration *= (++scanCount + 1);

                Debug.Log("He is Dumb X " + scanCount);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isScanning)
            return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Door Active");
            OnScanStart.Invoke();
            isScanning = true;
            scanTimer = 0.0f;
        }
    }
}