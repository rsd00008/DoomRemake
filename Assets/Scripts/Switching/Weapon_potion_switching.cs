using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_potion_switching : MonoBehaviour
{
    int selectedPotion = 0;

    void Start()
    {
        SelectPotion();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedPotion = selectedPotion;

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(selectedPotion >= transform.childCount - 1)
            {
                selectedPotion = 0;
            }
            else
            {
                selectedPotion++;
            }
        }  
        
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(selectedPotion <= 0)
            {
                selectedPotion = transform.childCount - 1;
            }
            else
            {
                selectedPotion--;
            }
        }

        if(previousSelectedPotion != selectedPotion)
        {
            SelectPotion();
        }
    }

    void SelectPotion()
    {
        int i = 0;
        
        foreach (Transform potion in transform)
        {
            if (i == selectedPotion)
            {
                potion.gameObject.SetActive(true);
            }
            else
            {
                potion.gameObject.SetActive(false);
            }

            i++;
        } 
    }
}
