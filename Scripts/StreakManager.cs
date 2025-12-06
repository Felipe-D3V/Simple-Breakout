using UnityEngine;

public class StreakManager : MonoBehaviour
{
    public static StreakManager instancia;

    public int streakAtual = 0;
    public float multiplicador => 1f + (streakAtual * 0.05f);

    private void Awake()
    {
        instancia = this;
    }

    public void AdicionarStreak()
    {
        streakAtual++;
    }

    public void ResetarStreak()
    {
        streakAtual = 0;
    }
}
