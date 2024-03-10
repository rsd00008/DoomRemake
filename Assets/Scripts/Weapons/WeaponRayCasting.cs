using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRayCasting : MonoBehaviour
{
    [SerializeField] private Transform shootPosition;
    public float laserRange = 15f;
    public float laserDuration = 0.5f;
    public float damage = 20f;

    public Camera cam;
    private LineRenderer lineRenderer;
    public bool hasShooted = false; //para detectar si el jugador ha disparado

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            hasShooted = true;
            
            RaycastHit hit;
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            Ray ray = new Ray(rayOrigin, cam.transform.forward);
            
            lineRenderer.SetPosition(0, shootPosition.position);

            // Ajusta siempre el punto final del láser al centro de la pantalla, hasta el rango máximo
            Vector3 endPosition = rayOrigin + (cam.transform.forward * laserRange);
            lineRenderer.SetPosition(1, endPosition);

            if (Physics.Raycast(ray, out hit, laserRange)) // Verifica si el rayo impacta algo dentro del rango
            {
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    // Aplica daño si el objeto impactado es un enemigo
                    LifeBarLogic enemyLife = hit.collider.GetComponent<LifeBarLogic>();

                    if (enemyLife != null){
                        enemyLife.TakeDamage(damage);
                    }
                }
            }

            StartCoroutine(RenderLine());
        }
    }
    
    IEnumerator RenderLine()
    {
        lineRenderer.enabled = true; //habilitamos lineRenderer
        yield return new WaitForSeconds(laserDuration); //lo habilitamos por un tiempo determinado por laserDuration
        lineRenderer.enabled = false;  //deshabilitamos lineRenderer
    }
}
