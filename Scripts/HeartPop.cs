using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeartPop : MonoBehaviour
{
 
    public AudioSource PopSound;
    public float popScale = 1.4f;
    public float popTime = 0.15f;

    Image img;
    Vector3 originalScale;

    void Awake()
    {
        img = GetComponent<Image>();
        originalScale = transform.localScale;
    }

    public IEnumerator DoPopAndHide()
    {
        // Aumenta
        float t = 0;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime / popTime;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * popScale, t);
            
            PopSound.Play();
            

            yield return null;
        }

        // Diminui
        t = 0;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime / popTime;
            transform.localScale = Vector3.Lerp(originalScale * popScale, Vector3.zero, t);
            yield return null;
        }

        img.enabled = false;
        transform.localScale = originalScale;
    }
}
