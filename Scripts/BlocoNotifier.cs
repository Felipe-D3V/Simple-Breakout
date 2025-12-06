using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocoNotifier : MonoBehaviour
{
    private GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void OnDestroy()
    {
        // impedir chamada caso a cena esteja descarregando
        if (!gameObject.scene.isLoaded) return;

        // só notifica se for realmente um bloco válido
        if (CompareTag("Tijolo") || CompareTag("Bloco"))
        {
            if (gm != null)
                gm.BlocoQuebrado();
        }
    }
}
