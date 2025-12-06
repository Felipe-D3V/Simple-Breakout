using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TelaInicial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IniciarJogo()
    {
        SceneManager.LoadScene("Earth");
    }

    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Você Saiu do Jogo");
    }
}
