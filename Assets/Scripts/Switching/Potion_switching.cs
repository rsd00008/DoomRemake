using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_switching : MonoBehaviour
{
    private int selectedPotion = 0;

    void Start()
    {
        SelectPotion(true);
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedPotion = selectedPotion;

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedPotion = 0;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedPotion = 1;
        }

        if(Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedPotion = 2;
        }

        if(previousSelectedPotion != selectedPotion)
        {
            GameManager.instance.UpdateItemShowed(ItemShowed.Potions);
            //SelectPotion(true);
        }
    }

    public void SelectPotion(bool selected)
    {
        int i = 0;
        
        foreach (Transform potion in transform)
        {
            if (i == selectedPotion)
            {
                if(selected == false){
                    potion.gameObject.SetActive(false);
                }else{
                    potion.gameObject.SetActive(true);
                }
            }
            else
            {
                potion.gameObject.SetActive(false);
            }

            i++;
        } 
    }  
}
