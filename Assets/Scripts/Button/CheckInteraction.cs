using System;
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

    public TextMesh textMesh;

    private ArrayList activatedButtons = new ArrayList(); //lista de botones que han sido activados, para que no puedan volver a ser activados

    private void Start()
    {
        activatedButtons = new ArrayList();

        cam = GetComponent<Camera>();

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

        if (canInteract == true)
        {
            if (Input.GetKeyDown(KeyCode.E) && currentReceiver != null)
            {
                if(activatedButtons.Contains(currentReceiver.name) == false)
                { 
                    if(currentReceiver.name != "KyleRobot"){
                        activatedButtons.Add(currentReceiver.name);
                    }
                    
                    currentReceiver.Activate();
                    textMesh.gameObject.SetActive(false);
                }
            }
        }
    }

    private void CheckRaycast()
    {
        RaycastHit hit;
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        ray = new Ray(rayOrigin, cam.transform.forward);

        Debug.DrawRay(rayOrigin, cam.transform.forward * minInteractionDistance, Color.green);
        

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < minInteractionDistance)
            {
                InteractionReceiver receiver = hit.transform.gameObject.GetComponent<InteractionReceiver>();

                if (receiver != null){
                    if(receiver == currentReceiver && activatedButtons.Contains(currentReceiver.name) == false){
                        // El receptor es el mismo que ya estábamos apuntando
                        canInteract = true;
                        textMesh.gameObject.SetActive(true);
                        textMesh.text = receiver.GetInteractionMessage();

                    }else{
                        // Hemos apuntado a un nuevo receptor
                        if (currentReceiver != null){
                            // Desactivar el mensaje del receptor anterior
                            textMesh.gameObject.SetActive(false);
                        }

                        currentReceiver = receiver;
                        canInteract = true;

                        if(textMesh != null && currentReceiver != null && activatedButtons.Contains(currentReceiver.name) == false){
                            textMesh.gameObject.SetActive(true);
                            textMesh.text = receiver.GetInteractionMessage();
                        }
                    }
                }
            }
            else
            {
                // El hit es válido pero está demasiado lejos
                if (currentReceiver != null)
                {
                    textMesh.gameObject.SetActive(false);
                }

                canInteract = false;
                currentReceiver = null;
            }
        }
        else
        {
            // No hay un hit
            if (currentReceiver != null)
            {
                textMesh.gameObject.SetActive(false);
            }
            canInteract = false;
            currentReceiver = null;
        }
    }







    private void DetectingAReceiver()
    {
        canInteract = true;
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
