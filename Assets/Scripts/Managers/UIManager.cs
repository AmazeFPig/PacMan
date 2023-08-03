using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : SingletonMono<UIManager>
{
    [SerializeField]
    private TMP_Text scoreText;
    public void UpdateScoreText(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}
