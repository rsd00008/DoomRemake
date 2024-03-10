using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckBoundaries : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();

            if (controller != null)
            {
                // Desactiva temporalmente el Character Controller para moverlo
                controller.enabled = false;

                other.transform.position = new Vector3(-53, 25.28f, 0);

                // Reactiva el Character Controller después de moverlo
                controller.enabled = true;
            }
            else
            {
                // Si no hay Character Controller, mueve el transform directamente
                other.transform.position = new Vector3(-53, 25.28f, 0);
            }
        }
    }

}
