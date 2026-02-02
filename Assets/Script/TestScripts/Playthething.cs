using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class Playthething : MonoBehaviour
{
    private PlayableDirector pd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0.5f;
        pd = GetComponent<PlayableDirector>();
        pd.Play();
    }
}
