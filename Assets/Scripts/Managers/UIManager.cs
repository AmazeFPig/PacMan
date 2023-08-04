using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : SingletonMono<UIManager>
{
    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text healthText;

    [SerializeField]
    private RectTransform FinishPanel;

    [SerializeField]
    private TMP_Text FinishPanelText;

    [SerializeField]
    private Button BtnRestart;

    [SerializeField]
    private Button BtnQuit;

    private void Start()
    {
        BtnRestart.onClick.AddListener(Restart);
        BtnQuit.onClick.AddListener(Quit);

    }
    public void UpdateScoreText(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void UpdateHealthText(int health)
    {
        healthText.text = $"Health: {health}";
    }

    public void DisplayFinishPanel(string text)
    {
        FinishPanelText.text = text;
        FinishPanel.gameObject.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Restart()
    {
        Time.timeScale = 1;

        EventCenter.GetInstance().Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
