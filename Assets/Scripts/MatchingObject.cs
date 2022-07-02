using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingObject : MonoBehaviour
{
    [SerializeField] Enums.FruitType fruitType;
    [SerializeField] ParticleSystem creatingEffect;
    [SerializeField] GameObject outline;
    [SerializeField] float landingSpeed;

    EventData eventData;

    bool _isCollected;

    public bool IsCollected
    {
        get { return _isCollected; }
        set { _isCollected = value; }
    }

    public Enums.FruitType FruitType => fruitType;

    private void Start()
    {
        outline.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if (_isCollected) return;

        outline.SetActive(true);
    }

    private void OnMouseUpAsButton()
    {
        if (_isCollected) return;

        eventData.OnCollectObject?.Invoke(this);
        outline.SetActive(false);
    }

    private void OnMouseExit()
    {
        if (_isCollected) return;

        outline.SetActive(false);
    }

    public void SetEventData(EventData data)
    {
        eventData = data;
    }

    public void MoveToLocalPosition(Vector3 refPosition, bool withEffect = false)
    {
        if (withEffect)
        {
            creatingEffect.Play();
        }

        StartCoroutine(MoveToLocalPositionCoroutine(refPosition));
    }

    IEnumerator MoveToLocalPositionCoroutine(Vector3 refPosition)
    {

        while (Vector3.Distance(transform.localPosition, refPosition) > 0.01f)
        {
            float speed = Mathf.Clamp(Vector3.Distance(transform.localPosition, refPosition), 1, 10) * landingSpeed * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, refPosition, speed);
            yield return null;
        }

        transform.localPosition = refPosition;
    }
}
