using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 1.9f;
    public float outsideGravityScale = -15f;
    public float insideGravityScale = -15f;
    private float gravityScale;

    // private Animator animatorController;

    public TextMeshProUGUI taskText;
    public bool hasJumped = false; //para detectar si el jugador ha saltado
    public bool hasSprinted = false; //para detectar si el jugador ha sprintado
    

    public Transform cameraTransform; // Agrega una referencia a la Transform de la cámara

    Vector3 moveInput = Vector3.zero;
    CharacterController characterController;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        // animatorController = GetComponent<Animator>();  
    }

    private void Update()
    {
        Shader.SetGlobalVector("ObjectPosition", new Vector4(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z, this.transform.localScale.x));

        if(transform.position.x >= -5){
            gravityScale = insideGravityScale;

        }else{
            gravityScale = outsideGravityScale;
        }
        
        Move();
    }

    private void Move()
    {
        if (characterController.isGrounded)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            // animatorController.SetFloat("VelX", x);
            // animatorController.SetFloat("VelZ", z);

            // Utiliza la orientación de la cámara para calcular la dirección del movimiento
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            moveInput = forward * z + right * x;
            moveInput = Vector3.ClampMagnitude(moveInput, 1f); // Limita el movimiento diagonal
            
            if (Input.GetButton("Sprint")){
                moveInput *= runSpeed;
                hasSprinted = true; // Indica que el jugador ha sprintado

                // animatorController.SetBool("isRunning", true);
           
            }else{
                moveInput *= walkSpeed;
            }

            moveInput.y = 0; // Prepara el componente y para el salto o la gravedad

            if (Input.GetButtonDown("Jump")){
                moveInput.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale); // Aplica impulso del salto
                hasJumped = true; // Indica que el jugador ha saltado

                // animatorController.SetBool("isJumping", true);
            }
            
        }else{
            // animatorController.SetBool("isJumping", false);
        }

        moveInput.y += gravityScale * Time.deltaTime; // Aplica gravedad constantemente

        characterController.Move(moveInput * Time.deltaTime); // Mueve el personaje
    }
}