using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlaybackEditor : MonoBehaviour
{
    [Header("Playback Window")]
    [SerializeField] private float startTime = 0f;
    [SerializeField] private float endTime = -1f;
    [SerializeField] private bool loopInWindow = true;

    [Header("Playback")]
    [SerializeField] private float playbackPitch = 1f;
    [SerializeField] private bool playOnStart = true;

    private AudioSource audioSource;

    public float StartTime => Mathf.Max(0f, startTime);
    public float EndTime => endTime;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        ApplyPitch(playbackPitch);
    }

    private void Start()
    {
        NormalizeWindow();

        if (playOnStart)
        {
            PlayFromWindowStart();
        }
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            return;
        }

        if (!HasValidEndTime())
        {
            return;
        }

        if (audioSource.time >= endTime)
        {
            HandleWindowEnded();
        }
    }

    public void SetPlaybackWindow(float newStartTime, float newEndTime)
    {
        startTime = Mathf.Max(0f, newStartTime);
        endTime = newEndTime;
        NormalizeWindow();

        if (audioSource.isPlaying)
        {
            SeekToStart();
        }
    }

    public void SetPlaybackPitch(float newPlaybackPitch)
    {
        ApplyPitch(newPlaybackPitch);
    }

    public void PlayFromWindowStart()
    {
        NormalizeWindow();
        SeekToStart();
        audioSource.Play();
    }

    public void Pause() => audioSource.Pause();

    public void Stop()
    {
        audioSource.Stop();
        SeekToStart();
    }

    private void HandleWindowEnded()
    {
        if (loopInWindow)
        {
            SeekToStart();
            audioSource.Play();
            return;
        }

        audioSource.Pause();
        audioSource.time = endTime;
    }

    private void SeekToStart()
    {
        if (audioSource.clip == null)
        {
            return;
        }

        audioSource.time = startTime;
    }

    private bool HasValidEndTime()
    {
        return endTime > startTime;
    }

    private void NormalizeWindow()
    {
        if (audioSource.clip == null)
        {
            return;
        }

        var clipLength = audioSource.clip.length;
        startTime = Mathf.Clamp(startTime, 0f, clipLength);

        if (endTime < 0f)
        {
            endTime = clipLength;
            return;
        }

        endTime = Mathf.Clamp(endTime, startTime, clipLength);
    }

    private void ApplyPitch(float newPlaybackPitch)
    {
        playbackPitch = Mathf.Clamp(newPlaybackPitch, 0.1f, 3f);
        audioSource.pitch = playbackPitch;
    }
}
