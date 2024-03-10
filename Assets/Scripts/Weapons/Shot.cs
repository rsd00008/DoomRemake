using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public Transform spawnPoint; //donde spawnea la bala
    public GameObject bullet;
    public float bulletSpeed = 1500f; //velocidad de la bala
    public float shotRate = 0.5f; //cada cuanto podemos disparar

    private float shotRateTimeCounter = 0; //contador del tiempo que ha transcurrido para saber si han pasado shotRate segundos para que pueda volver a disparar

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Time.time > shotRateTimeCounter) //Time.time es el tiempo que ha pasado desde que hemos dado al play
            {
                GameObject newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation); //como vamos a instanciar muchas balas, hay que tener referencia de la bala para darle velocidad. Este gamObject es la instancia de la bala
                newBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * bulletSpeed);

                shotRateTimeCounter = Time.time + shotRate;

                Destroy(newBullet, 5); // a los 5 segundos, la bala desaparece
            }
        }

    }
}
