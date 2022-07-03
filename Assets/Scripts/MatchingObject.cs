using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingObject : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] Enums.FruitType fruitType;
    [SerializeField] ParticleSystem creatingEffect;
    [SerializeField] GameObject outline;
    [SerializeField] float landingSpeed;

    EventData eventData;

    bool playability;
    bool _isCollected;

    public Enums.FruitType FruitType => fruitType;

    public bool IsCollected
    {
        get { return _isCollected; }
        set { _isCollected = value; }
    }

    #endregion

    #region MonoBehaviour Methods

    private void Start()
    {
        outline.SetActive(false);
    }

    private void OnDisable()
    {
        if (!eventData) return;

        eventData.OnPlay -= PlayGame;
        eventData.OnPause -= PauseGame;
        eventData.OnLose -= PauseGame;
    }

    private void OnMouseEnter()
    {
        if (_isCollected || !playability) return;

        outline.SetActive(true);
    }

    private void OnMouseUpAsButton()
    {
        if (_isCollected || !playability) return;

        eventData.OnCollectObject?.Invoke(this);
        outline.SetActive(false);
    }

    private void OnMouseExit()
    {
        if (_isCollected) return;

        outline.SetActive(false);
    }

    #endregion

    #region Unique Methods

    #region Listener Methods

    void PlayGame()
    {
        playability = true;
    }
    void PauseGame()
    {
        playability = false;
    }

    #endregion

    public void SetEventData(EventData data)
    {
        eventData = data;
        eventData.OnPlay += PlayGame;
        eventData.OnPause += PauseGame;
        eventData.OnLose += PauseGame;
    }

    public void MoveToLocalPosition(Vector3 refPosition, bool withEffect = false)
    {
        if (withEffect)
        {
            creatingEffect.Play();
        }

        StartCoroutine(MoveToLocalPositionCoroutine(refPosition));
    }

    #endregion

    #region Coroutines

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

    #endregion
}
