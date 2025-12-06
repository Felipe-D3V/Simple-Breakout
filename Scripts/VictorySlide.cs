using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictorySlide : MonoBehaviour
{
    RectTransform rt;
    Vector2 finalPos;
    Vector2 startPos;
    float duracao = 0.8f;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        finalPos = rt.anchoredPosition;
        startPos = finalPos + new Vector2(-2000, 0); // fora da tela
    }

    public void Animar()
    {
        StopAllCoroutines();
        StartCoroutine(SlideIn());
    }

    IEnumerator SlideIn()
    {
        float t = 0;
        rt.anchoredPosition = startPos;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime / duracao;
            rt.anchoredPosition = Vector2.Lerp(startPos, finalPos, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
    }
}
