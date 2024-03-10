using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowsPlayer : MonoBehaviour
{
    public float damage = 20f;
    public float detectionRadius = 10f;
    public Transform player;
    private Animator anim;
    public float movementVelocity = 5f;
    public LayerMask layerToDetect;
    private bool playerDetected = false;
    public float stopDistance = 2f; // Distancia mínima antes de detenerse. Define qué tan cerca puede llegar el enemigo al jugador antes de detenerse. Esto evita que el enemigo entre en el collider del jugador, manteniendo una distancia mínima entre ellos.

    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<Animator>();    
    }

    void Update()
    {
        playerDetected = Physics.CheckSphere(transform.position, detectionRadius, layerToDetect);  
        
        if(playerDetected == true){
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            // Calcula la distancia entre el enemigo y el jugador
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Se mueve hacia el jugador solo si la distancia es mayor que stopDistance
            if (distanceToPlayer > stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, movementVelocity * Time.deltaTime);
            }
        }
    }

    // Update is called once per frame
    // void Update()
    // {
    //     //Para que el enemigo pueda detectar la presencia del jugador
    //     playerDetected = Physics.CheckSphere(transform.position, detectionRadius, layerToDetect);  
        
    //     if(playerDetected == true){
    //         // anim.SetBool("isIdle", false);

    //         transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    //         transform.position = Vector3.MoveTowards(transform.position, player.position, movementVelocity * Time.deltaTime);
    //         // transform.Translate(Vector3.forward * 5 * Time.deltaTime);

    //     }else{
    //         // anim.SetBool("isIdle", true);
    //     }
    // }

    //ayuda a visualizar cosas en el editor
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Collision with: "+other.gameObject.name+"("+other.gameObject.tag+")");
        
        if(other.gameObject.tag == "Player"){
            Debug.Log("Player hit");
            LifeBarLogic playerLife = other.GetComponent<LifeBarLogic>();
                    
            if (playerLife != null)
            {
                playerLife.TakeDamage(damage);
            }
        }
    }
}
