using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class NPCInteraction : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    public float interactDistance = 5f;

    void Start()
    {
        GameManager.instance.interactionPanelUpdate(false, null);
    }

    void Update()
    {
        // Remove the previous highlight
        if (highlight != null)
        {
            highlight = null;
            GameManager.instance.interactionPanelUpdate(false, null);
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        // Detect NPC
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit, interactDistance))
        {
            Transform currentHighlight = raycastHit.transform;

            if (currentHighlight.CompareTag("NPC"))
            {
                highlight = currentHighlight; // Set the new highlight
                GameManager.instance.interactionPanelUpdate(true, "Press E to interact");
            }
        }

        // Interact with NPC
        if (Input.GetKeyDown(KeyCode.E) && highlight != null && highlight.CompareTag("NPC"))
        {
            // You could also check for distance here if needed
            selection = highlight; // Set the selection to the highlighted NPC

            // Activate all IAction components on the selected NPC
            IAction[] actions = selection.GetComponents<IAction>();
            
            foreach (var action in actions)
            {
                action.Activate();
            }

            // Reset the selection and hide the interaction text
            selection = null;
            highlight = null;
            GameManager.instance.interactionPanelUpdate(false, null);
        }
    }
}
