using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instancia;

    public static int pontuacaoAtual = 0;
    public Text textoPontuacao;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;

            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }

    void Start()
    {
         // ou procure pelo objeto correto
        AtualizarTexto();
        
    }

    public void ResetarPontuacao()
    {
        pontuacaoAtual = 0;
        AtualizarTexto();
    }

    public void AdicionarPontos(int valor)
    {
        pontuacaoAtual += valor;
        AtualizarTexto();
    }

    void AtualizarTexto()
    {
        if (textoPontuacao != null)
            textoPontuacao.text = "Pontos: " + pontuacaoAtual;
    }
}