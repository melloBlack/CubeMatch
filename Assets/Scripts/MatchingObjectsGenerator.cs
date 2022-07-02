using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingObjectsGenerator : MonoBehaviour
{
    [SerializeField] Vector3 size;
    [SerializeField] Vector3 firstPos;
    [SerializeField] float creatingInterval;
    [SerializeField] MatchingObject[] fruits;

    // Start is called before the first frame update
    void Start()
    {
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
                    newFruit.MoveToLocalPosition(new Vector3(i, j, k));
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
                    newFruit.MoveToLocalPosition(new Vector3(i, j, k));
                    yield return new WaitForSeconds(creatingInterval);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, transform.localScale);
        Gizmos.DrawCube(firstPos, transform.localScale);
    }
}
