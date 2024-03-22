using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class WeaponRayCasting : MonoBehaviour
{
    public class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer trail;
        public float distanceTraveled = 0f;
    }

    // SHOT PARAMS
    private List<Bullet> bullets = new List<Bullet>();  

    [SerializeField] private Transform shootPosition;
    [SerializeField] private float range = 15f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float fireRate = 15f;
    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private float bulletDrop = 0.0f;
    [SerializeField] private float bulletLife = 3.0f;
    [SerializeField] private int ammoCapacity = 15;
    
    [SerializeField] private ParticleSystem[] muzzleFlash;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private TrailRenderer laserTrail;
    [SerializeField] private Transform triggerTransform;




    [SerializeField] private Camera cam;


    private float timeElapsed = 0f;
    public bool hasShooted = false;

    private void Start()
    {
        timeElapsed = 0f;
        hasShooted = false;

        GameManager.instance.setGunAmmo(ammoCapacity, 40);
        GameManager.instance.updateAmmoText();
    }

    private Vector3 GetBulletPosition(Bullet b)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return b.initialPosition + (b.initialVelocity * b.time) + (0.5f * gravity * b.time * b.time);
    }

    private Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet b = new Bullet
        {
            initialPosition = position,
            initialVelocity = velocity,
            time = 0,
            trail = Instantiate(laserTrail, position, Quaternion.identity)
        };

        b.trail.AddPosition(position);

        return b;
    }

    public void UpdateBullets(float deltaTime)
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            Bullet bullet = bullets[i];
            
            Vector3 previousPosition = GetBulletPosition(bullet); // Get the position before updating time
            bullet.time += deltaTime;
            Vector3 currentPosition = GetBulletPosition(bullet); // Get the updated position

            // Calculate the distance the bullet moved in this update
            float distanceMoved = Vector3.Distance(previousPosition, currentPosition);
            bullet.distanceTraveled += distanceMoved;

            if (bullet.distanceTraveled > range) // Check if the bullet has exceeded its range
            {
                Destroy(bullet.trail.gameObject);
                bullets.RemoveAt(i);
                continue; // Skip the rest of the loop for this bullet
            }

            Ray ray = new Ray(previousPosition, currentPosition - previousPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distanceMoved))
            {
                HandleBulletHit(hit, bullet);
                bullet.time = bulletLife;
            }
            else
            {
                bullet.trail.transform.position = currentPosition;
                bullet.trail.AddPosition(currentPosition);
            }

            if (bullet.time >= bulletLife)
            {
                Destroy(bullet.trail.gameObject);
                bullets.RemoveAt(i);
            }
        }
    }

    private void HandleBulletHit(RaycastHit hit, Bullet bullet)
    {
        hitEffect.transform.position = hit.point;
        hitEffect.transform.forward = hit.normal;
        hitEffect.Emit(1);
        bullet.trail.transform.position = hit.point;
        if (hit.collider.gameObject.CompareTag("Enemy"))
        {
            LifeBarLogic enemyLife = hit.collider.GetComponent<LifeBarLogic>();
            enemyLife?.TakeDamage(damage);
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;  
        UpdateBullets(Time.deltaTime);

        if (Input.GetButtonDown("Fire1") && timeElapsed >= 1f / fireRate && GameManager.instance.getGunAmmoLoaded() > 0 && GameManager.instance.getItemShowed() == ItemShowed.Weapons)
        {
            timeElapsed = 0f;
            hasShooted = true;

            Vector3 velocity = cam.transform.forward * bulletSpeed;
            Bullet b = CreateBullet(shootPosition.position, velocity);
            bullets.Add(b);

            foreach (ParticleSystem muzzle in muzzleFlash)
            {
                muzzle.Emit(1);
            }

            GameManager.instance.updateGunAmmo(-1);
            StartCoroutine(AnimateTrigger()); // Add this line to start the trigger animation
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.instance.reload(ammoCapacity);
        }
    }

    private IEnumerator AnimateTrigger()
    {
        if (triggerTransform == null)
            yield break;

        // Move the trigger back
        float time = 0.1f; // Duration of the trigger pull animation
        Vector3 originalPosition = triggerTransform.localPosition;
        Vector3 retractedPosition = originalPosition + triggerTransform.localRotation * new Vector3(0.01f, 0f, 0f); // Adjust as needed
        
        while (time > 0f)
        {
            triggerTransform.localPosition = Vector3.Lerp(retractedPosition, originalPosition, time / 0.05f);
            time -= Time.deltaTime;
            yield return null;
        }

        // Reset the position
        triggerTransform.localPosition = originalPosition;
    }

}
