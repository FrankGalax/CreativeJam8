using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    void Start()
    {
        GetComponent<AudioSource>().Play();
    }
}