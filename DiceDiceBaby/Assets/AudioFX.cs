using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFX : MonoBehaviour
{
    AudioSource source;
    float startTime;
    const float minimum_play_time = 0.2f;

    void Start()
    {
        source = GetComponent<AudioSource>();
        startTime = Time.time;
    }

    void Update()
    {
        // if source is done , destroy
        //Debug.Log(source.isPlaying);
        if (!source.isPlaying && Time.time > startTime + minimum_play_time)
        {
            Destroy(gameObject);
        }
    }
}