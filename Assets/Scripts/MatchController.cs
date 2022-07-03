using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] EventData eventData;
    [SerializeField] MatchData matchData;

    List<MatchingObject> matchingObjects = new List<MatchingObject>();

    bool canMatch = true;
    bool isMatch;
    float currentComboTime;
    bool isCombo;
    int combo;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        currentComboTime = 0;
    }

    private void OnEnable()
    {
        eventData.OnCollectObject += AddObject;
    }

    private void OnDisable()
    {
        eventData.OnCollectObject -= AddObject;
    }

    private void Update()
    {
        if (currentComboTime > 0)
        {
            currentComboTime -= Time.deltaTime;
            eventData.ComboTime?.Invoke(currentComboTime / matchData.ComboTime, combo);

            if (!isCombo)
            {
                isCombo = true;
            }
        }
        else if(isCombo)
        {
            isCombo = false;
            combo = 0;
        }
    }

    #endregion

    #region Unique Methods

    void CheckMatching()
    {
        isMatch = false;

        for (int i = 1; i < matchingObjects.Count - 1; i++)
        {
            if (matchingObjects[i - 1].FruitType == matchingObjects[i].FruitType && matchingObjects[i].FruitType == matchingObjects[i + 1].FruitType)
            {
                isMatch = true;
                StartCoroutine(UpdateArea(matchingObjects[i - 1], matchingObjects[i], matchingObjects[i + 1]));
                break;
            }
        }

        if (!isMatch)
        {
            canMatch = true;

            if (matchData.MaxObjectCount == matchingObjects.Count)
            {
                eventData.OnLose?.Invoke();
                Debug.Log("GameOver");
            }
        }
    }

    public void AddObject(MatchingObject matchingObject)
    {
        if (matchData.MaxObjectCount == matchingObjects.Count || !canMatch) return;

        StartCoroutine(AddObjectCoroutine(matchingObject));
    }

    #endregion

    #region Coroutines

    IEnumerator AddObjectCoroutine(MatchingObject matchingObject)
    {
        matchingObject.transform.parent = transform;
        Vector3 newMtachPos = Vector3.right * matchingObjects.Count;
        matchingObjects.Add(matchingObject);
        matchingObject.IsCollected = true;
        canMatch = false;

        while (Vector3.Distance(matchingObject.transform.localPosition, newMtachPos) > 0.01f)
        {
            float speed = Mathf.Clamp(Vector3.Distance(matchingObject.transform.localPosition, newMtachPos), 1, 20) * matchData.CollectSpeed * Time.deltaTime;
            matchingObject.transform.localPosition = Vector3.Lerp(matchingObject.transform.localPosition, newMtachPos, speed);
            matchingObject.transform.localRotation = Quaternion.Lerp(matchingObject.transform.localRotation, Quaternion.identity, speed);
            yield return null;
        }

        matchingObject.transform.localPosition = newMtachPos;
        matchingObject.transform.localRotation = Quaternion.identity;

        CheckMatching();
    }

    IEnumerator UpdateArea(params MatchingObject[] matchings)
    {
        currentComboTime = matchData.ComboTime;
        combo++;
        eventData.OnCollectStar?.Invoke(combo);
        eventData.OnMoveStar?.Invoke(matchings[1].transform.position, combo);

        Vector3 movePos = matchings[1].transform.localPosition;
        float currentTime = matchData.MatchTime;

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            foreach (MatchingObject item in matchings)
            {
                item.transform.localPosition = Vector3.Lerp(item.transform.localPosition, movePos, 0.08f);
                item.transform.localScale = Vector3.Lerp(item.transform.localScale, Vector3.zero, 0.01f);
            }

            yield return null;
        }


        for (int i = 0; i < matchings.Length; i++)
        {
            Destroy(matchings[i].gameObject);
            matchingObjects.Remove(matchings[i]);
        }

        CheckMatching();
    }

    #endregion
}
