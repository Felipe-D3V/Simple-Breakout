using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocoForte : MonoBehaviour
{
    [Header("Vida e valor")]
    public int vida = 3;
    public int valorDoBloco = 300;

    [Header("Efeitos e Áudio")]
    public GameObject efeitoQuebra;
    public AudioSource SomQuebra;
    public AudioSource SomHit;

    [Header("Sprites por dano")]
    public Sprite spriteNormal;
    public Sprite spriteRachado;
    public Sprite spriteQuebrado;

    private int hits = 0; // Contador de hits
    private int maxHits; // Número máximo de hits que o bloco pode receber

    [Header("Feedback visual")]
    [SerializeField] private Color corDano = Color.white;

    // componente cacheado
    private SpriteRenderer sr;
    private GameManager gm;

    void Start()
    {

        maxHits = vida;
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) Debug.LogWarning("SpriteRenderer não encontrado no " + name);

        // define sprite inicial
        if (spriteNormal != null) sr.sprite = spriteNormal;

        gm = FindObjectOfType<GameManager>();
    }

    public void TomarDano()
    {
        vida--;

        // Som do dano (play one-shot)
        hits++; // Incrementa o contador de hits

        // Som do dano (play one-shot)
        if (SomHit != null && hits <= 2)
            AudioSource.PlayClipAtPoint(SomHit.clip, transform.position);
        else if (SomQuebra != null && hits == maxHits)
            AudioSource.PlayClipAtPoint(SomQuebra.clip, transform.position);

        // Shake da câmera no hit (se existir)
        if (CameraShake.instancia != null)
            CameraShake.instancia.Tremer(0.15f, 0.2f);

        if (vida <= 0)
        {
            
            // Partículas ao quebrar
            if (efeitoQuebra != null)
                Instantiate(efeitoQuebra, transform.position, Quaternion.identity);

            // Pontuação
            if (ScoreManager.instancia != null)
                ScoreManager.instancia.AdicionarPontos(valorDoBloco);

            // Notifica GameManager que um bloco quebrou (importantíssimo)
            if (gm != null)
            Destroy(gameObject);
        }
        else
        {
            // Troca de sprite conforme vida
            AtualizarSprite();

            // Efeito visual de dano (cor piscando)
            StopAllCoroutines();
            StartCoroutine(EfeitoDano());
        }
    }

    void AtualizarSprite()
    {
        if (vida == 2)
        {
            if (spriteRachado != null) sr.sprite = spriteRachado;
        }
        else if (vida == 1)
        {
            if (spriteQuebrado != null) sr.sprite = spriteQuebrado;
        }
    }

    IEnumerator EfeitoDano()
    {
        if (sr == null) yield break;

        Color corOriginal = sr.color;

        // pico rápido de dano
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 10f;
            sr.color = Color.Lerp(corOriginal, corDano, t);
            yield return null;
        }

        // volta suave
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 4f;
            sr.color = Color.Lerp(corDano, corOriginal, t);
            yield return null;
        }

        // garantir cor original exata ao final
        sr.color = corOriginal;
    }
}
