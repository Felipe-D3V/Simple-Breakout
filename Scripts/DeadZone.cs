using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    [Header("UI")]
    public GameObject TelaDerrota;
    public GameObject RankingCanvas;
    public GameObject ContagemCanvas;
    public Text contagemTxt;

    public AudioSource BtnSound;
    public AudioSource AudioMorte;

    [Header("Referências")]
    public Rigidbody2D RigidBola;
    public GameManager scriptB;

    [Header("Sistema de Vidas")]
    public int VidaPlayer = 3;
    public List<Image> coracoes;
    public Transform playerHeartsParent;

    private bool gameOverAtivado = false;

    private void Start()
    {
        string cena = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // No boss ou monstro  não carregamos corações
        if (cena == "Boss" || cena == "Monstro")
            return;

        
        RecarregarCoracoesDaCena();
    }
    void Update()
    {
        if (!gameOverAtivado && VidaPlayer > 0)
        {
            FindAnyObjectByType<GameManager>().MenuEsc();
        }

        // Atalhos para reiniciar ou voltar ao menu
        if (gameOverAtivado)
        {
            if (Input.GetKeyDown(KeyCode.R))
                ReiniciarJogo();
            if (Input.GetKeyDown(KeyCode.Escape))
                TelaInicial();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameOverAtivado) return;

        if (other.CompareTag("Ball"))
        {
            FindAnyObjectByType<BallMoviment>().ResetarBola();
            PerderVida();
            return;
        }

        if (other.CompareTag("Ataque"))
        {
            PerderVida();
            Destroy(other.gameObject);
            return;
        }
    }

    public void Curar1Vida()
    {
        if (gameOverAtivado) return;

        if (VidaPlayer < coracoes.Count)
        {
            VidaPlayer++;
            AtualizarCoracoes();
        }
    }

    public void PerderVida()
    {
        if (gameOverAtivado) return;

        VidaPlayer--;

        if (VidaPlayer < 0)
            VidaPlayer = 0;

        AtualizarCoracoes();

        if (VidaPlayer == 0)
        {
            if (AudioMorte != null)
                AudioSource.PlayClipAtPoint(AudioMorte.clip, transform.position);

            GameOver();
        }
    }
    void RecarregarCoracoesDaCena()
    {
        if (playerHeartsParent == null)
            return;

        coracoes.Clear();

        foreach (Transform t in playerHeartsParent)
        {
            Image img = t.GetComponent<Image>();
            if (img != null)
                coracoes.Add(img);
        }
    }


    void AtualizarCoracoes()
    {
        for (int i = 0; i < coracoes.Count; i++)
        {
            // Se o coração foi destruído, pule
            if (coracoes[i] == null)
                continue;

            if (i < VidaPlayer)
            {
                coracoes[i].enabled = true;
                coracoes[i].transform.localScale = Vector3.one;
            }
            else
            {
                if (coracoes[i].enabled)
                {
                    HeartPop hp = coracoes[i].GetComponent<HeartPop>();

                    if (hp != null && coracoes[i] != null)
                        StartCoroutine(hp.DoPopAndHide());
                    else
                        coracoes[i].enabled = false;
                }
            }
        }
    }


    void GameOver()
    {
       

        gameOverAtivado = true;

        // Bloqueia movimento da bola e jogador
        var bola = FindAnyObjectByType<BallMoviment>();
        if (bola != null)
        {
            bola.BolaSolta = false;
            bola.RigidBola.velocity = Vector2.zero;
        }

        var player = FindAnyObjectByType<PlayerMoviment>();
        if (player != null)
            player.enabled = false;

        // Salva score
        int scoreFinal = ScoreManager.pontuacaoAtual;
        LeaderboardManager.instancia.AdicionarPontuacao(scoreFinal);

        // Atualiza ranking
        var ui = FindObjectOfType<LeaderboardUI>();
        if (ui != null)
            ui.AtualizarLeaderboard();

        // Exibe telas
        if (RankingCanvas != null)
            RankingCanvas.SetActive(true);

        if (TelaDerrota != null)
            TelaDerrota.SetActive(true);

       
    }

    public void ReiniciarJogo()
    {
        if (BtnSound != null)
            AudioSource.PlayClipAtPoint(BtnSound.clip, transform.position);

        ScoreManager.pontuacaoAtual = 0;
        ScoreManager.instancia.ResetarPontuacao();

        SceneManager.LoadScene("Earth");
    }

    public void TelaInicial()
    {
        if (BtnSound != null)
            AudioSource.PlayClipAtPoint(BtnSound.clip, transform.position);

        ScoreManager.pontuacaoAtual = 0;
        ScoreManager.instancia.ResetarPontuacao();

        SceneManager.LoadScene("MenuInicial");
    }
}
