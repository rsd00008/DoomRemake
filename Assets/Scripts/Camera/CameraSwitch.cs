using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera first;
    public Camera third;
    private bool firstActivated = true;

    //WEAPONS CHAGNE VIEW
    public GameObject[] weapons;
    public Transform[] weaponsTransform_FirstPerson;
    public Transform[] weaponsTransform_ThirdPerson;

    void Start()
    {
        firstActivated = true;
        first.enabled = true;
        third.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            firstActivated = !firstActivated;
            first.enabled = firstActivated;
            third.enabled = !firstActivated;
            
            changeWeaponView();
        }
    }

    public void changeWeaponView()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (firstActivated)
            {
                weapons[i].transform.localScale = weaponsTransform_FirstPerson[i].localScale;
                weapons[i].transform.localPosition = weaponsTransform_FirstPerson[i].localPosition;
                weapons[i].transform.localRotation = weaponsTransform_FirstPerson[i].localRotation;
            }
            else
            {
                weapons[i].transform.localScale = weaponsTransform_ThirdPerson[i].localScale;
                weapons[i].transform.localPosition = weaponsTransform_ThirdPerson[i].localPosition;
                weapons[i].transform.localRotation = weaponsTransform_ThirdPerson[i].localRotation;
            }
        }
    }
}
