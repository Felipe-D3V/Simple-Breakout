using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instancia;

    Vector3 posInicial;

    void Awake()
    {
        instancia = this;
        posInicial = transform.localPosition;
        sr = GetComponent<SpriteRenderer>();
    }

    public void Tremer(float intensidade, float duracao)
    {
        StartCoroutine(TremerRoutine(intensidade, duracao));
    }

    IEnumerator TremerRoutine(float intensidade, float duracao)
    {
        float tempo = 0;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            transform.localPosition = posInicial +
                (Vector3)Random.insideUnitCircle * intensidade;

            yield return null;
        }

        transform.localPosition = posInicial;
    }
    [SerializeField] Color corDano = Color.white;
    SpriteRenderer sr;


    IEnumerator EfeitoDano()
    {
        Color corOriginal = sr.color;

        //  pico de dano instantâneo
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 10f;
            sr.color = Color.Lerp(corOriginal, corDano, t);
            yield return null;
        }

        //  volta suave para a cor original do bloco
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 4f;
            sr.color = Color.Lerp(corDano, corOriginal, t);
            yield return null;
        }
    }

}

