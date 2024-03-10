using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 1.9f;
    public float outsideGravityScale = -15f;
    public float insideGravityScale = -15f;
    private float gravityScale;
    

    public Transform cameraTransform; // Agrega una referencia a la Transform de la cámara

    Vector3 moveInput = Vector3.zero;
    CharacterController characterController;

    private Vector3 forwardProjected;
    private Vector3 rightProjected;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
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
        if (characterController.isGrounded) //si está en el suelo
        {
            float x = -Input.GetAxis("Horizontal");
            float z = -Input.GetAxis("Vertical");

            moveInput = transform.TransformDirection(cameraTransform.right * x + cameraTransform.forward * z);
            moveInput.y = 0; // Ignora el movimiento en el eje Y para no afectar el salto/gravedad

            moveInput = Vector3.ClampMagnitude(moveInput, 1f); //para limitar y asegurarse de que el movimiento diagonal no exceda velocidad

            if (Input.GetButton("Sprint")) //si se mantiene presionado el boton sprint
            {
                moveInput = transform.TransformDirection(moveInput) * runSpeed;
            }
            else
            {
                moveInput = transform.TransformDirection(moveInput) * walkSpeed;
            }

            if (Input.GetButtonDown("Jump")) //si se mantiene presionado el boton space
            {
                moveInput.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);
            }
        }

        moveInput.y += gravityScale * Time.deltaTime;
        characterController.Move(moveInput * Time.deltaTime);
    }
}