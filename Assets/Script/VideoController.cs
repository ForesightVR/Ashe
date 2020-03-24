using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
public class VideoController : MonoBehaviour
{
    public MediaPlayer mediaPlayer;
    public GameObject videoSphere;
    public GameObject world;

    private void OnEnable()
    {
        FocusManager.Instance.OnStateChanged += TranslateState;
    }

    private void OnDisable()
    {
        FocusManager.Instance.OnStateChanged -= TranslateState;
    }

    void TranslateState(VideoState state)
    {
        switch(state)
        {
            case VideoState.Entry:
                Reset();
                break;
            case VideoState.Play:
                Play();
                break;
            case VideoState.Pause:
                Pause();
                break;
        }
    }

    private void Update()
    {
        if (mediaPlayer.Control.IsFinished())
            FocusManager.Instance.ChangeState(VideoState.Entry);
    }

    public void Play()
    {
        videoSphere.SetActive(true);
        mediaPlayer.Play();

        world.SetActive(false);
    }

    public void Pause()
    {
        mediaPlayer.Pause();
    }

    public void Reset()
    {
        world.SetActive(true);
        videoSphere.SetActive(false);
        mediaPlayer.Stop();
        mediaPlayer.Control.Rewind();
    }
}
