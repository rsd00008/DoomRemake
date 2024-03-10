using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EntryRotation : MonoBehaviour
{
    public GameObject ReferenceObject;
    public float rotationSpeed = 10.0f;
    public float rotationTarget;
    private bool finish = false;

    private bool startRotation = false;
    void OnEnable()
    {
        ShipMovement.OnMovementComplete += StartRotation;
    }

    void OnDisable()
    {
        ShipMovement.OnMovementComplete -= StartRotation;
    }

    void StartRotation()
    {
        startRotation = true;
    }

    void Update()
    {
        if (startRotation == true && finish == false)
        {
            // Calcula la rotación deseada
            transform.RotateAround(ReferenceObject.transform.position, ReferenceObject.transform.up, rotationSpeed * Time.deltaTime);

            // Convertir la rotación actual del objeto a ángulos de Euler y obtener el ángulo en el eje Z
            float currentRotationZ = transform.eulerAngles.z;

            // Normalizar los ángulos para comparar
            currentRotationZ = (currentRotationZ > 180) ? currentRotationZ - 360 : currentRotationZ;

            Debug.Log("Mathf.Abs(currentRotationZ - rotationTarget): " + Mathf.Abs(currentRotationZ - rotationTarget));

            if (Mathf.Abs(currentRotationZ - rotationTarget) < 1f) // Asumiendo un pequeño margen de tolerancia
            {
                finish = true; // Marca que la rotación se ha completado
            }
        }
    }
}
