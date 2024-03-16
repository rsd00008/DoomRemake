using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveItems : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    public float interactDistance = 5f; // Maximum distance at which objects can be interacted with

    void Update()
    {
        // Always attempt to clear the current highlight at the start of Update
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        // Check for interactive objects under the camera's forward vector
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit, interactDistance))
        {
            Transform currentHighlight = raycastHit.transform;

            if (currentHighlight.CompareTag("Interactive") && currentHighlight != selection)
            {
                // Enable the outline for the object currently in focus
                var outline = currentHighlight.gameObject.GetComponent<Outline>();
                if (outline == null)
                {
                    outline = currentHighlight.gameObject.AddComponent<Outline>();
                    outline.OutlineColor = Color.magenta;
                    outline.OutlineWidth = 7.0f;
                }
                outline.enabled = true;

                highlight = currentHighlight;
            }
        }

        // Handle selection
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (highlight)
            {
                if (selection != null)
                {
                    // Previously selected object's outline is disabled here
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                }

                // Update the selection
                selection = highlight;

                // Disable the outline for the newly selected object
                if (selection.gameObject.GetComponent<Outline>() != null)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                }
                
                // Reset highlight to allow for re-selection logic
                highlight = null;
            }
            else if (selection)
            {
                // If 'E' is pressed without a new highlight and there's a current selection, disable its outline
                selection.gameObject.GetComponent<Outline>().enabled = false;
                selection = null;
            }
        }
    }
}
