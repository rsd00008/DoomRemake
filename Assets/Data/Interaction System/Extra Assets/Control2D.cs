using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control2D : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(-0.05f,0,0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(0.05f, 0, 0);
        }
    }
}
