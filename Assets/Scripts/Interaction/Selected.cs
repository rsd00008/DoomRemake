using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    private LayerMask mask; // LayerMask to filter the objects that are selectable - rayo solo interactua con este mask
    public float distance = 2f; //distancia para seleccionar
    public Texture2D pointer; //cursor
    public GameObject texto; //texto que aparece al seleccionar un objeto
    private GameObject lastDetected = null; //ultimo objeto detectado
    private Material lastDetectedMaterial = null; //material del ultimo objeto detectado


    void Start()
    {
        mask = LayerMask.GetMask("Default");
        texto.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance, mask))
        {
            Deselect();
            SelectedObject(hit.transform);

            if(hit.collider != null)
            {
                if(hit.collider.tag == "Interactive")
                {
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        hit.collider.GetComponent<InteractiveObject>().Interact();
                    }
                }
            }

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * distance, Color.yellow);

        }else{
            Deselect();
        }
    }

    void SelectedObject(Transform t)
    {
        lastDetectedMaterial = t.GetComponent<MeshRenderer>().material;
        t.GetComponent<MeshRenderer>().material.color = Color.green;
        lastDetected = t.gameObject;  
    }

    void Deselect(){
        if(lastDetected != null)
        {
            lastDetected.GetComponent<MeshRenderer>().material = lastDetectedMaterial;
            lastDetected = null;
        }
    }

    void OnGUI()
    {
        Rect rect = new Rect(Screen.width/2, Screen.height/2, pointer.width, pointer.height);
        GUI.DrawTexture(rect, pointer);

        if(lastDetected != null)
        {
            texto.SetActive(true);
            texto.transform.position = Camera.main.WorldToScreenPoint(lastDetected.transform.position);

        }else{
            texto.SetActive(false);
        }
    }   
}
