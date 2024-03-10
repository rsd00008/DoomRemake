using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInteraction : MonoBehaviour
{
    public enum TypeOfDetection
    {
        PointToReceiver,
        Radial3DNoDirection,
    };

    [SerializeField]
    private TypeOfDetection typeOfDetection;

    [SerializeField]
    private float minInteractionDistance = 5f;

    private Camera cam;


    private Ray ray;

    private bool canInteract;

    private InteractionReceiver currentReceiver;

    private UI ui;

    private void Start()
    {
        cam = GetComponent<Camera>();

        ui = FindObjectOfType<UI>();

        if (typeOfDetection == TypeOfDetection.Radial3DNoDirection)
        { 
            if (GetComponent<Rigidbody>() == null)
            {
                Debug.Log("The GameObject with the CheckInteraction Script requires a RigidBody component");
                Debug.Break();
            }

            if (GetComponent<SphereCollider>() == null)
            {
                Debug.Log("The GameObject with the CheckInteraction Script requires a SphereCollider component");
                Debug.Break();
            }
        }
    }

    void Update()
    {
        if (typeOfDetection == TypeOfDetection.PointToReceiver)
        {
            CheckRaycast();  
        }

        if (canInteract)
        {
            #region ESPAÑOL
            /*
            *En esta región el personaje está viendo un objeto con el que puede interactuar
            *En mi caso voy a hacer la lectura de la tecla E aquí mismo, pero esto se puede manejar de distintas formas
            */
            #endregion

            if (Input.GetKeyDown(KeyCode.E))
            {
                currentReceiver.Activate();
            }
        }
    }

    private void CheckRaycast()
    {
        RaycastHit hit;
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)); //convierte punto del viewport a world space. Al pasar 0.5 para x e y, le damos 50% del ancho y alto del viweport -> el rayo sale siempre en el centro de la pantalla

        ray = new Ray(rayOrigin, cam.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * 5, Color.green);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < minInteractionDistance)
            {
                currentReceiver = hit.transform.gameObject.GetComponent<InteractionReceiver>();

                if (currentReceiver != null)
                {
                    DetectingAReceiver();                 
                }
                else
                {
                    canInteract = false;
                }
            }
        }
    }

    private void DetectingAReceiver()
    {
        #region ESPAÑOL
        //Aquí puedes hacer algo con el mensaje de interacción
        #endregion

        canInteract = true;
        Debug.Log(currentReceiver.GetInteractionMessage());
    }


    private void OnTriggerStay(Collider other)
    {
        if (typeOfDetection == TypeOfDetection.Radial3DNoDirection)
        {
            if (other.gameObject.GetComponent<InteractionReceiver>() != null)
            {
                currentReceiver = other.gameObject.GetComponent<InteractionReceiver>();
                DetectingAReceiver();

            }

            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (typeOfDetection == TypeOfDetection.Radial3DNoDirection)
        {
            if (other.gameObject.GetComponent<InteractionReceiver>() != null)
            {
               
                currentReceiver =null;
            
                canInteract = false;
            }
        }
    }


}
