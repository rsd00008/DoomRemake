using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRaycast : MonoBehaviour
{
    public bool isFiring = false;
    public ParticleSystem[] muzzleFlash;
    public Transform shootPosition;

    Ray ray;
    RaycastHit hit;

    private void Update() {
        if(Input.GetButtonDown("Fire1")){
            isFiring = true;
            
            foreach (ParticleSystem muzzle in muzzleFlash) {
                muzzle.Emit(1);
            }

            ray.origin = shootPosition.position;    
            ray.direction = shootPosition.forward;

            if(Physics.Raycast(ray, out hit)){
                Debug.DrawLine(ray.origin, hit.point, Color.red, 1f); 
            }

        }else{
            isFiring = false;
        }
    }
}
