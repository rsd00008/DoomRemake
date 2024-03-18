using UnityEngine;
using System;
using System.Collections;

public class LifterMovement : MonoBehaviour
{
    public static event Action OnElevatorReachedTarget; // Evento para notificar que el ascensor ha llegado
    public Vector3 zeroFloor;
    public Vector3 firstFloor;
    public float lerpDuration;
    public AnimationCurve curve;
    private bool activated = false;

    // Nueva referencia para almacenar el CharacterController del jugador
    private CharacterController playerController;
    private Transform playerTransform; // Almacena la transformada del jugador para ajustes de posición

    public void Activate()
    {
        if (!activated)
        {
            Vector3 targetPosition = transform.localPosition == zeroFloor ? firstFloor : zeroFloor;
            StartCoroutine(LerpPosition(targetPosition));
        }
    }

    IEnumerator LerpPosition(Vector3 target)
    {
        activated = true;
        float timeElapsed = 0;
        Vector3 startPosition = transform.localPosition;

        while (timeElapsed < lerpDuration)
        {
            Vector3 newPosition = Vector3.Lerp(startPosition, target, curve.Evaluate(timeElapsed / lerpDuration));
            Vector3 displacement = newPosition - transform.localPosition;
            displacement.y += 0.01f;            

            transform.localPosition = newPosition;

            // Si hay un jugador en el ascensor, ajusta su posición directamente
            if (playerController != null && playerTransform != null)
            {
                playerTransform.position += displacement;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        activated = false;
        OnElevatorReachedTarget?.Invoke();
    }

    public bool IsActivated()
    {
        return activated;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<CharacterController>();
            playerTransform = other.transform; // Obtiene y almacena la transformada del jugador
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = null;
            playerTransform = null; // Limpia la referencia cuando el jugador sale del ascensor
        }
    }

}
