using UnityEngine;
using System.Collections;

public class PlayMusic : MonoBehaviour
{

    public AudioClip Music;
    private AudioSource source;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = Music;
        source.loop = true;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
