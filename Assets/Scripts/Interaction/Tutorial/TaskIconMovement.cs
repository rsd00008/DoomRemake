using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskIconMovement : MonoBehaviour
{
    public Transform kyleTransform; // Asigna esto en el Inspector con el transform de Kyle
    public float rotationSpeed = 50f; // Velocidad de rotación
    public float floatSpeed = 0.5f; // Velocidad de movimiento vertical
    public float floatHeight = 0.5f; // Altura máxima del movimiento vertical

    private Vector3 offset; // Offset respecto a Kyle

    void Start()
    {
        kyleTransform = transform.parent.transform; // Busca el GameObject con el nombre "Kyle" y obtiene su transform
        offset = transform.position - kyleTransform.position; // Calcula el offset inicial respecto a Kyle
    }

    void Update()
    {
        // Actualiza la posición inicial basándose en la posición de Kyle
        Vector3 startPos = kyleTransform.position + offset;

        // Rotación
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Movimiento vertical
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight + startPos.y;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}