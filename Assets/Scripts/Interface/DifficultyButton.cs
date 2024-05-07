using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public static DifficultyButton Instance { get; private set; }
    public Toggle[] toggles;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Добавляем слушателей для каждого тоггла
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggle); });
        }
    }

    void OnToggleValueChanged(Toggle changedToggle)
    {
        if (changedToggle.isOn)
        {
            SoundManager.Instance.PlaySound(1);
            // Здесь вы можете добавить код, который будет выполняться при выборе уровня сложности
            Debug.Log("Выбран уровень сложности: " + changedToggle.name);

            // Выключаем все остальные тогглы
            foreach (Toggle toggle in toggles)
            {
                if (toggle != changedToggle)
                {
                    toggle.isOn = false;
                }
            }
        }
    }
    public int GetLevelDifficulty()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                // Возвращается уровень сложности в виде целого числа
                return i + 1;
            }
        }

        // Возвращается 0, если ни один из переключателей не включен
        return 0;
    }
}
