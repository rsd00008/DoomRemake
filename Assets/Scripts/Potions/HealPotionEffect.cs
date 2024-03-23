using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPotionEffect : MonoBehaviour
{
    [SerializeField] private float healAmount = 30;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && GameManager.instance.getHealPotionAmount() > 0 && GameManager.instance.getPlayerLife() < GameManager.instance.getMaxPlayerLife()){
            GameManager.instance.UpdatePlayerLife(healAmount);
            GameManager.instance.UsedItem(gameObject);

            if(GameManager.instance.getHealPotionAmount() == 0){
                GameManager.instance.UpdateItemShowed(ItemShowed.Weapons);
            }
        }
    }
}
