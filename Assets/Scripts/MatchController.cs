using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    [SerializeField] EventData eventData;
    [SerializeField] int maxObjectCount = 7;
    [SerializeField] float matchTime;
    [SerializeField] float collectSpeed;
    [SerializeField] float comboTime;

    List<MatchingObject> matchingObjects = new List<MatchingObject>();

    bool canMatch = true;
    bool isMatch;
    float currentComboTime;
    bool isCombo;
    int combo;

    void Start()
    {
        currentComboTime = 0;
        eventData.SetMatchController(this);
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
            eventData.ComboTime?.Invoke(currentComboTime / comboTime, combo);

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

            if (maxObjectCount == matchingObjects.Count)
            {
                Debug.Log("GameOver");
            }
        }
    }

    public void AddObject(MatchingObject matchingObject)
    {
        if (maxObjectCount == matchingObjects.Count || !canMatch) return;

        StartCoroutine(AddObjectCoroutine(matchingObject));
    }

    IEnumerator AddObjectCoroutine(MatchingObject matchingObject)
    {
        matchingObject.transform.parent = transform;
        Vector3 newMtachPos = Vector3.right * matchingObjects.Count;
        matchingObjects.Add(matchingObject);
        matchingObject.IsCollected = true;
        canMatch = false;

        while (Vector3.Distance(matchingObject.transform.localPosition, newMtachPos) > 0.01f)
        {
            float speed = Mathf.Clamp(Vector3.Distance(matchingObject.transform.localPosition, newMtachPos), 1, 20) * collectSpeed * Time.deltaTime;
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
        currentComboTime = comboTime;
        combo++;

        Vector3 movePos = matchings[1].transform.localPosition;
        float currentTime = matchTime;

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

    //IEnumerator CollateObjects()
    //{
    //    int count = matchingObjects.Count;

    //    for (int i = 0; i < count; i++)
    //    {
    //        matchingObjects[i].MoveToLocalPosition(Vector3.right * i);
    //        yield return new WaitForSeconds(collateInterval);
    //    }
    //}
}
