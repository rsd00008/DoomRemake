using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour, IAction
{

    public void Activate()
    {
        GameManager.instance.takeItem(gameObject);
    }
}
