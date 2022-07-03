using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAnimator : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] EventData eventData;
    [SerializeField] GameObject star;
    [SerializeField] int maxStart;
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 lastPos;
    [SerializeField] float firstScaleFactor;
    [SerializeField] float lastScaleFactor;

    Queue<GameObject> stars = new Queue<GameObject>();

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        Initiation();
    }
    private void OnEnable()
    {
        eventData.OnMoveStar += PlayStarAnimation;
    }

    private void OnDisable()
    {
        eventData.OnMoveStar -= PlayStarAnimation;
    }

    #endregion

    #region Unique Methods

    void Initiation()
    {
        for (int i = 0; i < maxStart; i++)
        {
            GameObject newStar = Instantiate(star, transform);
            newStar.SetActive(false);
            stars.Enqueue(newStar);
        }
    }

    void PlayStarAnimation(Vector3 startPos, int combo)
    {
        for (int i = 0; i < combo; i++)
        {
            float x = Random.Range(Mathf.Clamp(-i, -5f, 0f), Mathf.Clamp(i, 0f, 5f));
            float y = Random.Range(Mathf.Clamp(-i, -5f, 0f), Mathf.Clamp(i, 0f, 5f));
            
            Vector3 offset = new Vector3(x, y, 0);

            StartCoroutine(StarAnimation(startPos + offset));
        }
    }

    #endregion

    #region Coroutines

    IEnumerator StarAnimation(Vector3 startPos)
    {
        if (stars.Count == 0)
        {
            GameObject newStar = Instantiate(star, transform);
            newStar.SetActive(false);
            stars.Enqueue(newStar);
        }

        GameObject currentStar = stars.Dequeue();

        currentStar.transform.position = startPos;
        currentStar.transform.localScale = Vector3.one * Random.Range(firstScaleFactor, lastScaleFactor);
        currentStar.SetActive(true);

        while (Vector3.Distance(currentStar.transform.position, lastPos) > 0.1f)
        {
            float speed = Mathf.Clamp(Vector3.Distance(currentStar.transform.position, lastPos), 2, 5) * moveSpeed * Time.deltaTime;
            currentStar.transform.position = Vector3.Lerp(currentStar.transform.position, lastPos, speed);
            currentStar.transform.localScale = Vector3.Lerp(currentStar.transform.localScale, Vector3.one * lastScaleFactor, speed * 0.5f);

            yield return null;
        }

        currentStar.SetActive(false);

        stars.Enqueue(currentStar);
    }

    #endregion
}
