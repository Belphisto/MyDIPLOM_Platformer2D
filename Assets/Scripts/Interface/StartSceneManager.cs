using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlaySound(0);
    }
    public void StartGame()
    {
        // Сохраняем выбранный уровень сложности
        int difficulty = DifficultyButton.Instance.GetLevelDifficulty();
        if (difficulty == 0)
            return;
        PlayerPrefs.SetInt("Difficulty", difficulty);
        SceneManager.LoadScene("TestLevel"); //  имя игровой сцены
    }


    public void QuitGame()
    {
        // Выходим из игры
        Application.Quit();
    }
}
