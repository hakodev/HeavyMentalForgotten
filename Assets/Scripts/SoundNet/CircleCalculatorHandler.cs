using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCalculatorHandler : MonoBehaviour
{
    [Header("Circle Behaviour Stats")]
    Vector2 circleCenter = new Vector2(0, 0);
    [SerializeField] private float circleRadius;
    private GameObject[] connectedOrbs;
    private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        connectedOrbs = GameObject.FindGameObjectsWithTag("SoundOrbConnected");
    }

    void Update() 
    {
        Circlecalculate();
    }

    private void Circlecalculate() 
    {
        foreach (GameObject orb in connectedOrbs) 
        {
            Vector2 orbPosition = new Vector2(orb.transform.position.x, orb.transform.position.y);
            float distance = Vector2.Distance(circleCenter, orbPosition);
            ConnectedSoundOrbHandler connectedOrbReference = orb.GetComponent<ConnectedSoundOrbHandler>();

            if (distance > circleRadius) 
            {
                Debug.Log("Orb is outside the circle");
                if (!audioSource.isPlaying)
                {
                    connectedOrbReference.isOutsideCircle = true;
                    audioSource.PlayOneShot(connectedOrbReference.connectedAudioClip);
                }
                
            }
            else
            {
                connectedOrbReference.isOutsideCircle = false;
            }
        }
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(circleCenter, circleRadius);
    }
}
