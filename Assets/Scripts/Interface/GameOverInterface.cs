using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Platformer2D
{
    public class GameOver : MonoBehaviour
    {
        public GameObject panel;
        public UnityEngine.UI.Button GameOverButton;

        private void Start()
        {
            // Подписываемся на событие нажатия кнопки
            GameOverButton.onClick.AddListener(OnOkButtonClicked);

            Bus.Instance.PlayerFell +=OnPlayerFell;
        }

        private void OnOkButtonClicked()
        {
            // Выгружаем текущую сцену
            //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

            // Загружаем сцену StartScene
            SceneManager.LoadScene("StartScene");
        }

        private void OnPlayerFell(bool isFell)
        {
            if (isFell)
            {
                Debug.Log("gameover");
                panel.SetActive(true);
            }
        }
    }
}

