using UnityEngine;
using System.Collections;

public class SpeedPotionEffect : MonoBehaviour
{
    public float speedMultiplier = 2f; // Multiplicador de la velocidad
    public float effectDuration = 2f; // Duración del efecto de la poción en segundos

    private bool isEffectActive = false;
    private float originalWalkSpeed;
    private float originalRunSpeed;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameManager.instance.getPlayerMovement();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && GameManager.instance.getSpeedPotionAmount() > 0 && !isEffectActive){
            GameManager.instance.UsedItem(gameObject);

            originalWalkSpeed = playerMovement.walkSpeed;
            originalRunSpeed = playerMovement.runSpeed;

            playerMovement.walkSpeed *= speedMultiplier;
            playerMovement.runSpeed *= speedMultiplier;

            isEffectActive = true;

            // Usa el GameManager para manejar la restauración de la velocidad
            GameManager.instance.ResetPlayerSpeed(originalWalkSpeed, originalRunSpeed, effectDuration);

            if(GameManager.instance.getSpeedPotionAmount() == 0){
                GameManager.instance.UpdateItemShowed(ItemShowed.Weapons);
            }
        }
    }
}
