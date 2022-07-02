using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesGenerator : MonoBehaviour
{
    [SerializeField] EventData eventData;
    [SerializeField] Vector3 size;
    [SerializeField] Vector3 firstPos;
    [SerializeField] float creatingInterval;
    [SerializeField] MatchingObject[] fruits;

    Vector3 centerPos;

    void Start()
    {
        transform.localPosition = new Vector3(-size.x * 0.5f + 0.5f, -size.y * 0.5f + 0.5f, -size.z * 0.5f + 0.5f);
        eventData.SetGenerator(this);
        StartCoroutine(SpawnCubesFirstHalf());
        StartCoroutine(SpawnCubesSecondHalf());
    }

    IEnumerator SpawnCubesFirstHalf()
    {
        for (int i = 0; i < (int)size.x / 2; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                for (int k = 0; k < size.z; k++)
                {
                    int fruitIndex = Random.Range(0, fruits.Length);
                    MatchingObject newFruit = Instantiate(fruits[fruitIndex], firstPos, Quaternion.identity, transform);
                    newFruit.MoveToLocalPosition(new Vector3(i, j, k), true);
                    newFruit.SetEventData(eventData);
                    yield return new WaitForSeconds(creatingInterval);
                }
            }
        }
    }
    IEnumerator SpawnCubesSecondHalf()
    {
        yield return new WaitForSeconds(creatingInterval);

        for (int i = (int)size.x / 2; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                for (int k = 0; k < size.z; k++)
                {
                    int fruitIndex = Random.Range(0, fruits.Length);
                    MatchingObject newFruit = Instantiate(fruits[fruitIndex], firstPos, Quaternion.identity, transform);
                    newFruit.MoveToLocalPosition(new Vector3(i, j, k), true);
                    newFruit.SetEventData(eventData);
                    yield return new WaitForSeconds(creatingInterval);
                }
            }
        }

        eventData.GenerationIsDone();
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
}
