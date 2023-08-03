using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMono<LevelManager>
{
    [SerializeField]
    private int score = 0;

    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<int>("DotEaten", UpdateScore);
        EventCenter.GetInstance().AddEventListener<int>("PowerUpEaten", UpdateScore);
    }

    private void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        Debug.Log($"Score: {score}");
        UIManager.GetInstance().UpdateScoreText(score);
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener<int>("DotEaten", UpdateScore);
    }
}
