using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour, IAction
{
    public GameManager gameManager;

    public void Activate()
    {
        gameManager.takeItem(gameObject);
    }
}
