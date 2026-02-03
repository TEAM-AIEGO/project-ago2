using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class Playthething : MonoBehaviour
{
    [SerializeField] Camera PlayCam;
    private PlayableDirector pd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlaythethingPlz()
    {
        if (PlayCam)
        {
            Camera.main.enabled = false;
            PlayCam.enabled = true;
        }
        Time.timeScale = 0.5f;
        pd = GetComponent<PlayableDirector>();
        pd.Play();
    }
}
