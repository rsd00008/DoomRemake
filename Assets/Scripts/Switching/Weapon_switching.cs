using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_switching : MonoBehaviour
{
    private int selectedWeapon = 0;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedPotion = selectedWeapon;

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            GameManager.instance.UpdateItemShowed(ItemShowed.Weapons);

            if(selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }  
        
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            GameManager.instance.UpdateItemShowed(ItemShowed.Weapons);

            if(selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        if(previousSelectedPotion != selectedWeapon)
        {
            GameManager.instance.UpdateItemShowed(ItemShowed.Weapons);
            SelectWeapon(true);
        }
    }

    public void SelectWeapon(bool selected)
    {
        int i = 0;

        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                if(selected == false){
                    weapon.gameObject.SetActive(false);
                }else{
                    weapon.gameObject.SetActive(true);
                }
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            i++;
        } 
    }
}