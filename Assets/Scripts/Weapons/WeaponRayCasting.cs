using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRayCasting : MonoBehaviour
{
    [SerializeField] private Transform shootPosition;
    public float laserRange = 15f;
    public float laserDuration = 0.25f;

    public Camera cam;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")){
            RaycastHit hit;
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)); //convierte punto del viewport a world space. Al pasar 0.5 para x e y, le damos 50% del ancho y alto del viweport -> el rayo sale siempre en el centro de la pantalla
            Ray ray = new Ray(rayOrigin, cam.transform.forward); 

            lineRenderer.SetPosition(0, shootPosition.position); //linea que simula disparo empieza en el punto de la pantalla

            if(Physics.Raycast(ray, out hit)) //si impacta con algo 
            {
                lineRenderer.SetPosition(1, hit.point); //linea simulada tiene punto final en punto impacto de ray cast
                //Destroy(hit.transform.gameObject); //destruimos objeto
            }
            else //si no impacta con nada
            {
                lineRenderer.SetPosition(1, shootPosition.position + cam.transform.forward * laserRange); //dibujamos linea con rango definido por laserRange
            }

            StartCoroutine(RenderLine()); //para dibujar la linea
        }
    }
    IEnumerator RenderLine()
    {
        lineRenderer.enabled = true; //habilitamos lineRenderer
        yield return new WaitForSeconds(laserDuration); //lo habilitamos por un tiempo determinado por laserDuration
        lineRenderer.enabled = false;  //deshabilitamos lineRenderer
    }
}
