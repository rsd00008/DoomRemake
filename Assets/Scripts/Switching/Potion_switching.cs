using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_switching : MonoBehaviour
{
    private int selectedPotion = 0;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedPotion = selectedPotion;

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.instance.UpdateItemShowed(ItemShowed.Potions);
            selectedPotion = 0;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            GameManager.instance.UpdateItemShowed(ItemShowed.Potions);
            selectedPotion = 1;
        }

        if(Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            GameManager.instance.UpdateItemShowed(ItemShowed.Potions);
            selectedPotion = 2;
        }

        if(previousSelectedPotion != selectedPotion)
        {
            GameManager.instance.UpdateItemShowed(ItemShowed.Potions);
            SelectPotion(true);
        }
    }

    public void SelectPotion(bool selected)
    {
        int i = 0;
        
        foreach (Transform potion in transform)
        {
            if (i == selectedPotion)
            {
                if(selected == true && CheckPotionAmount(potion.name) == true){
                    potion.gameObject.SetActive(true);
                }else{
                    potion.gameObject.SetActive(false);
                }
            }
            else
            {
                potion.gameObject.SetActive(false);
            }

            i++;
        }
    }  

    public bool CheckPotionAmount(string potionName)
    {
        if(potionName == "HealPotion")
        {
            return GameManager.instance.getHealPotionAmount() > 0;
        }
        else if(potionName == "SpeedPotion")
        {
            return GameManager.instance.getSpeedPotionAmount() > 0;
        }
        else if(potionName == "AcidPotion")
        {
            return GameManager.instance.getAcidPotionAmount() > 0;
        }

        return false;
    }
}
