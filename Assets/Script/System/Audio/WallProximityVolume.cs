using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WallProximityVolume : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioLibrary audioLibrary;
    [SerializeField] private string audioId = AudioIds.SFXGasLeak;

    [Header("Detection")]
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float updateInterval;
    [SerializeField] private int rayCount;
    [SerializeField] private float rayOriginHeightOffset;

    [Header("Volume")]
    [SerializeField] private float farVolume;
    [SerializeField] private float nearVolume;

    private AudioSource targetAudioSource;

    private float nextUpdateTime;

    private void Awake()
    {
        targetAudioSource = GetComponent<AudioSource>();
        SetupGasLeakAudio();
        ApplyVolume(0f);
    }

    private void Update()
    {
        if (Time.time < nextUpdateTime)
        {
            return;
        }

        nextUpdateTime = Time.time + updateInterval;

        float nearestWallDistance = FindNearestWallDistanceByRadialRays();
        float proximity = Mathf.Clamp01(1f - (nearestWallDistance / detectionRadius));
        ApplyVolume(proximity);
    }

    private void SetupGasLeakAudio()
    {
        if (audioLibrary == null)
        {
            Debug.LogWarning($"{nameof(WallProximityVolume)}: AudioLibrary is not assigned.", this);
            return;
        }

        if (!audioLibrary.TryGet(audioId, out AudioEntry entry) || entry.clip == null)
        {
            Debug.LogWarning($"{nameof(WallProximityVolume)}: Audio entry is missing or clip is null. id={audioId}", this);
            return;
        }

        targetAudioSource.clip = entry.clip;
        targetAudioSource.outputAudioMixerGroup = entry.mixerGroup;
        targetAudioSource.pitch = Mathf.Clamp(entry.basePitch, -3f, 3f);
        targetAudioSource.spatialBlend = 1f;
        targetAudioSource.minDistance = entry.minDistance;
        targetAudioSource.maxDistance = entry.maxDistance;
        targetAudioSource.loop = true;
        targetAudioSource.playOnAwake = true;

        if (!targetAudioSource.isPlaying)
        {
            targetAudioSource.Play();
        }
    }

    private float FindNearestWallDistanceByRadialRays()
    {
        if (rayCount < 1)
        {
            return detectionRadius;
        }

        float nearestDistance = detectionRadius;
        Vector3 origin = transform.position + Vector3.up * rayOriginHeightOffset;
        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));

            if (Physics.Raycast(origin, direction, out RaycastHit hit, detectionRadius, wallLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.distance < nearestDistance)
                {
                    nearestDistance = hit.distance;
                }
            }
        }

        return nearestDistance;
    }

    private void ApplyVolume(float proximity)
    {
        targetAudioSource.volume = Mathf.Lerp(farVolume, nearVolume, proximity);
    }
}