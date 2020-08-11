using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
public class VideoController : MonoBehaviour
{
    public MediaPlayer mediaPlayer;
    public GameObject videoSphere;
    public GameObject world;
    public GameObject canvas;

    bool hasStarted;

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
        Debug.LogError("Changing State to : " + state);
        switch(state)
        {
            case VideoState.Entry:
                ResetScene();
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
        if (hasStarted && mediaPlayer.Control.IsFinished())
        {
            hasStarted = false;
            FocusManager.Instance.ChangeState(VideoState.Entry);
            Debug.LogError("Video Finished!");
        }
    }

    public void Play()
    {
        Debug.LogError("Play");
        mediaPlayer.Control.SeekFast(0);
        videoSphere.SetActive(true);
        mediaPlayer.Play();
        world.SetActive(false);
        hasStarted = true;
    }

    public void Pause()
    {
        Debug.LogError("Pause");
        mediaPlayer.Pause();
    }

    public void ResetScene()
    {
        Debug.LogError("Reset");
        canvas.SetActive(true);
        world.SetActive(true);
        videoSphere.SetActive(false);
        mediaPlayer.Stop();
    }
}
