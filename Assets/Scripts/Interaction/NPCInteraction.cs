using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class NPCInteraction : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    public float interactDistance = 5f;
    public TextMesh interactionText;

    void Start()
    {
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Remove the previous highlight
        if (highlight != null)
        {
            highlight = null;
            if (interactionText != null)
                interactionText.gameObject.SetActive(false);
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        // Detect NPC
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit, interactDistance))
        {
            Transform currentHighlight = raycastHit.transform;

            if (currentHighlight.CompareTag("NPC"))
            {
                highlight = currentHighlight; // Set the new highlight

                // Show interaction message when an NPC is in focus
                if (interactionText != null)
                {
                    interactionText.text = "Press E to interact";
                    interactionText.gameObject.SetActive(true);
                }
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
            if (interactionText != null)
                interactionText.gameObject.SetActive(false);
        }
    }
}
