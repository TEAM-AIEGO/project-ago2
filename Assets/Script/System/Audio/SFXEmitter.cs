using UnityEngine;

public class SFXEmitter : MonoBehaviour
{
    [SerializeField] private AudioRequestChannel channel;
    [SerializeField] private Vector3 offset;

    public void Play(string id)
    {
        if (channel == null)
        {
            Debug.LogError($"{name}: AudioRequestChannel이 연결되지 않았습니다.");
            return;
        }

        channel.Raise(id, transform.TransformPoint(offset));
    }

    public void PlayFollow(string id, Transform target)
    {
        if (channel == null)
        {
            Debug.LogError($"{name}: AudioRequestChannel이 연결되지 않았습니다.");
            return;
        }

        channel.RaiseFollow(id, target);
    }
}
