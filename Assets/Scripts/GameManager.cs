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

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public GameState gameState;

    [Header("Player")]
    public GameObject player;

    // GUI
    [Header("GUI")]
    public TextMesh dialogPanel;
    public TextMesh interactionPanel;
    public TextMesh tasksPanel;
    public TextMeshProUGUI healPotionAmount_text;
    public TextMeshProUGUI speedPotionAmount_text;
    public TextMeshProUGUI acidPotionAmount_text;
    


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
        if (dialogPanel != null)
            dialogPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void dialogPanelUpdate(bool active, string text){
        if (dialogPanel != null){
            if(active != null){
                dialogPanel.gameObject.SetActive(active);
            }
            
            if(text != null){
                dialogPanel.text = text;
            }
        }
    }

    public void interactionPanelUpdate(bool active, string text){
        if (interactionPanel != null){
            if(active != null){
                interactionPanel.gameObject.SetActive(active);
            }
            
            if(text != null){
                interactionPanel.text = text;
            }
        }
    }

    public void tasksPanelUpdate(bool active, string text){
        if (tasksPanel != null){
            if(active != null){
                tasksPanel.gameObject.SetActive(active);
            }
            
            if(text != null){
                tasksPanel.text = text;
            }
        }
    }

    public void takeItem(GameObject g){
        Debug.Log("Item taken: " + g.name);

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
}
