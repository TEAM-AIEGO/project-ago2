using UnityEngine;

public class SFXEmitter : MonoBehaviour
{
    [SerializeField] private AudioRequestChannel channel;
    [SerializeField] private Vector3 offset;

    public void Play(string id)
    {
        Play(id, true);
    }

    public void Play(string id, bool playFullClip, float startTime = 0f, float endTime = -1f)
    {
        if (channel == null)
        {
            Debug.LogError($"{name}: AudioRequestChannel이 연결되지 않았습니다.");
            return;
        }

        channel.Raise(id, transform.TransformPoint(offset), playFullClip, startTime, endTime);
    }

    public void PlayFollow(string id, Transform target)
    {
        PlayFollow(id, target, true);
    }

    public void PlayFollow(string id, Transform target, bool playFullClip, float startTime = 0f, float endTime = -1f)
    {
        if (channel == null)
        {
            Debug.LogError($"{name}: AudioRequestChannel이 연결되지 않았습니다.");
            return;
        }

        channel.RaiseFollow(id, target, playFullClip, startTime, endTime);
    }
}
