using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource source;

    private void OnEnable()
    {
        if(!source)
            source = GetComponent<AudioSource>();

        if(source)
        {
            source.time = 0;
            source.Play();
        }
    }

    private void OnDisable()
    {
        if(source)
            source.Stop();
    }
}
