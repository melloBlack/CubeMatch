using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesGenerator : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] EventData eventData;
    [SerializeField] Vector3 size;
    [SerializeField] Vector3 firstPos;
    [SerializeField] float creatingInterval;
    [SerializeField] int leftAmount;
    [SerializeField] MatchingObject[] objects;

    Vector3 centerPos;
    int totalSize;

    List<MatchingObject> matchingObjects = new List<MatchingObject>();

    #endregion

    #region MonoBehaviour Methods
    private void OnEnable()
    {
        eventData.OnStart += StartGame;
        eventData.OnCollectObject += RemoveObject;
    }

    private void OnDisable()
    {
        eventData.OnStart -= StartGame;
        eventData.OnCollectObject -= RemoveObject;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(centerPos, transform.localScale);
        Gizmos.DrawCube(firstPos, transform.localScale);
    }
    private void OnValidate()
    {
        centerPos = new Vector3(-size.x * 0.5f + 0.5f, -size.y * 0.5f + 0.5f, -size.z * 0.5f + 0.5f);
    }

    #endregion

    #region Listener Methods

    void StartGame()
    {
        transform.localPosition = new Vector3(-size.x * 0.5f + 0.5f, -size.y * 0.5f + 0.5f, -size.z * 0.5f + 0.5f);

        totalSize = (int) (size.x * size.y * size.z);

        for (int i = 0; i < totalSize; i++)
        {
            int index = (i % objects.Length + 1) - 1;
            MatchingObject newFruit = Instantiate(objects[index], firstPos, Quaternion.identity, transform);
            newFruit.gameObject.SetActive(false);
            matchingObjects.Add(newFruit);
        }

        StartCoroutine(SpawnCubesFirstHalf());
        StartCoroutine(SpawnCubesSecondHalf());
    }

    void RemoveObject(MatchingObject matchingObject)
    {
        matchingObjects.Remove(matchingObject);

        if (matchingObjects.Count <= leftAmount)
        {
            eventData.OnVictory?.Invoke();
        }
    }

    #endregion

    #region Coroutines

    IEnumerator SpawnCubesFirstHalf()
    {
        int index = 0;

        for (int i = 0; i < (int)size.x / 2; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                for (int k = 0; k < size.z; k++)
                {
                    matchingObjects[index].gameObject.SetActive(true);
                    matchingObjects[index].MoveToLocalPosition(new Vector3(i, j, k), true);
                    matchingObjects[index].SetEventData(eventData);
                    index++;

                    yield return new WaitForSeconds(creatingInterval);
                }
            }
        }
    }
    IEnumerator SpawnCubesSecondHalf()
    {
        yield return new WaitForSeconds(creatingInterval);

        int index = (int)((int)(size.x / 2) * size.y * size.z);

        for (int i = (int)size.x / 2; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                for (int k = 0; k < size.z; k++)
                {
                    matchingObjects[index].gameObject.SetActive(true);
                    matchingObjects[index].MoveToLocalPosition(new Vector3(i, j, k), true);
                    matchingObjects[index].SetEventData(eventData);
                    index++;

                    yield return new WaitForSeconds(creatingInterval);
                }
            }
        }

        yield return new WaitForSeconds(creatingInterval);

        eventData.OnPlay?.Invoke();
    }

    #endregion
}
