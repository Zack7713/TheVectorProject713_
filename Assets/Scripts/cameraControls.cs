using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;
    //add variables for 3rd person cam
    [Header("---------3rd Person View---------")]
    [SerializeField] bool is3rdPercam;
    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    //adding variables to move the player weapon when aiming in 3rd person 


    [SerializeField] bool invertY;

    float xRot;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //added check for 3rd person view
        if (is3rdPercam)
        {
            xAxis.Update(Time.deltaTime);
            yAxis.Update(Time.deltaTime);
        }
        else
        {
            //get input
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

            //clamp rotation on the y - axis
            if (invertY)
                xRot += mouseY;
            else
                xRot -= mouseY;

            //now we clamp the rotation on the x-axis
            xRot = Mathf.Clamp(xRot, lockVertMin, lockVertMax);

            //rotate the camera on the x-axis
            transform.localRotation = Quaternion.Euler(xRot, 0, 0);

            //rotate the player on the y-axis
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }
    //3rd person camera functionality 
    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis.Value, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }
}
