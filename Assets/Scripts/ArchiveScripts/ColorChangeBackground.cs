using UnityEngine;
using UnityEngine.UI;

public class ColorChangeBackground : MonoBehaviour
{
    public float transitionSpeed = 1f;
    public Color targetColor = Color.white; // Целевой цвет - белый
    public int targetScore = 40; // Целевое количество очков для изменения цвета
    public float lighteningPercentagePerPoint = 0.01f; // Процентное значение изменения яркости за одно очко

    private Image image;
    private Color currentColor;
    private float t = 0f; // Значение t для интерполяции цвета
    private int previousScore = 0;

    void Start()
    {
        image = GetComponent<Image>(); 
        currentColor = image.color;

        // подписка на событие изменения счетчика очков из класса Hero
        Hero.OnScoreUpdate += HandleScoreUpdate;
    }
    void HandleScoreUpdate(int score)
    {
        // процент выполнения цели
        float progress = (float)score / targetScore;

        // остановка процесса изменения цвета, если цель достигнута
        if (progress >= 1f)
        {
            progress = 1f;
        }

        // обновить значение t
        t = progress;

        // интерполирование цвета от оригинального к целевому с использованием t
        Color newColor = new Color(
            Mathf.Lerp(currentColor.r, targetColor.r, t),
            Mathf.Lerp(currentColor.g, targetColor.g, t),
            Mathf.Lerp(currentColor.b, targetColor.b, t),
            currentColor.a // Прозрачность  без изменений
        );

        image.color = newColor;
    }

    void Update()
    {

    }
}
