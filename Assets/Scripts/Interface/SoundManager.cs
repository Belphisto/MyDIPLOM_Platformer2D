using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    // Ссылки на все звуковые компоненты в игре
    public AudioSource[] audioSources;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Это делает объект постоянным между сценами
        }
    }

    void Start()
    {
        // Подписываемся на события изменения громкости
        SoundSettings.OnMusicVolumeChanged += UpdateMusicVolume;
        SoundSettings.OnSFXVolumeChanged += UpdateSFXVolume;
        // Устанавливаем громкость всех звуковых компонентов в соответствии с настройками звука
        // Устанавливаем начальную громкость
        UpdateMusicVolume(SoundSettings.Instance.MusicVolume);
        UpdateSFXVolume(SoundSettings.Instance.SFXVolume);
    }

    // Методы для воспроизведения и остановки звуков
    public void PlaySound(AudioSource audioSource)
    {
        audioSource.Play();
    }

    public void StopSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void PlaySound(int index)
    {
        audioSources[index].Play();
    }

    public void StopSound(int index)
    {
        audioSources[index].Stop();
    }
    void UpdateMusicVolume(float volume)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.CompareTag("Music"))
            {
                audioSource.volume = volume;
            }
        }
    }

    void UpdateSFXVolume(float volume)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.CompareTag("Music"))
            {
                audioSource.volume = volume;
            }
        }
    }
}
