using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KyleRobotLogic : MonoBehaviour, IAction
{
    public GameObject player; // Referencia al jugador
    private PlayerMovement playerMovementScript; // Variable para almacenar el script de movimiento del jugador
    public float rotationSpeed = 5f; // Velocidad de rotación ajustable desde el Inspector


    public GameObject gun; // Referencia al arma del jugador
    private WeaponRayCasting gunShotScript; // Variable para almacenar el script de movimiento del jugador


    public GameObject healPotion; // Referencia a la poción de curación 
    public GameObject speedPotion; // Referencia a la poción de velocidad
    public GameObject acidPotion; // Referencia a la poción de ácido    

    
    //DIALOGS
    public GameObject taskIcon; // Objecto que se situara en la cabeza de Kyle cuando tenga una tarea para nosotros
    private TaskIconMovement taskIconScript; // Referencia al script de movimiento del icono de tarea    

    //DIALOGS CONTROL
    private int dialogStep = 1; // Controla el paso actual del diálogo, empezamos con el primero
    private float timeSinceDialogStart = 0f; // Tiempo desde que comenzó el último diálogo
    private bool canProceedToNextDialog = false; // Si el jugador puede proceder al siguiente diálogo
    public bool kyleIsTalking = false; // variable que nos dira si Kyle esta hablando
    public bool canTalkToKyle = true; // variable que nos dira si podemos hablar con Kyle
    public float timeToStepDialog = 3f; // Tiempo que tardara en pasar al siguiente paso del dialogo    


    //TUTORIAL
    public GameObject banana_man; // Banana-man para practicar
    public List<GameObject> enemies; // Array de objetos que se activaran cuando interactuemos con Kyle, los enemigos


    //WAITING FOR PLAYER ACTIONS
    private bool waitingForJumping = false; // Si estamos esperando el salto del jugador
    private bool waitingForSprinting = false; // Si estamos esperando el sprint del jugador
    private bool waitingForShooting = false; // Si estamos esperando el disparo del jugador
    private bool waitingForKillingEnemies = false; // Si estamos esperando que el jugador mate a los enemigos


    public void Activate()
    {
        if(canTalkToKyle == true){
            playerMovementScript = player.GetComponent<PlayerMovement>();
            gunShotScript = gun.GetComponent<WeaponRayCasting>();
            taskIconScript = taskIcon.GetComponent<TaskIconMovement>();

            if (kyleIsTalking == false){ // Si Kyle no esta hablando, antes de que empiece a hablar, preparamos todo
                StartDialog();
                LookAtKyleOnce();

            }else { // Si Kyle no ha dejado de hablar, comprobamos si podemos continuar con el dialogo
                if (canProceedToNextDialog == true){
                    ContinueDialog();
                }
            }
        }
    }   

    private void StartDialog()
    {
        taskIcon.SetActive(false); // desactivamos el icono de tarea (diamante)
        taskIconScript.enabled = false;   // desactivamos el script de movimiento del icono de tarea (diamante)

        GameManager.instance.tasksPanelUpdate(true, "");
        GameManager.instance.dialogPanelUpdate(true, null);
        GameManager.instance.interactionPanelUpdate(false, null);
        
        playerMovementScript.setIsTalking(true);
        playerMovementScript.Move();
        playerMovementScript.enabled = false; // Desactiva el movimiento del jugador
        
        Camera.main.GetComponent<CameraLook>().enabled = false; // Desactiva el control de la cámara
        gunShotScript.enabled = false; // Desactiva por disparar

        updateTaskConditions(); // Actualiza las condiciones de la tarea en funcion de la etapa del dialogo
        
        timeSinceDialogStart = 0f; // Reinicia el contador de tiempo
        canProceedToNextDialog = false; // Aún no puede proceder al siguiente diálogo

        kyleIsTalking = true; // ya todo esta preparado, kyle va a empezar a hablar 
    }

    private void updateTaskConditions(){
        if(dialogStep <= 6){
            playerMovementScript.hasJumped = false; // Reinicia el estado del salto del jugador
        }

        if(dialogStep <= 7){
            playerMovementScript.hasSprinted = false; // Reinicia el estado de sprint del jugador
        }

        if(dialogStep <= 9){
            gunShotScript.hasShooted = false; // Reinicia el estado de disparo del jugador
        }
    }

    private void ContinueDialog()
    {
        dialogStep++; // Avanza al siguiente paso del diálogo
        timeSinceDialogStart = 0f; // Reinicia el contador de tiempo
        canProceedToNextDialog = false; // Aún no puede proceder al siguiente diálogo

        switch (dialogStep)
        {
            case 1:
                ShowDialog("Kyle: WOW! A survivor!!!");
                break;

            case 2:
                ShowDialog("Kyle: I have been here for hundreds of years...");
                break;

            case 3:
                ShowDialog("Kyle: The StarLord doesn't let me free...");
                break;

            case 4:
                ShowDialog("Kyle: Please, help me to get rid of him!");
                break;

            case 5:
                ShowDialog("Kyle: To do so, I will teach you how to fight...");
                break;

            case 6:
                ShowDialog("Kyle: First of all, you can jump by pressing \"space\" ");
                break;

            case 7:
                ShowDialog("Kyle: You are awesome! Now, let's try to run, press \"shift\" while moving");
                break;

            case 8:
                ShowDialog("Kyle: Well done! Now, let's try to shoot");
                break;

            case 9:
                ShowDialog("Kyle: Press \"left click\" and kill this BANANA-MAN!");
                break;

            case 10:
                ShowDialog("Kyle: Nice! Now you are ready to fight and help me to get rid of the StarLord !");
                break;

            case 11:
                ShowDialog("Kyle: Oh! some enemies appeared! kill them!");
                break;

            case 12:
                ShowDialog("Kyle: Thanks for your help! Oh I almost forget...");
                break;

            case 13: 
                ShowDialog("Kyle: There are potions which you can take and get some powers...");
                break;

            case 14:
                ShowDialog("Kyle: There are three different potions, each one will give you a different power...");
                break;

            case 15:
                ShowDialog("Kyle: The red one will heal you ");
                break;

            case 16: 
                ShowDialog("Kyle: The blue one will make you run faster");
                break;

            case 17: 
                ShowDialog("Kyle: There is a green potion which you can throw and create a acid pool");
                break;

            case 18:
                ShowDialog("Kyle: Select the potion you want to use by pressing the numbers 1, 2 or 3 and then press \"left click\" to use it");
                break;

            case 19:
                ShowDialog("Kyle: You helped me a lot, so you deserve a reward !");
                break;

            case 20:
                ShowDialog("Kyle: I will give you potions to help you in your journey !");
                break;

            case 21:
                ShowDialog("Kyle: Now you are ready to fight! get throught the ship to find and kill the StarLord");
                break;

            case 22:
                ShowDialog("Kyle: Good luck !");
                break;

            default:
                EndDialog();
                break;
        }
    }
    
    private void EndDialog()
    {
        GameManager.instance.dialogPanelUpdate(false, null);
        kyleIsTalking = false;
        
        playerMovementScript.setIsTalking(false);
        playerMovementScript.enabled = true; // Reactiva el movimiento
        Camera.main.GetComponent<CameraLook>().enabled = true; // Reactiva el control de la cámara
        gunShotScript.enabled = true; // Activa por disparar
    }

    void Update(){
        if(dialogStep < 22){
            checkIfConditionHasDone();
            checkIfNewTask();
        }

        if(kyleIsTalking == true){
            GameManager.instance.tasksPanelUpdate(true, "");
            timeSinceDialogStart += Time.deltaTime;

            if (timeSinceDialogStart >= timeToStepDialog){
                GameManager.instance.interactionPanelUpdate(true, null);
                canProceedToNextDialog = true;

            }else{
                GameManager.instance.interactionPanelUpdate(false, null);
            }

            Vector3 posicionKyle = transform.position;
            posicionKyle.y = player.transform.position.y;
        }
    }

    private void checkIfConditionHasDone(){
        bool done = false;

        if (waitingForJumping == true && playerHasJumped() == true) // Si estamos esperando el salto y el jugador ha saltado   
        {
            waitingForJumping = false;
            done = true;
        }

        if (waitingForSprinting == true && playerHasSprinted() == true) // Si estamos esperando el sprint y el jugador ha sprintado   
        {
            waitingForSprinting = false;
            done = true;
        }

        if (waitingForShooting == true && playerHasShooted() == true) // Si estamos esperando el sprint y el jugador ha sprintado   
        {
            waitingForShooting = false;
            done = true;
        }

        if (waitingForKillingEnemies == true && enemies.Count == 0) // Si estamos esperando el sprint y el jugador ha sprintado   
        {
            waitingForKillingEnemies = false;
            done = true;
        }

        if(done == true){
            GameManager.instance.tasksPanelUpdate(true, "Talk to Kyle");

            taskIconScript.enabled = true; // Reactiva el icono de tarea
            taskIcon.SetActive(true);  

            canTalkToKyle = true; // Ahora podemos hablar con Kyle  
            kyleIsTalking = false;       
        }
    }

    private void checkIfNewTask(){
        bool done = false;

        if(dialogStep == 7 && playerHasJumped() == false){
            done = true;
            waitingForJumping = true;
            GameManager.instance.tasksPanelUpdate(true, "Press SPACE to jump");
        }

        if(dialogStep == 8 && playerHasSprinted() == false){
            done = true;
            waitingForSprinting = true;
            GameManager.instance.tasksPanelUpdate(true, "Press SHIFT while moving to sprint");
        }

        if(dialogStep == 10 && playerHasShooted() == false){
            done = true;
            waitingForShooting = true;

            banana_man.SetActive(true);

            GameManager.instance.tasksPanelUpdate(true, "Press LEFT CLICK to shoot");
        }

        if(enemies != null){
            if(dialogStep == 12 && enemies.Count != 0){
                done = true;
                waitingForKillingEnemies = true;

                for(int i=0;i<enemies.Count;i++){  
                    if(enemies[i] == null){
                        enemies.RemoveAt(i);
                        i--;

                    }else{
                        enemies[i].SetActive(true);
                    }
                }   

                GameManager.instance.tasksPanelUpdate(true, "Kill all the enemies");
            }
        }

        if(dialogStep == 21){
            healPotion.SetActive(true);
            speedPotion.SetActive(true);
            acidPotion.SetActive(true);
        }

        if(done == true){
            GameManager.instance.dialogPanelUpdate(false, null);
            kyleIsTalking = false;

            canTalkToKyle = false; // Ahora no podemos hablar con Kyle

            GameManager.instance.interactionPanelUpdate(false, null);

            playerMovementScript.setIsTalking(false);
            playerMovementScript.enabled = true; // Reactiva el movimiento
            Camera.main.GetComponent<CameraLook>().enabled = true; // Reactiva el control de la cámara
            gunShotScript.enabled = true; // Activa por disparar
        }
    }

    private void LookAtKyleOnce()
    {
        Vector3 direccion = (transform.position - player.transform.position);
        direccion.y = 0; // Opcional: eliminar para incluir la inclinación arriba/abajo en la rotación.
        Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
        player.transform.rotation = rotacionDeseada; // Aplica la rotación directamente sin Slerp para un cambio inmediato.
    }

    private void ShowDialog(string message)
    {
        GameManager.instance.dialogPanelUpdate(true, message);
    }

    private bool playerHasJumped()
    {
        return playerMovementScript.hasJumped;
    }

    private bool playerHasSprinted()
    {
        return playerMovementScript.hasSprinted;
    }

    private bool playerHasShooted()
    {
        return gunShotScript.hasShooted;
    }
}
