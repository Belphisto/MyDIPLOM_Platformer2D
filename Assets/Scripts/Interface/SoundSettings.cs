using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    public static SoundSettings Instance { get; private set; }
    // События для изменения громкости музыки и звуковых эффектов
    public delegate void OnVolumeChanged(float volume);
    public static event OnVolumeChanged OnMusicVolumeChanged;
    public static event OnVolumeChanged OnSFXVolumeChanged;
    
    // Слайдеры для управления громкостью музыки и звуковых эффектов
    public Slider musicSlider;
    public Slider sfxSlider;

    // Значения громкости по умолчанию
    private const float DefaultMusicVolume = 0.5f;
    private const float DefaultSFXVolume = 0.5f;

    // Свойства для громкости музыки и звуковых эффектов
    public float MusicVolume { get; private set; }
    public float SFXVolume { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // объект постоянный между сценами
        }

        // Загрузка сохраненных настроек или значений по умолчанию
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", DefaultMusicVolume);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", DefaultSFXVolume);

        // Установка значений слайдеров
        musicSlider.value = MusicVolume;
        sfxSlider.value = SFXVolume;

        // Добавление слушателей для слайдеров
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Методы для изменения и сохранения настроек звука
    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        OnMusicVolumeChanged?.Invoke(volume); // Вызов событиея
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        OnSFXVolumeChanged?.Invoke(volume); // Вызов событиея
    }
}
