using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectObject : MonoBehaviour
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
        if (highlight != null)
        {
            var outline = highlight.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
            highlight = null;
            if (interactionText != null)
                interactionText.gameObject.SetActive(false);
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
                if (interactionText != null)
                    interactionText.gameObject.SetActive(true);
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
            if (interactionText != null)
                interactionText.gameObject.SetActive(false);
        }
    }
}
