using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivity = 80f;
    public Transform playerBody; // Referencia al cuerpo del jugador para ajustar su rotación
    private float xRotation = 0f;
    public Texture2D pointer; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX); // Rota el cuerpo del jugador en el eje Y basado en el movimiento del ratón.
    }

    void OnGUI()
    {
        Rect rect = new Rect(Screen.width/2, Screen.height/2, pointer.width, pointer.height);
        GUI.DrawTexture(rect, pointer);
    }
}
