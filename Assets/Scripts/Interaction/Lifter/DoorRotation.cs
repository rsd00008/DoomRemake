using System;
using UnityEngine;

public class DoorRotation : MonoBehaviour
{
    public static event Action<bool> OnDoorMovementComplete; // bool indica si es FirstDoor
    public GameObject ReferenceObject;
    public float rotationSpeed = 10.0f;
    public bool isFirstDoor; // True para FirstDoor, False para SecondDoor
    private float rotationTarget;
    private bool isRotating = false;

    public void Activate(bool closeDoor)
    {
        rotationTarget = closeDoor ? 0f : 104f;
        isRotating = true;
    }

    void Update()
    {
        if (isRotating)
        {
            RotateDoor();
        }
    }
    void RotateDoor()
    {
        float step = rotationSpeed * Time.deltaTime * (rotationTarget == 0f ? -1 : 1);
        transform.RotateAround(ReferenceObject.transform.position, Vector3.up, step);

        if (Mathf.Abs(transform.localEulerAngles.y - rotationTarget) < 1f)
        {
            isRotating = false;
            transform.localEulerAngles = new Vector3(0, rotationTarget, 0); // Ajuste final para precisión
            OnDoorMovementComplete?.Invoke(isFirstDoor);
        }
    }
}
