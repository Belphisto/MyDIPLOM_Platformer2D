using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    public GameObject PanelManual;
    public GameObject Player;
    private void Start()
    {
        SoundManager.Instance.PlaySound(0);
        
    }
    public void OK()
    {
        //не работает
        if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            SceneManager.UnloadSceneAsync(1);
        }
        SceneManager.LoadScene("TestLevel"); //  имя игровой сцены
    }
    public void StartGame()
    {
        int difficulty = DifficultyButton.Instance.GetLevelDifficulty();
        if (difficulty == 0)
            return;
        PlayerPrefs.SetInt("Difficulty", difficulty);
        PanelManual.SetActive(true);
        Player.SetActive(false);
    }

    public void QuitGame()
    {
        // Выходим из игры
        Application.Quit();
    }

    // Обновление каждый кадр
    void Update()
    {

            return;

    }

    

}
