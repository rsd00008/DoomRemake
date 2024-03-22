using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivity = 80f;
    public Transform playerBody;
    private float xRotation = 0f;
    public Texture2D pointer;

    public GameObject weapon;
    public Transform weaponPosition;
    public Transform aimingWeaponPosition;

    public float bobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    private float defaultPosX = 0;
    private float defaultPosY = 0;
    private float defaultPosZ = 0;
    private float timer = 0;

    private bool isAiming = false;

    private float aimingSpeed = 5f;
    private float currentLerpTime = 0f;

    public Transform leftArm;
    public Transform rightArm;

    private Vector2 moveInput;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        defaultPosX = weaponPosition.localPosition.x;
        defaultPosY = weaponPosition.localPosition.y;
        defaultPosZ = weaponPosition.localPosition.z;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        BobbingEffect();

        leftArm.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        rightArm.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (Input.GetMouseButtonDown(1) && GameManager.instance.getItemShowed() == ItemShowed.Weapons)
        {
            isAiming = true;
            currentLerpTime = 0f;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            currentLerpTime = 0f;
        }

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

    // COMPLEX BOBBING
    private void BobbingEffect()
    {
        bool isSprinting = GameManager.instance.playerIsSprinting(); // Assume GameManager has a method playerIsSprinting that returns a bool

        float currentBobbingSpeed = isSprinting ? bobbingSpeed * 2f : bobbingSpeed;
        float currentBobbingAmountY = isSprinting ? bobbingAmount * 2f : bobbingAmount;
        float currentBobbingAmountX = currentBobbingAmountY * 0.5f; // Side-to-side is typically less pronounced than up-and-down
        float currentBobbingAmountZ = currentBobbingAmountY * 0.3f; // Forward-and-back is subtle

        if (Mathf.Abs(moveInput.x) > 0.1f || Mathf.Abs(moveInput.y) > 0.1f) // Player is moving
        {
            timer += Time.deltaTime * currentBobbingSpeed;
            float waveSliceX = Mathf.Sin(timer * 0.5f); // Side-to-side should have a different phase
            float waveSliceY = Mathf.Sin(timer);
            float waveSliceZ = Mathf.Cos(timer * 0.2f); // Forward-and-back can be on a different phase or frequency

            float totalMovement = Mathf.Clamp01(Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y));
            
            // Offset by waveSlice and totalMovement
            float offsetX = waveSliceX * currentBobbingAmountX * totalMovement;
            float offsetY = waveSliceY * currentBobbingAmountY * totalMovement;
            float offsetZ = waveSliceZ * currentBobbingAmountZ * totalMovement;

            // Combine default position with the offsets
            Vector3 bobbingPosition = new Vector3(defaultPosX + offsetX, defaultPosY + offsetY, defaultPosZ + offsetZ);
            weaponPosition.localPosition = Vector3.Lerp(weaponPosition.localPosition, bobbingPosition, Time.deltaTime * currentBobbingSpeed);
        }
        else
        {
            timer = 0;
            // Smoothly return to the starting position
            Vector3 startPosition = new Vector3(defaultPosX, defaultPosY, defaultPosZ);
            weaponPosition.localPosition = Vector3.Lerp(weaponPosition.localPosition, startPosition, Time.deltaTime * currentBobbingSpeed);
        }
    }


    // EASY BOBBING
    // private void BobbingEffect()
    // {
    //     bool isSprinting = GameManager.instance.playerIsSprinting(); // Assume GameManager has a method playerIsSprinting that returns a bool

    //     float currentBobbingSpeed = isSprinting ? bobbingSpeed * 1.5f : bobbingSpeed;
    //     float currentBobbingAmount = isSprinting ? bobbingAmount * 1.5f : bobbingAmount;

    //     if (Mathf.Abs(moveInput.x) > 0.1f || Mathf.Abs(moveInput.y) > 0.1f) // Player is moving
    //     {
    //         timer += Time.deltaTime * currentBobbingSpeed;
    //         float waveSlice = Mathf.Sin(timer);
    //         float offsetY = waveSlice * currentBobbingAmount;
    //         float totalAxes = Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y);
    //         totalAxes = Mathf.Clamp(totalAxes, 0, 1);
    //         offsetY *= totalAxes;

    //         weaponPosition.localPosition = new Vector3(weaponPosition.localPosition.x, defaultPosY + offsetY, weaponPosition.localPosition.z);
    //     }
    //     else
    //     {
    //         timer = 0;
    //         weaponPosition.localPosition = new Vector3(weaponPosition.localPosition.x, Mathf.Lerp(weaponPosition.localPosition.y, defaultPosY, Time.deltaTime * currentBobbingSpeed), weaponPosition.localPosition.z);
    //     }
    // }

    void OnGUI()
    {
        Rect rect = new Rect((Screen.width - pointer.width) / 2, (Screen.height - pointer.height) / 2, pointer.width, pointer.height);
        GUI.DrawTexture(rect, pointer);
    }
}
