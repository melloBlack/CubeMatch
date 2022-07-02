using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingObject : MonoBehaviour
{
    [SerializeField] Enums.FruitType fruitType;
    [SerializeField] ParticleSystem creatingEffect;
    [SerializeField] float landingSpeed;

    public Enums.FruitType FruitType => fruitType;

    void Start()
    {
    }


    public void MoveToLocalPosition(Vector3 refPosition)
    {
        StartCoroutine(MoveToLocalPositionCoroutine(refPosition));
    }

    IEnumerator MoveToLocalPositionCoroutine(Vector3 refPosition)
    {
        creatingEffect.Play();

        while (Vector3.Distance(transform.localPosition, refPosition) > 0.01f)
        {
            float speed = Mathf.Clamp(Vector3.Distance(transform.localPosition, refPosition), 1 , 10) * landingSpeed * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, refPosition, speed);
            yield return null;
        }

        transform.localPosition = refPosition;
    }
}
