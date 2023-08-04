using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMono<LevelManager>
{
    [SerializeField]
    private int score = 0;

    private bool is600ScoreEventTriggered = false;
    private bool is1000ScoreEventTriggered = false;

    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<int>("DotEaten", UpdateScore);
        EventCenter.GetInstance().AddEventListener<int>("PowerUpEaten", UpdateScore);
        EventCenter.GetInstance().AddEventListener<int>("GhostEaten", UpdateScore);
    }

    private void Update()
    {
        if (score >= 600 && !is600ScoreEventTriggered)
        {
            EventCenter.GetInstance().EventTrigger("600Score");
            is600ScoreEventTriggered = true;
        }
        if (score >= 1000 && !is1000ScoreEventTriggered)
        {
            EventCenter.GetInstance().EventTrigger("1000Score");
            is1000ScoreEventTriggered = true;
        }
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
        EventCenter.GetInstance().RemoveEventListener<int>("PowerUpEaten", UpdateScore);
        EventCenter.GetInstance().RemoveEventListener<int>("GhostEaten", UpdateScore);
    }
}
