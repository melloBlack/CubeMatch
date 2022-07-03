using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] EventData eventData;

    [Header("Combo Elements")]
    [SerializeField] RectTransform comboBarFront;
    [SerializeField] TextMeshProUGUI comboText;

    [Header("Star Elements")]
    [SerializeField] TextMeshProUGUI starText;

    [Header("Pause Panel Elements")]
    [SerializeField] GameObject pausePanel;
    [SerializeField] Button startButton;

    [Header("Game Over Panel Elements")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Button restartButton;

    [Header("Victory Panel Elements")]
    [SerializeField] GameObject victoryPanel;
    [SerializeField] Button nextLevelButton;
    [SerializeField] TextMeshProUGUI totalStarText;

    int starCount;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame);
        nextLevelButton.onClick.AddListener(RestartGame);
        pausePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        comboBarFront.anchorMax = Vector2.up;
    }

    private void OnEnable()
    {
        eventData.ComboTime += ComboTime;
        eventData.OnCollectStar += UpdateStar;
        eventData.OnLose += GameOver;
        eventData.OnVictory += Victory;
    }
    private void OnDisable()
    {
        eventData.ComboTime -= ComboTime;
        eventData.OnCollectStar -= UpdateStar;
        eventData.OnLose -= GameOver;
        eventData.OnVictory -= Victory;
    }

    #endregion

    #region Unique Methods

    void ComboTime(float comboTime, int combo)
    {
        comboBarFront.anchorMax = new Vector2(comboTime, comboBarFront.anchorMax.y);

        comboText.text = comboTime <= 0 ? $"" : $"x{combo}";
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    void RestartGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    void StartGame()
    {
        eventData.OnStart?.Invoke();
        pausePanel.SetActive(false);
    }

    void UpdateStar(int star)
    {
        starCount += star;
        starText.text = $"{starCount}";
    }

    void Victory()
    {
        victoryPanel.SetActive(true);
        totalStarText.text = $"{starCount}";
    }

    #endregion
}
