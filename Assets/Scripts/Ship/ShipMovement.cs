using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ShipMovement : MonoBehaviour, IAction
{
    public static event Action OnMovementComplete;// Declaración del evento público que notifica cuando la animación ha terminado
    private Vector3 startPosition; //estado inicial
    public Vector3 targetPosition; //estado final
    public float lerpDuration; //duracion de transicion

    public TaskIconMovement taskIconMovement;

    public TextMeshProUGUI taskText;

    public AnimationCurve curve; //curva de animacion -> muestra como se va a realizar la animacion (rapido al principio y al final pero lento a la mitad, rapido solo al principio y el resto lento, cada vez mas rapido, ... eje x de la curva es el tiempo y el eje y es la velocidad)

    private Boolean activated = false;

    public GameObject ObjectToDisappear;
    private Renderer Renderer_ObjectToDisappear; // Renderer del objeto que desaparece

    // Start is called before the first frame update
    void Start()
    {
        Renderer_ObjectToDisappear = ObjectToDisappear.GetComponent<Renderer>();
        SetInitialTransparency(1.0f); // Asegura que el objeto que desaparece sea inicialmente visible
        startPosition = transform.position;
    }

    void SetInitialTransparency(float alpha)
    {
        Color color = Renderer_ObjectToDisappear.material.color;
        color.a = alpha;
        Renderer_ObjectToDisappear.material.color = color;
    }

    public void Activate()
    {
        if (activated == false)
        {
            taskIconMovement.enabled = false;
            taskText.text = "Talk to Kyle";
            StartCoroutine(LerpPosition(startPosition, targetPosition, lerpDuration)); //Se ejecuta corutina con los parametros aportados
            activated = true;
        }
    }

    //corutina para hacer el Lerp (pasar de un valor inicial a final) en un tiempo determinado
    IEnumerator LerpPosition(Vector3 start, Vector3 target, float lerpDuration)
    {
        float timeElapsed = 0; //lleva cuenta tiempo transcurrido

        while (timeElapsed < lerpDuration)
        {
            //transicionamos entre dos valores
            transform.position = Vector3.Lerp(start, target, curve.Evaluate(timeElapsed / lerpDuration)); //evaluate devuelve puntos de la curva en funcion de los parametros que le pasamos (el tiempo)
            timeElapsed = timeElapsed + Time.deltaTime; // time.deltaTime es tiempo que transcurre en cada frame

            hacerDesaparecer();

            yield return null; //detiene ejecucion de corutina hasta el proximo frame
        }

        transform.position = target; //como nunca llega a 2 segundos puesto que deltaTime es variable, le asignamos 50 a 
        Destroy(ObjectToDisappear);

        taskIconMovement.enabled = true;

        // Disparamos evento para rotar entrada al acabar el script
        OnMovementComplete?.Invoke();
    }

    void hacerDesaparecer()
    {
        // Calcula el porcentaje de progreso basado en el tiempo, no en la distancia
        float progress = Mathf.Clamp01(Vector3.Distance(transform.position, startPosition) / Vector3.Distance(startPosition, targetPosition));

        // Invierte el progreso para desvanecer (1.0 = completamente visible, 0.0 = completamente invisible)
        float alpha = 1.0f - progress;

        SetTransparency(alpha);
    }

    void SetTransparency(float alpha)
    {
        // Asegura que el valor alfa no sea mayor que 1 ni menor que 0
        alpha = Mathf.Clamp(alpha, 0f, 1f);

        // Aplica el valor alfa al material del objeto que desaparece
        Color color = Renderer_ObjectToDisappear.material.color;
        color.a = alpha;

        Renderer_ObjectToDisappear.material.color = color;
    }
}
