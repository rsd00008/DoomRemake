using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRayCasting : MonoBehaviour
{
    [SerializeField] private Transform shootPosition;
    public float range = 15f;
    public float damage = 20f;
    public float fireRate = 15f;
    private float timeElapsed = 0f; // tiempo transcurrido desde el �ltimo disparo

    public TrailRenderer laserTrail;

    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect; //efecto de impacto del l�ser

    public Camera cam;
    public bool hasShooted = false; //para detectar si el jugador ha disparado

    private void Start() {
        timeElapsed = 0f;
        hasShooted = false;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;  

        if (Input.GetButtonDown("Fire1") && timeElapsed >= 1f / fireRate) // Si el jugador presiona el bot�n de disparo y ha pasado el tiempo de recarga
        {
            timeElapsed = 0f;
            hasShooted = true;

            foreach (ParticleSystem muzzle in muzzleFlash) {
                muzzle.Emit(1);
            }

            RaycastHit hit;
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

            Ray ray = new Ray(rayOrigin, cam.transform.forward);
            
            var tracer = Instantiate(laserTrail, shootPosition.position, Quaternion.identity);
            tracer.AddPosition(shootPosition.position);

            if (Physics.Raycast(ray, out hit, range)) // Verifica si el rayo impacta algo dentro del rango
            {
                hitEffect.transform.position = hit.point; // Mueve el efecto de impacto al punto de colisi�n    
                hitEffect.transform.forward = hit.normal; // Ajusta la rotaci�n del efecto de impacto para que sea perpendicular a la superficie impactada
                hitEffect.Emit(1); // Emite el efecto de impacto

                tracer.transform.position = hit.point; // Mueve el rastro del l�ser al punto de colisi�n

                if (hit.collider.gameObject.tag == "Enemy")
                {
                    // Aplica da�o si el objeto impactado es un enemigo
                    LifeBarLogic enemyLife = hit.collider.GetComponent<LifeBarLogic>();

                    if (enemyLife != null){
                        enemyLife.TakeDamage(damage);
                    }
                }
            }
        }
    }
}
