using UnityEngine;
using System.Collections;

public class FlashText : MonoBehaviour
{
    public float flashTime = 1.0f;
    public float flashIntensity = 0.05f;
    public TMPro.TMP_Text text;

    private bool flash;
    private float time;

    private void Start()
    {
        time = 0.0f;
        flash = true;
    }

    public void Update()
    {
        UpdateFlash();
    }

    public void UpdateFlash()
    {
        time += Time.deltaTime;

        if (flash)
        {
            if (time >= flashTime)
            {
                flash = false;
                time = 0.0f;
                return;
            }
            else
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - time * flashIntensity);
        }

        else
        {
            if (time >= flashTime)
            {
                flash = true;
                time = 0.0f;
                return;
            }
            else
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + time * flashIntensity);
        }

    }

}
