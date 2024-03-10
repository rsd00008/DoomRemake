using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObject : MonoBehaviour
{
    public GameObject pocion;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) //si pulsas la tecla 1
        {
            pocion.SetActive(true);
        }   

        if(Input.GetKeyDown(KeyCode.Alpha2)) //si pulsas la tecla 2
        {
            pocion.SetActive(false);
        }   
    }
}
