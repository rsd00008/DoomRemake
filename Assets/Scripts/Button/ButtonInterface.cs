using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInterface : MonoBehaviour
{
    private float messageTime = 0.1f;

    [SerializeField]
    private GameObject messageGameObject;

    [SerializeField]
    private GameObject messageTextGameObject;

    private Text messageText;

    void Start()
    {
        messageText = messageTextGameObject.GetComponent<Text>();
        messageGameObject.SetActive(false);
    }

    void FixedUpdate()
    {

    }

    public void showMessage(string message)
    {
        messageText.text = message;
        messageGameObject.SetActive(true);
        CancelInvoke();
        Invoke("clearMessage", messageTime);
    }

    public void clearMessage()
    {
        messageGameObject.SetActive(false);
    }


}
