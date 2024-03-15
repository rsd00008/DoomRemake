using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivity = 80f;
    public Transform playerBody; // Referencia al cuerpo del jugador para ajustar su rotación
    private float xRotation = 0f;
    public Texture2D pointer; 

    public GameObject weapon; // Asumimos que tienes una referencia al GameObject del arma
    public Transform weaponPosition; // Cambia de GameObject a Transform para almacenar la posición y rotación
    public Transform aimingWeaponPosition; // Cambia de GameObject a Transform por lo mismo

    private bool isAiming = false; // Para controlar el estado de apuntado

    // Variables para el Lerp
    private float aimingSpeed = 5f; // Controla la velocidad de la transición
    private float currentLerpTime = 0f; // Tiempo actual desde que se inició el Lerp


    public Transform leftArm; 
    public Transform rightArm;


    public WeaponRaycast weaponScript; // Referencia al script de la clase RaycastWeapon



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
        playerBody.Rotate(Vector3.up * mouseX);

        leftArm.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        rightArm.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Detecta si el botón derecho del ratón está siendo presionado o soltado
        if (Input.GetMouseButtonDown(1)) // Botón derecho presionado
        {
            isAiming = true;
            currentLerpTime = 0f; // Reinicia el contador de Lerp
        }
        else if (Input.GetMouseButtonUp(1)) // Botón derecho soltado
        {
            isAiming = false;
            currentLerpTime = 0f; // Reinicia el contador de Lerp
        }

        // Realiza la transición suave del arma entre las posiciones
        if (isAiming)
        {
            currentLerpTime += Time.deltaTime * aimingSpeed;
            weapon.transform.position = Vector3.Lerp(weapon.transform.position, aimingWeaponPosition.position, currentLerpTime);
            weapon.transform.rotation = Quaternion.Lerp(weapon.transform.rotation, aimingWeaponPosition.rotation, currentLerpTime);
        }
        else
        {
            currentLerpTime += Time.deltaTime * aimingSpeed;
            weapon.transform.position = Vector3.Lerp(weapon.transform.position, weaponPosition.position, currentLerpTime);
            weapon.transform.rotation = Quaternion.Lerp(weapon.transform.rotation, weaponPosition.rotation, currentLerpTime);
        }
    }

    void OnGUI()
    {
        Rect rect = new Rect((Screen.width - pointer.width) / 2, (Screen.height - pointer.height) / 2, pointer.width, pointer.height);
        GUI.DrawTexture(rect, pointer);
    }
}
