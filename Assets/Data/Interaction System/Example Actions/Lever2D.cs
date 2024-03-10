using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever2D : MonoBehaviour , IAction
{
    [SerializeField]
    private Sprite leverOff;
    [SerializeField]
    private Sprite leverOn;
    // Start is called before the first frame update

    private bool activated;

    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = leverOff;

    }

    public void Activate()
    {
        activated = !activated;
        if (activated)
        {
            GetComponent<SpriteRenderer>().sprite = leverOn;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = leverOff;
        }

    }

}
