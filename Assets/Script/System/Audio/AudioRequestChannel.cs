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

    public bool playFullClip;
    public float startTime;
    public float endTime;
}

[CreateAssetMenu(menuName = "Audio/Audio Request Channel")]
public class AudioRequestChannel : ScriptableObject
{
    public event Action<AudioRequest> OnRequest;

    public void Raise(AudioRequest req) => OnRequest?.Invoke(req);

    public void Raise(string id, Vector3 pos)
        => Raise(id, pos, true);

    public void Raise(string id, Vector3 pos, bool playFullClip, float startTime = 0f, float endTime = -1f)
        => OnRequest?.Invoke(new AudioRequest
        {
            id = id,
            position = pos,
            hasPosition = true,
            playFullClip = playFullClip,
            startTime = startTime,
            endTime = endTime
        });

    public void RaiseFollow(string id, Transform target)
        => RaiseFollow(id, target, true);

    public void RaiseFollow(string id, Transform target, bool playFullClip, float startTime = 0f, float endTime = -1f)
        => OnRequest?.Invoke(new AudioRequest
        {
            id = id,
            follow = true,
            followTarget = target,
            hasPosition = true,
            playFullClip = playFullClip,
            startTime = startTime,
            endTime = endTime
        });
}