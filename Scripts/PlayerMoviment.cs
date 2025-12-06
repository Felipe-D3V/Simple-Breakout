using System.Collections;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;

    public float xMinimo;
    public float xMaximo;

    [Header("PowerUps")]
    public float tamanhoIncremento = 0.5f;
    public float velocidadeIncremento = 2f;

    private Vector3 tamanhoOriginal;
    private float velocidadeOriginal;
    private Color corOriginal;

    public AudioSource Buff;
    public AudioSource Debuff;

    private SpriteRenderer sr;

    void Start()
    {
        tamanhoOriginal = transform.localScale;
        velocidadeOriginal = velocidade;

        sr = GetComponent<SpriteRenderer>();
        corOriginal = sr.color;
    }

    void Update()
    {
        MoverJogador();
    }

    private void MoverJogador()
    {
        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, xMinimo, xMaximo),
            transform.position.y
        );

        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector2.right * velocidade * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector2.left * velocidade * Time.deltaTime);
    }

    // -------------------- POWER UPS COM 5s --------------------

    public void AumentarTamanho()
    {
        StopAllCoroutines();
        StartCoroutine(EfeitoBuffTamanho(+tamanhoIncremento, Color.green));
        if (Buff != null)
            AudioSource.PlayClipAtPoint(Buff.clip, transform.position);

    }

    public void DiminuirTamanho()
    {
        StopAllCoroutines();
        StartCoroutine(EfeitoBuffTamanho(-tamanhoIncremento, Color.red));
        if (Debuff != null)
            AudioSource.PlayClipAtPoint(Debuff.clip, transform.position);
    }

    public void AumentarVelocidade()
    {
        StopAllCoroutines();
        StartCoroutine(EfeitoBuffVelocidade(+velocidadeIncremento, Color.green));
        if (Buff != null)
            AudioSource.PlayClipAtPoint(Buff.clip, transform.position);
    }

    public void DiminuirVelocidade()
    {
        StopAllCoroutines();
        StartCoroutine(EfeitoBuffVelocidade(-velocidadeIncremento, Color.red));
        if (Debuff != null)
            AudioSource.PlayClipAtPoint(Debuff.clip, transform.position);
    }


    // -------------------- CORROTINAS DOS EFEITOS --------------------

    IEnumerator EfeitoBuffTamanho(float valor, Color corBuff)
    {
 
        // muda tamanho
        transform.localScale += new Vector3(valor, 0, 0);

        // troca cor
        yield return StartCoroutine(TrocarCor(corBuff));

        // espera 5s
        yield return new WaitForSeconds(5f);

        // volta ao normal
        transform.localScale = tamanhoOriginal;
        yield return StartCoroutine(TrocarCor(corOriginal));
    }

    IEnumerator EfeitoBuffVelocidade(float valor, Color corBuff)
    {

        velocidade += valor;

        // troca cor
        yield return StartCoroutine(TrocarCor(corBuff));

        // espera
        yield return new WaitForSeconds(5f);

        // volta ao original
        velocidade = velocidadeOriginal;

        yield return StartCoroutine(TrocarCor(corOriginal));
    }

    // -------------------- Color.Lerp suave --------------------

    IEnumerator TrocarCor(Color destino)
    {
        Color inicial = sr.color;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * 4f;  // velocidade da transição
            sr.color = Color.Lerp(inicial, destino, t);
            yield return null;
        }
    }
    public void JogadorCurou()
    {
        StartCoroutine(TrocarCor(Color.cyan));
    }

}
