using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [TextArea(1,3)]
    public string[] introTexts;
    public Image logo;
    int currentText;
    int logoLayer; //remove the logo from the interactable layer until the fade in has finished

    [Tooltip("The time between fade in ending and fade out playing.")]
    public float focusTime;
    [Tooltip("The time between fade out ending and fade in playing.")]
    public float betweenTime;
    Animator animator;
    public TextMeshProUGUI textbox;

    private void OnEnable()
    {
        OVRManager.HMDMounted += OnMounted;
        FocusManager.Instance.OnStateChanged += OnReturn;
    }
    private void OnDisable()
    {
        OVRManager.HMDMounted -= OnMounted;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        logoLayer = logo.gameObject.layer;

        SetUpIntro();
    }
    void OnMounted()
    {
        if (FocusManager.Instance.currentState == VideoState.Entry)
        {
            SetUpIntro();
        }
    }

    void OnReturn(VideoState state)
    {
        if (state == VideoState.Entry)
        {
            logo.gameObject.layer = 0;
            logo.gameObject.SetActive(false);
        }
    }

    void SetUpIntro()
    {
        logo.gameObject.layer = 0;
        logo.gameObject.SetActive(false);

        currentText = 0;        
        textbox.text = introTexts[currentText];        
        textbox.gameObject.SetActive(true);

        animator.Rebind();
        animator.Play("fadeIn");
    }
   

    public IEnumerator OnIntroFadeInFinished()
    {
        yield return new WaitForSeconds(focusTime);
        animator.Play("fadeOut");
    }

    public IEnumerator OnIntroFadeOutFinished()
    {
        yield return new WaitForSeconds(betweenTime);
        UpdateDisplay();

        Debug.LogError($"Intro Fade-out has inished: {currentText}");
    }

    public void OnLogoFadeInFinished() 
    {
        logo.gameObject.layer = logoLayer;
    }

    void UpdateDisplay()
    {
        currentText++;

        if (currentText < introTexts.Length)
        {
            textbox.text = introTexts[currentText];
            animator.Play("fadeIn");
        }
        else
        {
            textbox.gameObject.SetActive(false);
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, 0);
            logo.gameObject.SetActive(true);
            animator.Play("logoIn");
                   
        }
    }
}
