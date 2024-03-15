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
    private float timeElapsed = 0f; // tiempo transcurrido desde el último disparo

    public TrailRenderer laserTrail;

    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect; //efecto de impacto del láser

    public Camera cam;
    public bool hasShooted = false; //para detectar si el jugador ha disparado

    private void Start() {
        timeElapsed = 0f;
        hasShooted = false;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;  

        if (Input.GetButtonDown("Fire1") && timeElapsed >= 1f / fireRate) // Si el jugador presiona el botón de disparo y ha pasado el tiempo de recarga
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
                hitEffect.transform.position = hit.point; // Mueve el efecto de impacto al punto de colisión    
                hitEffect.transform.forward = hit.normal; // Ajusta la rotación del efecto de impacto para que sea perpendicular a la superficie impactada
                hitEffect.Emit(1); // Emite el efecto de impacto

                tracer.transform.position = hit.point; // Mueve el rastro del láser al punto de colisión

                if (hit.collider.gameObject.tag == "Enemy")
                {
                    // Aplica daño si el objeto impactado es un enemigo
                    LifeBarLogic enemyLife = hit.collider.GetComponent<LifeBarLogic>();

                    if (enemyLife != null){
                        enemyLife.TakeDamage(damage);
                    }
                }
            }
        }
    }
}
