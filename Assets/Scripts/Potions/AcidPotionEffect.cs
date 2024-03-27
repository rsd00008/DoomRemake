using UnityEngine;
using System.Collections;

public class AcidPotionEffect : MonoBehaviour
{
    public GameObject acidSpotPrefab;
    public GameObject acidSmokePrefab;
    public float scalingTime = 2.0f; // Duración de la animación de escalado
    public float acidPrefabsLifeTime = 15.0f; 

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Floor") || other.CompareTag("Wall")) {
            Vector3 position = transform.position + Vector3.down * 0.5f;

            Quaternion originalRotation = Quaternion.FromToRotation(Vector3.up, other.ClosestPointOnBounds(transform.position) - transform.position);
            Quaternion adjustedRotation = originalRotation * Quaternion.Euler(90f, 0f, 0f);

            if(other.CompareTag("Floor")){
                position += Vector3.up * 0.5f;
            }

            GameObject acidSpotInstance = Instantiate(acidSpotPrefab, position, adjustedRotation);
            GameObject acidSmokeInstance = Instantiate(acidSmokePrefab, position, adjustedRotation);

            // Comienza la corutina para escalar el acidSpotInstance
            StartCoroutine(ScaleOverTime(acidSpotInstance, new Vector3(0.25f,0.25f,0.25f), new Vector3(3.458957f, 3.10282f, 2.4687f), scalingTime));

            // Destruye el acidSpot y el acidSmoke después de 10 segundos
            Destroy(acidSpotInstance, acidPrefabsLifeTime);
            Destroy(acidSmokeInstance, acidPrefabsLifeTime);
        }
    }

    IEnumerator ScaleOverTime(GameObject targetObject, Vector3 initialScale, Vector3 finalScale, float duration) {
        float currentTime = 0.0f;

        while (currentTime < duration) {
            float t = currentTime / duration; // Normaliza el tiempo transcurrido

            targetObject.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            currentTime += Time.deltaTime;

            yield return null; // Espera un frame
        }

        // Asegura que el objeto tenga la escala final deseada
        targetObject.transform.localScale = finalScale;

        // Destruye el gameObject que contiene este script después de completar todo el proceso de escalado.
        // Nota: Como este Destroy se llama al final de la corutina, ocurre después de los 2 segundos de escalado, no inmediatamente.
        Destroy(gameObject);
    }
}
