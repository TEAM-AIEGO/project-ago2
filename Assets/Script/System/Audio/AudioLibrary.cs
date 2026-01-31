using UnityEngine;
using UnityEngine.Audio;

public enum AudioType
{
    SFX,
    UI,
    BGM
}

[System.Serializable]
public struct AudioEntry
{
    public string id;
    public AudioType type;

    public AudioClip clip;
    public AudioMixerGroup mixerGroup;

    [Range(0f, 1f)] public float baseVolume;
    [Range(-3f, 3f)] public float basePitch;
    
    public float minDistance;
    public float maxDistance;

    // BGM¿ë
    public bool loop;
}

[CreateAssetMenu(menuName = "Audio/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    [SerializeField] private AudioEntry[] entries;

    public bool TryGet(string id, out AudioEntry entry)
    {
        foreach (var e in entries)
        {
            if (e.id == id)
            {
                entry = e;
                return true;
            }
        }
        entry = default;
        return false;
    }
}
