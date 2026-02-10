using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public event Action<AudioPlayer> Finished;

    private AudioPlayer originPlayerPrefab;
    public AudioPlayer OriginPlayerPrefab => originPlayerPrefab;
    private Transform followTarget;
    private bool isStarted;
    private AudioSource src;

    private bool useSegmentPlayback;
    private float segmentEndTime;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
        src.playOnAwake = false;
        src.spatialBlend = 1f; // 3D
    }

    public void Initialize(AudioPlayer originPrefab)
    {
        originPlayerPrefab = originPrefab;
    }

    private void Update()
    {
        if (followTarget != null)
            transform.position = followTarget.position;

        if (!isStarted) 
            return;

        if (useSegmentPlayback && src.isPlaying && src.time >= segmentEndTime)
        {
            src.Stop();
            useSegmentPlayback = false;
        }

        if (!src.isPlaying)
        {
            isStarted = false;
            useSegmentPlayback = false;
            Finished?.Invoke(originPlayerPrefab);
        }
    }

    public void SetFollowTarget(Transform t)
    {
        followTarget = t;
    }

    public void Play(AudioEntry entry, float volumeMul, float pitchMul, bool playFullClip = true, float startTime = 0f, float endTime = -1f)
    {
        src.clip = entry.clip;
        src.outputAudioMixerGroup = entry.mixerGroup;

        src.volume = entry.baseVolume * volumeMul;

        float pitch = Mathf.Clamp(entry.basePitch * pitchMul, -3f, 3f);
        if (Mathf.Abs(pitch) < 0.01f) pitch = 0.01f;
        src.pitch = pitch;

        src.minDistance = entry.minDistance;
        src.maxDistance = entry.maxDistance;

        useSegmentPlayback = false;

        if (!playFullClip && src.clip != null)
        {
            float clipLength = src.clip.length;
            float normalizedStart = Mathf.Clamp(startTime, 0f, clipLength);
            float normalizedEnd = endTime < 0f ? clipLength : Mathf.Clamp(endTime, normalizedStart, clipLength);

            src.time = normalizedStart;
            segmentEndTime = normalizedEnd;
            useSegmentPlayback = true;
        }
        else
        {
            src.time = 0f;
        }

        src.Play();

        if (!src.isPlaying)
        {
            Debug.LogWarning(
                $"AudioPlayer: Play failed. " +
                $"clip={(src.clip ? src.clip.name : "null")}, vol={src.volume}, pitch={src.pitch}, " +
                $"active={gameObject.activeInHierarchy}, enabled={src.enabled}, " +
                $"mute={src.mute}"
            );
        }

        isStarted = true;
    }
}