using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivity = 80f;
    public Transform playerHead;
    private float xRotation = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation = xRotation - mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); //eje Y realmente
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //yRotation = yRotation - mouseX;
        //yRotation = Mathf.Clamp(yRotation, -80f, 80f); //eje Y realmente
        //transform.localRotation = Quaternion.Euler(0, yRotation, 0);

        playerHead.Rotate(Vector3.up * mouseX);
    }
}
