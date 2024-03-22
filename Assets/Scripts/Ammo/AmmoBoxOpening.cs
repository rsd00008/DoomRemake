using UnityEngine;

public class AmmoBoxOpening : MonoBehaviour, IAction
{
    public GameObject ReferenceObject;
    public GameObject ObjectToRotate;
    
    public float rotationSpeed = 10.0f;
    public float rotationTarget;
    private bool startRotation = false;

    private Quaternion targetRotation;
    
    public void Activate()
    {
        startRotation = true;
        targetRotation = ObjectToRotate.transform.rotation * Quaternion.Euler(-rotationTarget, 0f, 0f);
    }

    void Update()
    {
        if (startRotation == true)
        {
            // Perform the rotation
            ObjectToRotate.transform.rotation = Quaternion.RotateTowards(ObjectToRotate.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the rotation has reached the target rotation within a small tolerance
            if (Quaternion.Angle(ObjectToRotate.transform.rotation, targetRotation) < 1f)
            {
                startRotation = false; // Stop further rotation

                // Ensure it snaps to the exact target rotation
                ObjectToRotate.transform.rotation = targetRotation;

                // Call Activate on AmmoReload
                transform.GetComponent<AmmoReload>();
            }
        }
    }
}
