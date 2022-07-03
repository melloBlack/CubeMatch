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
    [SerializeField] MatchData matchData;

    [Header("Game Panel")]
    [SerializeField] GameObject gamePanel;
    [SerializeField] RectTransform comboBarFront;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI starText;
    [SerializeField] Button UndoButton;
    [SerializeField] TextMeshProUGUI undoAmountText;

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
    int undoAmount;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame);
        nextLevelButton.onClick.AddListener(RestartGame);
        UndoButton.onClick.AddListener(UndoLastObject);
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        comboBarFront.anchorMax = Vector2.up;
        undoAmount = matchData.UndoAmount;
        undoAmountText.text = $"{undoAmount}";
    }

    private void OnEnable()
    {
        eventData.ComboTime += ComboTime;
        eventData.OnCollectStar += UpdateStar;
        eventData.OnLose += GameOver;
        eventData.OnVictory += Victory;
        eventData.OnNullMatchArea += ReUndoAmount;
    }
    private void OnDisable()
    {
        eventData.ComboTime -= ComboTime;
        eventData.OnCollectStar -= UpdateStar;
        eventData.OnLose -= GameOver;
        eventData.OnVictory -= Victory;
        eventData.OnNullMatchArea -= ReUndoAmount;
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
        gamePanel.SetActive(true);
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

    void UndoLastObject()
    {
        if (undoAmount > 0)
        {
            eventData.OnClickUndo?.Invoke();
            undoAmount--;
            undoAmountText.text = $"{undoAmount}";
        }
    }

    void ReUndoAmount()
    {
        undoAmount++;
        undoAmountText.text = $"{undoAmount}";
    }

    #endregion
}
