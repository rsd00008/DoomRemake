using UnityEngine;

public class AcidPotionEffect : MonoBehaviour
{
    public GameObject acidSpotPrefab;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Floor") || other.CompareTag("Wall")) {
            Vector3 position = transform.position + Vector3.down * 0.5f; // Ajusta este valor según sea necesario

            // La rotación original es calculada respecto a la normal de la colisión
            Quaternion originalRotation = Quaternion.FromToRotation(Vector3.up, other.ClosestPointOnBounds(transform.position) - transform.position);

            // Agrega una rotación de 90 grados en el eje X a la rotación original
            Quaternion adjustedRotation = originalRotation * Quaternion.Euler(90f, 0f, 0f);

            if(other.CompareTag("Floor")){
                // Ajusta la posición 0.4 unidades hacia arriba en el eje Y para prevenir el solapamiento y elevar el efecto.
                position += Vector3.up * 0.5f; // Aquí haces el ajuste para elevar el acidSpotPrefab
            }
            
            // Instancia el acid spot en la posición ajustada, con la rotación ajustada.
            Instantiate(acidSpotPrefab, position, adjustedRotation);

            // Destruye el objeto potion para simular que la poción se ha usado/consumido.
            Destroy(gameObject);
        }
    }


}
