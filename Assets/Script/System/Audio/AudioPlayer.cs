using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public event Action<AudioPlayer> Finished;

    private Transform followTarget;
    private bool isStarted = false;
    private AudioSource src;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
        src.playOnAwake = false;
        src.spatialBlend = 1f; // 3D
    }

    private void Update()
    {
        if (followTarget != null)
            transform.position = followTarget.position;

        if (!isStarted) 
            return;

        if (!src.isPlaying)
        {
            isStarted = false;
            Finished?.Invoke(this);
        }
    }

    public void SetFollowTarget(Transform t)
    {
        followTarget = t;
    }

    public void Play(AudioEntry entry, float volumeMul, float pitchMul)
    {
        src.clip = entry.clip;
        src.outputAudioMixerGroup = entry.mixerGroup;

        src.volume = entry.baseVolume * volumeMul;

        float pitch = Mathf.Clamp(entry.basePitch * pitchMul, -3f, 3f);
        if (Mathf.Abs(pitch) < 0.01f) pitch = 0.01f;
        src.pitch = pitch;

        src.minDistance = entry.minDistance;
        src.maxDistance = entry.maxDistance;

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