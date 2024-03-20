using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int TargetScore = 0;
    [SerializeField] private SpriteRenderer stateColorless;
    [SerializeField] private SpriteRenderer stateColor;
    
    // Start is called before the first frame update
    void Start()
    {
        Hero.OnScoreUpdate += HandleScoreUpdate;
    }
    private void Awake()
    {
        // Получаем ссылку на первый дочерний объект с компонентом SpriteRenderer
        stateColorless = transform.GetChild(0).GetComponent<SpriteRenderer>();
        // Получаем ссылку на второй дочерний объект с компонентом SpriteRenderer
        stateColor = transform.GetChild(1).GetComponent<SpriteRenderer>();
        stateColor.gameObject.SetActive(false);
    }

    private void HandleScoreUpdate(int score)
    {
        if (score >= TargetScore) ChangeState();
    }

    public void ChangeState()
    {
        stateColor.gameObject.SetActive(true);
        stateColorless.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
