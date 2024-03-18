using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectObject : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    public float interactDistance = 5f;

    public GameManager gameManager;

    void Start()
    {
        gameManager.interactionPanelUpdate(false, null);
    }

    void Update()
    {
        if (highlight != null)
        {
            var outline = highlight.GetComponent<Outline>();
            
            if (outline != null)
            {
                outline.enabled = false;
            }
            
            highlight = null;

            gameManager.interactionPanelUpdate(false, null);
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit, interactDistance))
        {
            Transform currentHighlight = raycastHit.transform;

            if (currentHighlight.CompareTag("Interactive") && currentHighlight != selection)
            {
                var outline = currentHighlight.GetComponent<Outline>();

                if (outline == null)
                {
                    outline = currentHighlight.gameObject.AddComponent<Outline>();
                    outline.OutlineColor = Color.magenta;
                    outline.OutlineWidth = 7.0f;
                }

                outline.enabled = true;

                highlight = currentHighlight;

                gameManager.interactionPanelUpdate(true, "Press E to interact");
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && highlight != null)
        {
            selection = highlight;

            IAction[] actions = selection.GetComponents<IAction>();

            foreach (var action in actions)
            {
                action.Activate();
            }
            
            var outline = selection.GetComponent<Outline>();

            if (outline != null)
            {
                outline.enabled = false;
            }

            selection.tag = "Untagged";
            selection = null;

            highlight = null;

            gameManager.interactionPanelUpdate(false, null);
        }
    }
}
