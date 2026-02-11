using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private UnityEvent<string, Vector3> onSFXAudioPlay = new();
    private UnityEvent<string> onUIAudioPlay = new();

    [Header("Audio Assets")]
    [SerializeField] private AudioRequestChannel channel;
    [SerializeField] private AudioLibrary library;

    [Header("Object Pool")]
    [SerializeField] private ObjectPool objectPool;

    [Header("Audio Source")]
    [SerializeField] private AudioSource bgmSource;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    private const string MASTER_PARAM = "MasterVolume";
    private const string BGM_PARAM = "BGMVolume";
    private const string SFX_PARAM = "SFXVolume";
    private const string UI_PARAM = "UIVolume";

    [Header("Volume Settings")]
    //[Range(0f, 1f)][SerializeField] private float masterVolume = 1.0f;
    //[Range(0f, 1f)][SerializeField] private float bgmVolume = 0.5f;
    [Range(0f, 1f)][SerializeField] private float sfxVolume = 1.0f;
    //[Range(0f, 1f)][SerializeField] private float uiVolume = 1.0f;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (channel != null)
            channel.OnRequest += HandleAudioRequest;

        ValidateRegisteredAudioIds();
    }

    private void OnDisable()
    {
        if (channel != null)
            channel.OnRequest -= HandleAudioRequest;
    }

    private void HandleAudioRequest(AudioRequest request)
    {
        if (library == null)
        {
            Debug.LogError("AudioManager: AudioLibrary가 연결되지 않았습니다.");
            return;
        }

        if (!library.TryGet(request.id, out AudioEntry entry) || entry.clip == null)
        {
            Debug.LogWarning($"AudioManager: Audio id를 찾지 못했거나 clip이 null입니다: {request.id}");
            return;
        }

        switch (entry.type)
        {
            case AudioType.BGM:
                PlayBGM(entry, request, entry.loop);
                break;
            case AudioType.SFX:
                PlaySFX(entry, request);
                break;
            case AudioType.UI:
                PlayUISFX(request.id);
                break;
            default:
                Debug.LogWarning($"AudioManager: 알 수 없는 AudioType입니다: {entry.type}");
                break;
        }

    }

    private void PlayBGM(AudioEntry entry, AudioRequest request, bool loop = true)
    {

    }

    private void PlaySFX(AudioEntry entry, AudioRequest request)
    {
        //Debug.Log(objectPool);
        if (objectPool == null)
        {
            Debug.LogError("AudioManager: ObjectPool이 연결되지 않았습니다.");
            return;
        }

        AudioPlayer audioPlayer = objectPool.SpawnAudioPlayer();

        if (request.follow && request.followTarget)
        {
            request.position = request.followTarget.position;
            audioPlayer.SetFollowTarget(request.followTarget);
        }
        else
        {
            audioPlayer.transform.position = request.position;
            audioPlayer.SetFollowTarget(null);
        }

        audioPlayer.Play(entry, sfxVolume, 1f, request.playFullClip, request.startTime, request.endTime);
    }

    private void PlayUISFX(string clipName)
    {

    }

    private void LoadVolumes()
    {

    }

    private void ValidateRegisteredAudioIds()
    {
        if (library == null)
        {
            return;
        }

        foreach (var id in AudioIds.All)
        {
            if (!library.TryGet(id, out var entry) || entry.clip == null)
            {
                Debug.LogWarning($"AudioManager: Missing AudioLibrary entry or clip for id: {id}");
            }
        }
    }
}
