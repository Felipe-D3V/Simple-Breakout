using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    public Text[] linhas;
    public Text RankingText; // linkar no inspector


    void OnEnable()
    {
        AtualizarLeaderboard();
    }

  

    public void AtualizarLeaderboard()
    {
        int[] scores = LeaderboardManager.instancia.topScores;

        string ranking = "Ranking:\n";
        for (int i = 0; i < scores.Length; i++)
        {
            ranking += $"{i + 1}º - {scores[i]}\n";
        }

        RankingText.text = ranking;
    }
}

