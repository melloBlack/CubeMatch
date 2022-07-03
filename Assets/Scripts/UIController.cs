using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] EventData eventData;
    [SerializeField] RectTransform comboBarFront;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI starText;

    int starCount;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        eventData.SetUIController(this);
        comboBarFront.anchorMax = Vector2.up;
    }

    private void OnEnable()
    {
        eventData.ComboTime += ComboTime;
        eventData.OnCollectStar += UpdateStar;
    }
    private void OnDisable()
    {
        eventData.ComboTime -= ComboTime;
        eventData.OnCollectStar -= UpdateStar;
    }

    #endregion

    #region Unique Methods

    void ComboTime(float comboTime, int combo)
    {
        comboBarFront.anchorMax = new Vector2(comboTime, comboBarFront.anchorMax.y);

        comboText.text = comboTime <= 0 ? $"" : $"x{combo}";
    }

    void UpdateStar(int star)
    {
        starCount += star;
        starText.text = $"{starCount}";
    }

    #endregion
}
