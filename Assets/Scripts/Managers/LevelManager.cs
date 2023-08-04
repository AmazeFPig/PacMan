using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMono<LevelManager>
{
    [SerializeField]
    private int health = 3;

    [SerializeField]
    private int score = 0;

    [SerializeField]
    private Transform collectibles;

    private bool is600ScoreEventTriggered = false;
    private bool is1000ScoreEventTriggered = false;

    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<int>("DotEaten", UpdateScore);
        EventCenter.GetInstance().AddEventListener<int>("PowerUpEaten", UpdateScore);
        EventCenter.GetInstance().AddEventListener<int>("GhostEaten", UpdateScore);
        EventCenter.GetInstance().AddEventListener("PlayerDie", PlayerDie);
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
        if (!collectibles.GetComponentInChildren<Collectible>())
        {
            // Player Wins
            UIManager.GetInstance().DisplayFinishPanel("You Win!");
        }
    }

    private void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        Debug.Log($"Score: {score}");
        UIManager.GetInstance().UpdateScoreText(score);
    }

    private void PlayerDie()
    {
        health--;

        UIManager.GetInstance().UpdateHealthText(health);

        if (health <= 0)
        {
            // GameOver
            UIManager.GetInstance().DisplayFinishPanel("Game Over");
        }
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener<int>("DotEaten", UpdateScore);
        EventCenter.GetInstance().RemoveEventListener<int>("PowerUpEaten", UpdateScore);
        EventCenter.GetInstance().RemoveEventListener<int>("GhostEaten", UpdateScore);
    }
}
