using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    private List<Platform> platforms = new List<Platform>();

    // Start is called before the first frame update
    void Start()
    {
        Platform[] platformObjects = FindObjectsOfType<Platform>();

        // Добавляем найденные платформы в список
        platforms.AddRange(platformObjects);

        // Выполняем настройку для каждой платформы
        StartPlatforms();
    }

    public void StartPlatforms()
    {
        foreach (Platform platform in platforms)
        {
            platform.TargetScore = Random.Range(10, 41); // Случайное количество очков от 10 до 40
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
