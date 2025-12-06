using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocoPowerUp : MonoBehaviour
{
    public int vida = 1;

    [Header("Prefab do PowerUp")]
    public GameObject prefabPowerUp;
    [Range(0f, 1f)]
    public float dropChance = 1f; // probabilidade de dropar (0 = nunca, 1 = sempre)
    public AudioSource SomHit;

    [Header("Sprites dos PowerUps (configure todos no Inspector)")]
    public Sprite spriteAumentarBarra;
    public Sprite spriteAumentarVelocidade;
    public Sprite spriteDiminuirBarra;
    public Sprite spriteDiminuirVelocidade;

    // novos sprites
    public Sprite spriteAcelerarBola;
    public Sprite spriteDesacelerarBola;
    public Sprite spriteAumentarTamanhoBola;
    public Sprite spriteDiminuirTamanhoBola;
    public Sprite spriteBolaTransparente;
    public Sprite spriteCurar;

    private SpriteRenderer sr;
    private GameManager gm;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        gm = FindObjectOfType<GameManager>();
    }

    // método público para ser chamado pela bola
    public void TomarDano()
    {
        vida--;

        if (vida <= 0)
        {
            // Só solta powerup com base na chance configurada
            if (prefabPowerUp != null && Random.value <= dropChance)
            {
                SoltarPowerUp();
            }

            if (gm != null)
                gm.BlocoQuebrado();

            if (SomHit != null)
                AudioSource.PlayClipAtPoint(SomHit.clip, transform.position);

            Destroy(gameObject);
        }
    }

    public void SoltarPowerUp()
    {
        if (prefabPowerUp == null)
            return;

        GameObject obj = Instantiate(prefabPowerUp, transform.position, Quaternion.identity);

        PowerUp script = obj.GetComponent<PowerUp>();
        SpriteRenderer powerSR = obj.GetComponent<SpriteRenderer>();

        if (script == null)
        {
            Debug.LogWarning("Prefab de PowerUp não contém o componente PowerUp.");
            Destroy(obj);
            return;
        }

        // escolhe entre os 10 tipos (0..9)
        int escolha = Random.Range(0, 10);

        // atribui tipo e sprite correspondente (se o sprite estiver configurado)
        switch (escolha)
        {
            case 0:
                script.tipo = PowerUp.TipoPowerUp.AumentarBarra;
                if (powerSR != null) powerSR.sprite = spriteAumentarBarra;
                break;
            case 1:
                script.tipo = PowerUp.TipoPowerUp.AumentarVelocidade;
                if (powerSR != null) powerSR.sprite = spriteAumentarVelocidade;
                break;
            case 2:
                script.tipo = PowerUp.TipoPowerUp.DiminuirBarra;
                if (powerSR != null) powerSR.sprite = spriteDiminuirBarra;
                break;
            case 3:
                script.tipo = PowerUp.TipoPowerUp.DiminuirVelocidade;
                if (powerSR != null) powerSR.sprite = spriteDiminuirVelocidade;
                break;
            case 4:
                script.tipo = PowerUp.TipoPowerUp.AcelerarBola;
                if (powerSR != null) powerSR.sprite = spriteAcelerarBola;
                break;
            case 5:
                script.tipo = PowerUp.TipoPowerUp.DesacelerarBola;
                if (powerSR != null) powerSR.sprite = spriteDesacelerarBola;
                break;
            case 6:
                script.tipo = PowerUp.TipoPowerUp.AumentarTamanhoBola;
                if (powerSR != null) powerSR.sprite = spriteAumentarTamanhoBola;
                break;
            case 7:
                script.tipo = PowerUp.TipoPowerUp.DiminuirTamanhoBola;
                if (powerSR != null) powerSR.sprite = spriteDiminuirTamanhoBola;
                break;
            case 8:
                script.tipo = PowerUp.TipoPowerUp.BolaTransparente;
                if (powerSR != null) powerSR.sprite = spriteBolaTransparente;
                break;
            case 9:
                script.tipo = PowerUp.TipoPowerUp.Curar;
                if (powerSR != null) powerSR.sprite = spriteCurar;
                break;
        }
    }
}
