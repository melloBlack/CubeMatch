using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float stopRotateSpeed;

    Rigidbody _rigidbody;
    MatchingObjectsGenerator _matchingObjectsGenerator;

    bool isDragging;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _matchingObjectsGenerator = GetComponentInChildren<MatchingObjectsGenerator>();
    }

    private void OnMouseDrag()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void Update()
    {
        if (!_matchingObjectsGenerator.GenerationDone) return;

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }
        else if (Input.GetMouseButton(0))
        {

        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            StartCoroutine(ResetAngularVelocity());
        }
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            float x = Input.GetAxis("Mouse Y") * rotateSpeed;
            float y = -Input.GetAxis("Mouse X") * rotateSpeed;
            _rigidbody.angularVelocity = new Vector3(x, y, 0);
        }
    }

    IEnumerator ResetAngularVelocity()
    {
        while (_rigidbody.angularVelocity.magnitude > 0.05f)
        {
            if (isDragging) yield break;

            _rigidbody.angularVelocity = Vector3.Lerp(_rigidbody.angularVelocity, Vector3.zero, stopRotateSpeed * Time.deltaTime);

            yield return null;
        }

        if (!isDragging)
        {
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}