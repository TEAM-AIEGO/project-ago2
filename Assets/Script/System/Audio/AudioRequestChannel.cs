using System;
using UnityEngine;

[Serializable]
public struct AudioRequest
{
    public string id;          // AudioLibrary의 id

    public Vector3 position;
    public bool hasPosition;   // UI/BGM은 false로 보내도 됨

    public Transform followTarget;
    public bool follow;
}

[CreateAssetMenu(menuName = "Audio/Audio Request Channel")]
public class AudioRequestChannel : ScriptableObject
{
    public event Action<AudioRequest> OnRequest;

    public void Raise(AudioRequest req) => OnRequest?.Invoke(req);

    public void Raise(string id, Vector3 pos)
        => OnRequest?.Invoke(new AudioRequest { id = id, position = pos, hasPosition = true});

    public void RaiseFollow(string id, Transform target)
        => OnRequest?.Invoke(new AudioRequest { id = id, follow = true, followTarget = target, hasPosition = true});
}