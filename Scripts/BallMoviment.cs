using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoviment : MonoBehaviour
{
    [Header("Referências")]
    public Rigidbody2D RigidBola;
    public Rigidbody2D RigidPlayer;
    private SpriteRenderer spriteRenderer;
    public TrailRenderer trail; // <-- ADICIONADO

    [Header("Configuração")]
    public float velocidadeBola = 6f;
    public bool BolaSolta;

    [Header("Limites da Bola")]
    public float velocidadeMin = 3f;
    public float velocidadeMax = 12f;

    [Header("Som")]
    public AudioSource HitSound;
    public AudioSource CollisionSound;

    [Header("Variação de Direção")]
    public float direcaoAleatoriaX = 0.1f;
    public float direcaoAleatoriaY = 0.1f;

    [Header("FX")]
    public GameObject efeitoQuebra;

    // valores originais
    [HideInInspector] public float velocidadeOriginal;
    [HideInInspector] public Vector3 tamanhoOriginalBola;
    private Color corOriginal;


    void Start()
    {
        BolaSolta = false;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (trail == null)
            trail = GetComponent<TrailRenderer>(); // <-- ADICIONADO

        velocidadeOriginal = velocidadeBola;
        tamanhoOriginalBola = transform.localScale;
        corOriginal = spriteRenderer.color;
    }

    void Update()
    {
        BolaSeguir();
    }

    // --------------------------- MOVIMENTAÇÃO ---------------------------

    public void BolaSeguir()
    {
        if (!BolaSolta)
        {
            RigidBola.position = RigidPlayer.position + new Vector2(0, 0.5f);

            if (Input.GetKeyDown(KeyCode.W))
            {
                BolaSolta = true;
                MoverBola();
            }
        }
    }

    public void MoverBola()
    {
        RigidBola.velocity = new Vector2(0, velocidadeBola);
    }

    // --------------------------- COLISÕES ---------------------------

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Player" || tag == "Wall")
            StreakManager.instancia?.ResetarStreak();

        RigidBola.velocity += new Vector2(direcaoAleatoriaX, direcaoAleatoriaY);

        if (tag == "Tijolo")
        {
            Destroy(collision.gameObject);

            StreakManager.instancia?.AdicionarStreak();

            int pontos = Mathf.RoundToInt(100 * StreakManager.instancia.multiplicador);
            ScoreManager.instancia?.AdicionarPontos(pontos);

            if (HitSound != null)
                HitSound.Play();

            return;
        }

        BlocoForte forte = collision.collider.GetComponent<BlocoForte>();
        if (forte != null)
        {
            forte.TomarDano();
            return;
        }

        BlocoPowerUp power = collision.collider.GetComponent<BlocoPowerUp>();
        if (power != null)
        {
            power.TomarDano();
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ataque"))
        {
            Destroy(collision.gameObject);

            if (efeitoQuebra != null)
                Instantiate(efeitoQuebra, transform.position, Quaternion.identity);

            ResetarBola();
        }
    }

    // --------------------------- RESET ---------------------------

    public void ResetarBola()
    {
        RigidBola.velocity = Vector2.zero;
        RigidBola.angularVelocity = 0;

        transform.localScale = tamanhoOriginalBola;
        spriteRenderer.color = corOriginal;

        gameObject.layer = 0;

        transform.position = RigidPlayer.position + new Vector2(0, 0.5f);

        StreakManager.instancia?.ResetarStreak();

        BolaSolta = false;

        GameManager gm = FindAnyObjectByType<GameManager>();
        if (gm != null)
            gm.StartCoroutine(ChamarContagemDepois());
    }

    IEnumerator ChamarContagemDepois()
    {
        yield return null;
        FindAnyObjectByType<GameManager>().ContagemInicial();
    }

    // --------------------------- POWERUPS DA BOLA ---------------------------

    public void AcelerarBola(float valor)
    {
        velocidadeBola += valor;
        velocidadeBola = Mathf.Clamp(velocidadeBola, velocidadeMin, velocidadeMax);

        if (BolaSolta)
            RigidBola.velocity = RigidBola.velocity.normalized * velocidadeBola;
    }

    public void DesacelerarBola(float valor)
    {
        velocidadeBola -= valor;
        velocidadeBola = Mathf.Clamp(velocidadeBola, velocidadeMin, velocidadeMax);

        if (BolaSolta)
            RigidBola.velocity = RigidBola.velocity.normalized * velocidadeBola;
    }

    public void AlterarTamanhoBola(float delta)
    {
        transform.localScale += new Vector3(delta, delta, 0);

        float s = transform.localScale.x;
        s = Mathf.Clamp(s, 0.3f, 2f);
        transform.localScale = new Vector3(s, s, 1);
    }

    // --------------------------- INVISÍVEL + TRAIL ---------------------------

    public IEnumerator BolaTransparente(float t)
    {
        // deixar transparente
        Color c = spriteRenderer.color;
        c.a = 0.3f;
        spriteRenderer.color = c;

        // DESATIVAR TRAIL
        if (trail != null)
        {
            trail.enabled = false;
            trail.Clear();
        }

        yield return new WaitForSeconds(t);

        // voltar normal
        c.a = 1f;
        spriteRenderer.color = c;

        // REATIVAR TRAIL
        if (trail != null)
            trail.enabled = true;
    }

    // --------------------------- POWERUP GENÉRICO DE 7 SEGUNDOS ---------------------------

    public void AtivarPowerUpTemporario(System.Action ativar, System.Action desativar)
    {
        StartCoroutine(PowerUpTemporario(ativar, desativar));
    }

    private IEnumerator PowerUpTemporario(System.Action ativar, System.Action desativar)
    {
        ativar?.Invoke();
        yield return new WaitForSeconds(7f);
        desativar?.Invoke();
    }
    public void MudarCorBola(Color cor)
    {
        if (spriteRenderer != null)
            spriteRenderer.color = cor;
    }

    public void RestaurarCorOriginal()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = corOriginal;
    }

}
