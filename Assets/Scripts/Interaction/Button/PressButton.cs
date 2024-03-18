using UnityEngine;
using System;
using System.Collections;

public class PressButton : MonoBehaviour, IAction
{
    private Vector3 originalPosition;
    public Vector3 pressedPosition;
    private bool isPressed = false;
    public float moveSpeed = 10f; // The speed at which the button moves in and out

    void Start()
    {
        originalPosition = transform.localPosition; // Get the initial position in world space
    }

    public void Activate()
    {
        if (!isPressed)
        {
            isPressed = true; // Only allow the button to be pressed once
            StartCoroutine(MoveButton());
        }
    }

    private IEnumerator MoveButton()
    {
        // Move to pressed position
        while (Vector3.Distance(transform.localPosition, pressedPosition) > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, pressedPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        // Wait for a short time at the pressed position
        yield return new WaitForSeconds(0.5f);

        // Move back to original position
        while (Vector3.Distance(transform.localPosition, originalPosition) > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        isPressed = false; // Reset isPressed to allow for reactivation if needed
    }
}
