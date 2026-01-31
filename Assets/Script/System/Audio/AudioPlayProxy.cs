using System;
using UnityEngine;

public static class AudioEventBus
{
    public static event Action<string, Vector3> OnSFXPlay;
    public static event Action<string> OnUIPlay;

    public static void RaiseSFX(string key, Vector3 pos)
        => OnSFXPlay?.Invoke(key, pos);

    public static void RaiseUI(string key)
        => OnUIPlay?.Invoke(key);
}

public interface IAudioPlay
{
    void PlaySFX(string key, Vector3 worldPos);
    void PlayUI(string key);
}

public sealed class AudioPlayProxy : MonoBehaviour, IAudioPlay
{
    public void PlaySFX(string key, Vector3 worldPos)
        => AudioEventBus.RaiseSFX(key, worldPos);

    public void PlayUI(string key)
        => AudioEventBus.RaiseUI(key);
}