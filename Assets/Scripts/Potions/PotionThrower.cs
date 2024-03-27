using System.Collections;
using UnityEngine;

public class PotionThrower : MonoBehaviour
{
    public GameObject acidPotionPrefab; 
    public Camera cam;
    public Transform throwPoint;
    private LineRenderer trajectoryLine;

    public float throwForce = 10f; 
    public float maxForce = 20f;

    public Vector3 throwDirection = new Vector3(0,1,0);

    private bool isCharging = false;
    private float chargeTime = 0f;

    // Nuevas variables para el efecto pulsante del LineRenderer
    public float minWidth = 0.1f; // Ancho mínimo del LineRenderer
    public float maxWidth = 0.5f; // Ancho máximo del LineRenderer
    public float pulseSpeed = 2f; // Velocidad del pulso

    private void Start() {
        throwDirection = new Vector3(0,1,0);
        trajectoryLine = GetComponent<LineRenderer>();
        StartCoroutine(PulseLine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && GameManager.instance.getAcidPotionAmount() > 0)
        {
            StartThrowing();
        }

        if(isCharging == true)
        {
            ChargeThrow();
        }

        if(Input.GetKeyUp(KeyCode.Q) && GameManager.instance.getAcidPotionAmount() > 0)
        {
            ReleaseThrow();

            GameManager.instance.UsedItem(acidPotionPrefab);

            if(GameManager.instance.getAcidPotionAmount() == 0){
                GameManager.instance.UpdateItemShowed(ItemShowed.Weapons);
            }
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
        ThrowPotion( Mathf.Min(chargeTime * throwForce * 1.5f, maxForce * 1.5f) );
        isCharging = false;

        trajectoryLine.enabled = false;
    }

    private void ThrowPotion(float force){
        if (acidPotionPrefab != null)
        {
            Vector3 spawnPosition = throwPoint.position + cam.transform.forward;

            GameObject potion = Instantiate(acidPotionPrefab, spawnPosition, cam.transform.rotation);

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

    // Corutina para ajustar el grosor del LineRenderer en un ciclo continuo
    private IEnumerator PulseLine() {
        float timer = 0f;
        while (true) {
            timer += Time.deltaTime * pulseSpeed;
            float width = Mathf.Lerp(minWidth, maxWidth, (Mathf.Sin(timer) + 1) / 2);
            trajectoryLine.widthMultiplier = width;

            yield return null;
        }
    }
}
