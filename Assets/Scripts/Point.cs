using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private int score = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Hero.Instance.gameObject) // Проверяем, что персонаж вошел в триггер
        {
            Hero.Instance.GetScore(score); 
            Destroy(gameObject); 
        }
    }
}
