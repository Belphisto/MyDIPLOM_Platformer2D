using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


using TMPro;

namespace Platformer2D
{
    public class GameOver : MonoBehaviour
    {
        public GameObject panelGameWint;

        public GameObject panel;
        public GameObject panelSettings;
        public Button GameOverButton;
        public Button GameWinButton;
        //public UnityEngine.UI.Button returnToGame;

        //public UnityEngine.UI.Button Esc;
        public TextMeshProUGUI textTotalScore;

        private void Start()
        {
            // Подписываемся на событие нажатия кнопки
            GameOverButton.onClick.AddListener(OnOkButtonClicked);
            GameWinButton.onClick.AddListener(OnOkButtonClicked);

            Bus.Instance.PlayerFell +=OnPlayerFell;
            Bus.Instance.GameWin += GameCompleted;
            //UnityEngine.Cursor.visible = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Переключает активность панели при нажатии клавиши Esc
                panelSettings.SetActive(!panelSettings.activeSelf);
                //UnityEngine.Cursor.visible = !UnityEngine.Cursor.visible;
            }
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
                //UnityEngine.Cursor.visible = false;

            }
        }

        private void  GameCompleted(int count)
        {
            panelGameWint.gameObject.SetActive(true);
            textTotalScore.text = count.ToString();
            UnityEngine.Cursor.visible = false;
        }
    }
}

