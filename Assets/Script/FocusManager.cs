using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusManager : MonoBehaviour
{    
    public static FocusManager Instance;

    public delegate void OnStateChange(VideoState state);
    public event OnStateChange OnStateChanged;

    public float pauseDelay;

    public VideoState currentState = VideoState.Entry;
    Coroutine resetVideo;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        OVRManager.HMDMounted += OnMounted;
        OVRManager.HMDUnmounted += UnMounted;
    }
    private void OnDisable()
    {
        OVRManager.HMDMounted -= OnMounted;
        OVRManager.HMDUnmounted -= UnMounted;
    }

    public void ChangeState(VideoState nextState)
    {
        currentState = nextState;
        OnStateChanged?.Invoke(currentState);
    }

    void OnMounted()
    {
        if (resetVideo != null)
        {
            StopCoroutine(resetVideo);
        }

        if (currentState == VideoState.Pause)
            ChangeState(VideoState.Play);
    }

    void UnMounted()
    {
        if (currentState == VideoState.Play)
        {            
            ChangeState(VideoState.Pause);
            resetVideo = StartCoroutine(ResetVideo());
        }
    }

    IEnumerator ResetVideo()
    {
        yield return new WaitForSeconds(pauseDelay);
        ChangeState(VideoState.Entry);
    }
}

public enum VideoState
{
    Entry,
    Play,
    Pause
}
