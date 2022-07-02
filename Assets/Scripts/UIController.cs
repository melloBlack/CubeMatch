using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] EventData eventData;
    [SerializeField] RectTransform comboBarFront;
    [SerializeField] TextMeshProUGUI comboText;


    // Start is called before the first frame update
    void Start()
    {
        eventData.SetUIController(this);
        comboBarFront.anchorMax = Vector2.up;
    }

    private void OnEnable()
    {
        eventData.ComboTime += ComboTime;
    }
    private void OnDisable()
    {
        eventData.ComboTime -= ComboTime;
    }

    void ComboTime(float comboTime, int combo)
    {
        comboBarFront.anchorMax = new Vector2(comboTime, comboBarFront.anchorMax.y);

        comboText.text = comboTime <= 0 ? $"" : $"x{combo}";
    }
}
