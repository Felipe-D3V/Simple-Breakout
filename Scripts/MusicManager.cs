using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource MusicaDeFundo;
    public Slider VolumeMusica;

    private static MusicManager instance;
    public static MusicManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Carrega volume salvo
            if (PlayerPrefs.HasKey("volume"))
                MusicaDeFundo.volume = PlayerPrefs.GetFloat("volume");

            MusicaDeFundo.Play();

            // Garantir slider da cena atual (se existir)
            ConfigureSliderInScene();

            // Sempre que a cena carregar, tentar reconectar slider
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ConfigureSliderInScene();
    }

    private void ConfigureSliderInScene()
    {
        // Pega o slider por tag
        if (VolumeMusica == null)
            VolumeMusica = GameObject.FindWithTag("VolumeSlider")?.GetComponent<Slider>();

        if (VolumeMusica != null)
        {
            VolumeMusica.onValueChanged.RemoveAllListeners();

            VolumeMusica.value = MusicaDeFundo.volume;
            VolumeMusica.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float v)
    {
        MusicaDeFundo.volume = v;
        PlayerPrefs.SetFloat("volume", v);
    }

    public void PauseMusic()
    {
        MusicaDeFundo.Pause();
    }

    public void UnPauseMusic()
    {
        MusicaDeFundo.UnPause();
    }

    public void TryReconnectSlider()
    {
        // procura slider ATIVO dentro do menu recém-ativado
        var novo = GameObject.FindWithTag("VolumeSlider");

        if (novo != null)
        {
            VolumeMusica = novo.GetComponent<Slider>();

            VolumeMusica.onValueChanged.RemoveAllListeners();
            VolumeMusica.value = MusicaDeFundo.volume;
            VolumeMusica.onValueChanged.AddListener(SetVolume);
        }
    }
}