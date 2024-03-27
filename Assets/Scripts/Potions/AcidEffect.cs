using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidEffect : MonoBehaviour
{
    [SerializeField] private float damage = 10.0f;  

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            GameManager.instance.UpdatePlayerLife(-damage * Time.deltaTime);
        
        }else if(other.CompareTag("Enemy")){
            other.GetComponent<LifeBarLogic>().UpdateLife(-damage * Time.deltaTime);
        }
    }
}
