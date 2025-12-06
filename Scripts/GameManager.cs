using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static int NivelAtual { get; set; }
    public static bool BossDerrotado { get; set; }

    public static bool MonstroDerrotado { get; set; }



    public Text NivelText;
    public Text ContagemBlocos;
    public Text contagemTxt;
    public GameObject ContagemCanvas;
    public AudioSource BtnSound;
    public AudioSource SucessSound;

    public GameObject GameMenu;
    public GameObject ConfigMenu;


    public GameObject SucessCanvas;   // Canvas de fase concluída
    public LevelGenerator levelManager; // Gerenciador de níveis

    private int blocosRestantes;

    public VictorySlide victorySlide;

    private static GameManager instancia;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene cena, LoadSceneMode modo)
    {
        AtualizarNumeroFase();
    }

    void Start()
    {
        NivelAtual = 1;

        StartCoroutine(ContagemInicial());
        AtualizarContagemDeBlocos();
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            redimensionarTela();
        }
    }

    // ---------------------- MENUS ----------------------

    public void AlterarModoTela(int modo)
    {
        // 0 = Janela
        // 1 = Fullscreen (borderless)
        // 2 = Fullscreen Exclusivo
        switch (modo)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
        }

        // Atualiza tamanho da tela, se necessário
        Screen.fullScreen = (modo != 0);
    }
    public void redimensionarTela()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            // De janela → FullScreen borderless
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            // De borderless → Exclusive Fullscreen
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            // De Exclusive Fullscreen → janela
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }

        Debug.Log("Modo de tela atual: " + Screen.fullScreenMode);
    }
    void AtualizarNumeroFase()
    {
        if (NivelText != null)
            NivelText.text = "Fase: " + GameManager.NivelAtual;
    }
    public void MenuEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            GameMenu.SetActive(true);
        }
    }
    public void Resumir()
    {
        Time.timeScale = 1f;
        GameMenu.SetActive(false);
        if (BtnSound != null)
            AudioSource.PlayClipAtPoint(BtnSound.clip, transform.position);
    }

    public void Configuracoes()
    {
        if (BtnSound != null)
            AudioSource.PlayClipAtPoint(BtnSound.clip, transform.position);
        GameMenu.SetActive(false);
        ConfigMenu.SetActive(true);
    }

    public void VoltarMenu()
    {
        if (BtnSound != null)
            AudioSource.PlayClipAtPoint(BtnSound.clip, transform.position);
        ConfigMenu.SetActive(false);
        GameMenu.SetActive(true);

    }

    // ---------------------- CONTAGEM ----------------------

    public IEnumerator ContagemInicial()
    {
        Time.timeScale = 0f;

        ContagemCanvas.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            contagemTxt.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }

        contagemTxt.text = "";
        Time.timeScale = 1f;
        ContagemCanvas.SetActive(false);
    }

    void Awake()
    {
    }

    // ---------------------- SISTEMA DE BLOCOS ----------------------

    public void AtualizarContagemDeBlocos()
    {
        BlocoNotifier[] blocos = FindObjectsOfType<BlocoNotifier>();

        int count = 0;
        foreach (var b in blocos)
        {
            if (b.CompareTag("Tijolo") || b.CompareTag("Bloco"))
                count++;
        }

        blocosRestantes = count;

        if (ContagemBlocos != null)
            ContagemBlocos.text = "B: " + blocosRestantes;
    }





    public void BlocoQuebrado()
    {
        blocosRestantes--;

        AtualizarContagemDeBlocos();

        if (blocosRestantes <= 0)
        {
            NivelConcluido();
        }
    }

    // ---------------------- VITÓRIA ----------------------

    public void NivelConcluido()
    {
        Time.timeScale = 0f;

        SucessCanvas.SetActive(true);

        victorySlide.Animar();

        if (SucessSound != null)
            AudioSource.PlayClipAtPoint(SucessSound.clip, transform.position);

        StartCoroutine(EsperarProximoLevel());
        
    }
    IEnumerator EsperarProximoLevel()
    {
        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        SucessCanvas.SetActive(false);
        ChamarProximoLevel();
        AtualizarNumeroFase();
    }


    void ChamarProximoLevel()
    {
        string cena = SceneManager.GetActiveScene().name;

        // Garante LevelGenerator Earth
        if (cena == "Earth" && levelManager == null)
            levelManager = FindAnyObjectByType<LevelGenerator>();

        // =====================================================
        // 5. Fases 
        // =====================================================
        NivelAtual++;

        
        if (cena == "Earth")
            levelManager?.CarregarProximoLevel();
        else
            SceneManager.LoadScene("Earth");
    }

}
