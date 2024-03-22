using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoReload : MonoBehaviour, IAction
{
    private int maxAmmo = 23;
    private int minAmmo = 8;
    
    // Update is called once per frame
    public void Activate()
    {
        Debug.Log("AmmoReload Activate");
        int ammoAmount = Random.Range(minAmmo, maxAmmo);
        GameManager.instance.updateGunAmmo(ammoAmount);
    }
}
