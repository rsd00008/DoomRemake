using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public enum GameState {
    FreeRoam,
    Dialogue,
    Menu
}

public enum ItemShowed {
    Weapons,
    Potions
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public GameState gameState;
    private ItemShowed itemShowed = ItemShowed.Weapons;
    [SerializeField] private Camera fpsCamera;


    [Header("Player")]
    [SerializeField] private GameObject player;
    private PlayerMovement playerMovement;
    private LifeBarLogic playerLife;


    [Header("Weapons")]
    private int gunAmmoStored;
    private int gunAmmoLoaded;


    [Header("Scripts")]
    [SerializeField] private Weapon_switching weaponSwitching_script;
    [SerializeField] private Potion_switching potionSwitching_script;


    // GUI
    [Header("GUI")]
    [SerializeField] private TextMesh dialogPanel;
    private TextMeshProUGUI dialogPanelTMP;

    [SerializeField] private TextMesh interactionPanel;
    private TextMeshProUGUI interactionPanelTMP;

    [SerializeField] private TextMesh tasksPanel;
    private TextMeshProUGUI tasksPanelTMP;

    [SerializeField] private TextMeshProUGUI ammoPanelTMP;

    [SerializeField] private TextMeshProUGUI healPotionAmount_text;
    [SerializeField] private TextMeshProUGUI speedPotionAmount_text;
    [SerializeField] private TextMeshProUGUI acidPotionAmount_text;


    // ITEMS
    private int healPotionAmount = 0;
    private int speedPotionAmount = 0;
    private int acidPotionAmount = 0;


