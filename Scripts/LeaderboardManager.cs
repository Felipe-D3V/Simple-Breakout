using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instancia;

    public int[] topScores = new int[5];
    public bool novoRecorde = false;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            CarregarRanking();
        }
        else
            Destroy(gameObject);
    }

    public void AdicionarPontuacao(int score)
    {
        novoRecorde = false;

        for (int i = 0; i < topScores.Length; i++)
        {
            if (score > topScores[i])
            {
                // Desloca
                for (int j = topScores.Length - 1; j > i; j--)
                {
                    topScores[j] = topScores[j - 1];
                }

                topScores[i] = score;
                novoRecorde = true;
                break;
            }
        }

        SalvarRanking();
    }

    void SalvarRanking()
    {
        for (int i = 0; i < 5; i++)
            PlayerPrefs.SetInt("Rank" + i, topScores[i]);
    }

    void CarregarRanking()
    {
        for (int i = 0; i < 5; i++)
            topScores[i] = PlayerPrefs.GetInt("Rank" + i, 0);
    }
}
