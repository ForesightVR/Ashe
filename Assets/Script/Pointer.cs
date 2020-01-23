using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Pointer : MonoBehaviour
{
    public static Pointer Instance;

    Image pointerImage;
    float fillAmount;

    [HideInInspector]
    public bool stopFill;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pointerImage = GetComponent<Image>();
        stopFill = false;
        StopFill();
        StartCoroutine(Cooldown());
    }

    private void Update()
    {
        if (!stopFill)
        {
            if (pointerImage.fillAmount != fillAmount)
                pointerImage.fillAmount = Mathf.MoveTowards(pointerImage.fillAmount, fillAmount, 1);

            if (pointerImage.fillAmount == 1)
            {
                stopFill = true;
                StopFill();
                StartCoroutine(Cooldown());
            }
        }        
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(2.5f);
        stopFill = false;
    }

    public void Fill(float _fillAmount)
    {
        if (stopFill) return;
        fillAmount = _fillAmount;
    }

    public void StopFill()
    {
        fillAmount = 0;
    }
}