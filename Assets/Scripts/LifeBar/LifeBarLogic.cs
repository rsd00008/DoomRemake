using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeBarLogic : MonoBehaviour
{
    public float maxLife = 100;
    public float currentLife;
    public Image lifeBarImage;

    // Start is called before the first frame update
    void Start()
    {
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        lifeBarImage.fillAmount = currentLife / maxLife;

        if(currentLife <= 0)
        {
            if(gameObject.tag == "Player"){
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }else{
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            
        }   
    }

    IEnumerator DeathRoutine()
    {
        // Reproducir animación de muerte
        yield return new WaitForSeconds(1f); // Esperar a que la animación termine
        gameObject.SetActive(false); // Opcional: Desactivar primero
        Destroy(gameObject);
    }


    public void TakeDamage(float damage)
    {
        currentLife -= damage;
        currentLife = Mathf.Clamp(currentLife, 0, maxLife); // Asegura que la vida no sea negativa

        // Actualiza la barra de vida aquí si quieres que el cambio sea inmediato sin esperar al próximo Update
        lifeBarImage.fillAmount = currentLife / maxLife;
    }
}