    void Awake() {
        if (instance == null) {
            instance = this;

        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        if (player != null){ 
            playerMovement = player.GetComponent<PlayerMovement>();
            playerLife = player.GetComponent<LifeBarLogic>();
        }

        UpdateItemShowed(ItemShowed.Weapons);

        if (dialogPanel != null){
            dialogPanel.gameObject.SetActive(false);
            
            Transform dialogTextTransform = dialogPanel.transform.Find("DialogText");

            if (dialogTextTransform != null)
            {
                dialogPanelTMP = dialogTextTransform.GetComponent<TextMeshProUGUI>();
            }
        }

        if (interactionPanel != null){
            interactionPanel.gameObject.SetActive(false);

            Transform interactionTextTransform = interactionPanel.transform.Find("InteractionText");

            if (interactionTextTransform != null)
            {
                interactionPanelTMP = interactionTextTransform.GetComponent<TextMeshProUGUI>();
            }
        }

        if (tasksPanel != null){
            Transform tasksTextTransform = tasksPanel.transform.Find("TasksText");

            if (tasksTextTransform != null)
            {
                tasksPanelTMP = tasksTextTransform.GetComponent<TextMeshProUGUI>();
            }
        }
    }

    public void dialogPanelUpdate(bool active, string text){
        if (dialogPanel != null){
            if(active != null){
                dialogPanel.gameObject.SetActive(active);
            }
            
            if(text != null){
                dialogPanelTMP.text = text;
            }
        }
    }

    public void interactionPanelUpdate(bool active, string text){
        if (interactionPanel != null){
            if(active != null){
                interactionPanel.gameObject.SetActive(active);
            }
            
            if(text != null){
                interactionPanelTMP.text = text;
            }
        }
    }

    public void tasksPanelUpdate(bool active, string text){
        if (tasksPanel != null){
            if(active != null){
                tasksPanel.gameObject.SetActive(active);
            }
            
            if(text != null){
                tasksPanelTMP.text = text;
            }
        }
    }

    public void updateAmmoText()
    {
        ammoPanelTMP.text = gunAmmoLoaded + "/" + gunAmmoStored;
    }

    public void setGunAmmo(int _ammoLoaded, int _ammoStored)
    {
        gunAmmoLoaded = _ammoLoaded;
        gunAmmoStored = _ammoStored;
    }

    public void updateGunAmmo(int ammo){
        if (ammo < 0){
            gunAmmoLoaded += ammo;

        }else{
            gunAmmoStored += ammo;
        }

        updateAmmoText();
    }

    public void reload(int ammoCapacity)
    {
        // Calculate the amount needed to fully reload the gun
        int neededAmmo = ammoCapacity - gunAmmoLoaded;

        if (neededAmmo <= 0) return; // If the gun is already full or overfull, no need to reload.

        if (gunAmmoStored > 0)
        {
            if (gunAmmoStored >= neededAmmo)
            {
                // If there's enough or more ammo stored than needed, load what's needed and reduce stored ammo
                gunAmmoLoaded += neededAmmo;
                gunAmmoStored -= neededAmmo;
            }
            else
            {
                // If there's not enough ammo stored to fully reload, load what's available and empty stored ammo
                gunAmmoLoaded += gunAmmoStored;
                gunAmmoStored = 0;
            }

            updateAmmoText();
        }
    }

    public bool playerIsSprinting(){
        return playerMovement.isSpriting();
    }   

    public void UpdatePlayerLife(float quantity){
        playerLife.UpdateLife(quantity);
    }

    public void takeItem(GameObject g){
        switch(g.name){
            case "HealPotion":
                healPotionAmount++;
                healPotionAmount_text.text = "X " + healPotionAmount.ToString();
                break;

            case "SpeedPotion":
                speedPotionAmount++;
                speedPotionAmount_text.text = "X " + speedPotionAmount.ToString();
                break;

            case "AcidPotion":
                acidPotionAmount++;
                acidPotionAmount_text.text = "X " + acidPotionAmount.ToString();
                break;

            default:
                Debug.Log("Item not recognized");
                break;
        }

        Destroy(g);
    }

    public void UsedItem(GameObject g){
        switch(g.name){
            case "HealPotion":
                healPotionAmount--;
                healPotionAmount_text.text = "X " + healPotionAmount.ToString();
                break;

            case "SpeedPotion":
                speedPotionAmount--;
                speedPotionAmount_text.text = "X " + speedPotionAmount.ToString();
                break;

            case "AcidPotion":
                acidPotionAmount--;
                acidPotionAmount_text.text = "X " + acidPotionAmount.ToString();
                break;

            default:
                Debug.Log("Item not recognized");
                break;
        }
    }

    public void UpdateGameState(GameState state) {
        gameState = state;

        switch (gameState) {
            // case GameState.FreeRoam:
            //     Cursor.lockState = CursorLockMode.Locked;
            //     Cursor.visible = false;
            //     break;

            // case GameState.Dialogue:
            //     Cursor.lockState = CursorLockMode.None;
            //     Cursor.visible = true;
            //     break;

            // case GameState.Menu:
            //     Cursor.lockState = CursorLockMode.None;
            //     Cursor.visible = true;
            //     break;
        }
    }

    public void UpdateItemShowed(ItemShowed item) {
        itemShowed = item;

        switch (itemShowed) {
            case ItemShowed.Weapons:
                potionSwitching_script.SelectPotion(false);
                weaponSwitching_script.SelectWeapon(true);

                break;

            case ItemShowed.Potions:
                weaponSwitching_script.SelectWeapon(false);
                potionSwitching_script.SelectPotion(true);
                
                break;
        }
    }


    public void ResetPlayerSpeed(float originalWalkSpeed, float originalRunSpeed, float delay)
    {
        StartCoroutine(ResetSpeedCoroutine(originalWalkSpeed, originalRunSpeed, delay));
    }

    private IEnumerator ResetSpeedCoroutine(float originalWalkSpeed, float originalRunSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);

        var playerMovement = getPlayerMovement();
        if (playerMovement != null)
        {
            playerMovement.walkSpeed = originalWalkSpeed;
            playerMovement.runSpeed = originalRunSpeed;
        }
    }


    //----- GETTERS -----
    public Camera getFpsCamera(){
        return fpsCamera;
    }

    public int getGunAmmoStored(){
        return gunAmmoStored;
    }

    public int getGunAmmoLoaded(){
        return gunAmmoLoaded;
    }

    public ItemShowed getItemShowed(){
        return itemShowed;
    }

    public int getHealPotionAmount(){
        return healPotionAmount;
    }

    public int getSpeedPotionAmount(){
        return speedPotionAmount;
    }   

    public int getAcidPotionAmount(){
        return acidPotionAmount;
    }

    public float getPlayerLife(){
        return playerLife.GetCurrentLife();
    }

    public float getMaxPlayerLife(){
        return playerLife.GetMaxLife();
    }

    public PlayerMovement getPlayerMovement(){
        return playerMovement;
    }
}
