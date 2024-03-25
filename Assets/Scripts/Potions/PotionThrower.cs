using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionThrower : MonoBehaviour{
    public GameObject acidPotionPrefab; 
    public Camera cam;
    public Transform throwPoint;
    private LineRenderer trajectoryLine;

    public float throwForce = 10f; 
    public float maxForce = 20f;

    public Vector3 throwDirection = new Vector3(0,1,0);

    private bool isCharging = false;
    private float chargeTime = 0f;

    private void Start() {
        Debug.Log("Press Q to throw potion");
        throwDirection = new Vector3(0,1,0);
        trajectoryLine = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Throwing potion");
            StartThrowing();
        }

        if(isCharging == true)
        {
            ChargeThrow();
        }

        if(Input.GetKeyUp(KeyCode.Q))
        {
            ReleaseThrow();
        }
    }

    private void StartThrowing(){
        isCharging = true;
        chargeTime = 0f;

        trajectoryLine.enabled = true;
    }

    private void ChargeThrow(){
        chargeTime += Time.deltaTime;

        Vector3 newThrowDirection = (cam.transform.forward + throwDirection).normalized;
        Vector3 potionVelocity = newThrowDirection * Mathf.Min(chargeTime * throwForce, maxForce);
        
        ShowTrajectory(throwPoint.position + throwPoint.forward, potionVelocity);    
    }

    private void ReleaseThrow(){
        ThrowPotion( Mathf.Min(chargeTime * throwForce, maxForce) );
        isCharging = false;

        trajectoryLine.enabled = false;
    }

    private void ThrowPotion(float force){
        if (acidPotionPrefab != null)
        {
            Vector3 spawnPosition = throwPoint.position + cam.transform.forward;

            GameObject potion = Instantiate(acidPotionPrefab, spawnPosition, cam.transform.rotation);

            // Get the Rigidbody component and apply force to it to throw the potion
            Rigidbody rb = potion.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 newThrowDirection = (cam.transform.forward + throwDirection).normalized;
                rb.AddForce(newThrowDirection * force, ForceMode.VelocityChange);
            }
        }
    }

    private void ShowTrajectory(Vector3 origin, Vector3 speed){
        Vector3[] points = new Vector3[100];
        trajectoryLine.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = origin + speed * time + 0.2f * Physics.gravity * time * time;
        }

        trajectoryLine.SetPositions(points);
    }   
}
