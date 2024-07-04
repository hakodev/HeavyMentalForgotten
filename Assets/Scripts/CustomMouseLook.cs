// Attach this script to your player or camera GameObject
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomMouseLook : MonoBehaviour
{
    public float horizontalSpeed = 5f; // Adjust as needed
    public float verticalSpeed = 5f;   // Adjust as needed

    private Vector3 position;
    public float sensitivity;


    public float mouseSpeed = 100;
    public float keySpeed = 0.1f;


    void Update()
    {
        Test2();

    }

    private void Test1()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        //transform.position = mousePosition;

        position = mousePosition * sensitivity;

        gameObject.transform.position = position;
        /*
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        gameObject.transform.position += new Vector3 (h, v, 0);
        */
    }



    private void Test2()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            transform.position = mousePosition;
        }


        float x = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSpeed;
        float y = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSpeed;

        transform.position += new Vector3(x,y,0);
        

    }
}