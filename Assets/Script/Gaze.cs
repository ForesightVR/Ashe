using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OuterRimStudios.Utilities;

public class Gaze : MonoBehaviour
{
    public float gazeTime;
    public float gazeRadius;
    public LayerMask interactionLayer;
    public VideoController videoController;

    float time;
    float storedTime;
    bool begun;

    private void Awake()
    {
        FocusManager.Instance.OnStateChanged += TranslateState;
    }

    public void Start()
    {
        ResetTime();
    }

    public void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, gazeRadius, out hit, 1000f, interactionLayer))
        {
            if (Pointer.Instance && !Pointer.Instance.stopFill)
            {
                if (!begun)
                {
                    ResetTime();
                    begun = true;
                }

                MathUtilities.Timer(ref time);
                float pointerTime = MathUtilities.MapValue(0, gazeTime, 1, 0, time);
                Pointer.Instance.Fill(pointerTime);

                if (time <= 0)
                {
                    begun = false;
                    FocusManager.Instance.ChangeState(VideoState.Play);
                }
            }
            else
            {
                MathUtilities.Timer(ref time);

                if (time <= 0)
                    FocusManager.Instance.ChangeState(VideoState.Play);
            }
        }
        else
        {
            if (Pointer.Instance)
            {
                begun = false;
                Pointer.Instance.StopFill();
            }

            ResetTime();
        }
    }

    private void OnDestroy()
    {
        FocusManager.Instance.OnStateChanged -= TranslateState;
    }

    void TranslateState(VideoState state)
    {
        switch (state)
        {
            case VideoState.Entry:
                enabled = true;
                break;
            case VideoState.Play:
                enabled = false;
                break;
        }
    }

    void ResetTime()
    {
        time = gazeTime;
    }
}
