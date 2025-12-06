using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum TipoPowerUp
    {
        AumentarBarra,
        AumentarVelocidade,
        DiminuirBarra,
        DiminuirVelocidade,
        AcelerarBola,
        DesacelerarBola,
        AumentarTamanhoBola,
        DiminuirTamanhoBola,
        BolaTransparente,
        Curar
    }

    public TipoPowerUp tipo;
    public float velocidadeQueda = 2f;

    void Update()
    {
        transform.Translate(Vector2.down * velocidadeQueda * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
            return;

        PlayerMoviment player = col.GetComponent<PlayerMoviment>();

        // tenta achar a bola de forma robusta
        BallMoviment bola = FindAnyObjectByType<BallMoviment>();
        if (bola == null)
        {
            GameObject b = GameObject.FindWithTag("Ball");
            if (b != null) bola = b.GetComponent<BallMoviment>();
        }

        DeadZone dead = FindAnyObjectByType<DeadZone>();

        ApplyEffect(player, bola, dead);
        Destroy(gameObject);
    }

    private void ApplyEffect(PlayerMoviment player, BallMoviment bola, DeadZone dead)
    {
        switch (tipo)
        {
            case TipoPowerUp.AumentarBarra:
                player?.AumentarTamanho();
                break;

            case TipoPowerUp.AumentarVelocidade:
                player?.AumentarVelocidade();
                if (bola != null)
                    bola.AtivarPowerUpTemporario(() => { bola.MudarCorBola(Color.green); }, () => { bola.RestaurarCorOriginal(); });
                break;

            case TipoPowerUp.DiminuirBarra:
                player?.DiminuirTamanho();
                break;

            case TipoPowerUp.DiminuirVelocidade:
                player?.DiminuirVelocidade();
                if (bola != null)
                    bola.AtivarPowerUpTemporario(() => { bola.MudarCorBola(Color.red); }, () => { bola.RestaurarCorOriginal(); });
                break;

            case TipoPowerUp.AcelerarBola:
                if (bola != null)
                {
                    float original = bola.velocidadeOriginal;
                    bola.AtivarPowerUpTemporario(
                        () =>
                        {
                            bola.AcelerarBola(3f);
                            bola.MudarCorBola(Color.green);
                        },
                        () =>
                        {
                            bola.velocidadeBola = original;
                            bola.RestaurarCorOriginal();
                            if (bola.BolaSolta)
                                bola.RigidBola.velocity = bola.RigidBola.velocity.normalized * bola.velocidadeBola;
                        }
                    );
                }
                break;

            case TipoPowerUp.DesacelerarBola:
                if (bola != null)
                {
                    float original = bola.velocidadeOriginal;
                    bola.AtivarPowerUpTemporario(
                        () =>
                        {
                            bola.DesacelerarBola(2f);
                            bola.MudarCorBola(Color.red);
                        },
                        () =>
                        {
                            bola.velocidadeBola = original;
                            bola.RestaurarCorOriginal();
                            if (bola.BolaSolta)
                                bola.RigidBola.velocity = bola.RigidBola.velocity.normalized * bola.velocidadeBola;
                        }
                    );
                }
                break;

            case TipoPowerUp.AumentarTamanhoBola:
                if (bola != null)
                {
                    Vector3 originalScale = bola.tamanhoOriginalBola;
                    bola.AtivarPowerUpTemporario(
                        () => { bola.AlterarTamanhoBola(0.4f); },
                        () => { bola.transform.localScale = originalScale; }
                    );
                }
                break;

            case TipoPowerUp.DiminuirTamanhoBola:
                if (bola != null)
                {
                    Vector3 originalScale = bola.tamanhoOriginalBola;
                    bola.AtivarPowerUpTemporario(
                        () => { bola.AlterarTamanhoBola(-0.25f); },
                        () => { bola.transform.localScale = originalScale; }
                    );
                }
                break;

            case TipoPowerUp.BolaTransparente:
                if (bola != null)
                    bola.AtivarPowerUpTemporario(() => { bola.StartCoroutine(bola.BolaTransparente(7f)); }, null);
                break;

            case TipoPowerUp.Curar:
                if (dead != null && dead.VidaPlayer < 3)
                    dead.Curar1Vida();
                break;
        }
    }

}
