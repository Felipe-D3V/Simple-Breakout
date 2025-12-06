using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Vector2 Size;
    public Vector2 offSet;

    [Header("Prefabs dos Blocos")]
    public GameObject blocoComum;
    public GameObject blocoForte;
    public GameObject blocoPowerUp;

    [Header("Gradientes (escolhe 1 aleatório por mapa)")]
    public Gradient[] gradientes;  // <<<<<< Agora são vários gradientes!
    private Gradient gradienteAtual;

    private void Start()
    {
        GerarMapa();
    }

    public void GerarMapa()
    {
        // escolhe um gradiente aleatório
        if (gradientes.Length > 0)
            gradienteAtual = gradientes[Random.Range(0, gradientes.Length)];

        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                // Sistema de probabilidade
                float chance = Random.value;
                GameObject blocoSelecionado;

                if (chance < 0.6f)
                    blocoSelecionado = blocoComum;
                else if (chance < 0.8f)
                    blocoSelecionado = blocoForte;
                else
                    blocoSelecionado = blocoPowerUp;

                // Instanciar bloco
                GameObject novoTijolo = Instantiate(blocoSelecionado, transform);

                novoTijolo.transform.position =
                    transform.position +
                    new Vector3(
                        (Size.x - 1) * .5f - i * offSet.x,
                        j * offSet.y,
                        0
                    );

                // Aplicar cor usando o gradiente escolhido
                SpriteRenderer sr = novoTijolo.GetComponent<SpriteRenderer>();
                if (sr != null && gradienteAtual != null)
                {
                    sr.color = gradienteAtual.Evaluate((float)j / (Size.y - 1));
                }
            }
        }
    }

    public void CarregarProximoLevel()
    {
        // destruir blocos antigos
        foreach (GameObject bloco in GameObject.FindGameObjectsWithTag("Bloco"))
            Destroy(bloco);

        foreach (GameObject bloco in GameObject.FindGameObjectsWithTag("Tijolo"))
            Destroy(bloco);

        // gerar novo mapa com novo gradiente aleatório
        GerarMapa();

        FindObjectOfType<GameManager>().AtualizarContagemDeBlocos();
        FindObjectOfType<BallMoviment>().ResetarBola();
    }
}
