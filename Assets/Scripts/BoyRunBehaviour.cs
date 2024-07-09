using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class BoyRunBehaviour : MonoBehaviour
{

    public float zTargetPos;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime;
    public float maxSpeed;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    [SerializeField]
    private bool startedRace;
    


    // Start is called before the first frame update
    void Start()
    {
        targetPosition = new Vector3(transform.position.z, transform.position.y, zTargetPos);
    }


    public void StartRun()
    {
        startedRace = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startedRace)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, maxSpeed);
        }
    }
}
