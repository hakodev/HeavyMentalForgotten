using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class LayingDown : MonoBehaviour
{

    [SerializeField] private float requiredMousePressTime = 1f;
    //[SerializeField] 
    private float timeMousePressed;

    [SerializeField] private bool mousePressed;

    [SerializeField] private int currentAnimation = 0;
    //[SerializeField] private float firstAnimationDuration;

    [Header("Put player in here!")]
    [SerializeField] private Animator animator;
    //[SerializeField] 
    private float animatorIsLayingTimeLamp;

    [SerializeField] private Transform player;
    [SerializeField] private float maxRange;
   



    //private const string PLAYER_IS_WALKING = "isWalking";


    private void Awake()
    {
        //animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mousePressed)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1)
            || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.E))
            {
                timeMousePressed += Time.deltaTime;
                
                if (timeMousePressed > requiredMousePressTime)
                {
                    GameManager.Ins.LockMovement = true;
                    LayDown(currentAnimation);
                    currentAnimation++;

                }                
            }
            else
            {
                mousePressed = false;
                timeMousePressed = 0;
                animator.SetFloat("isLayingTime_Lamp", timeMousePressed);
            }
        }
    }

    
    private void LayDown(int pCurrentAnimation)
    {
        if (pCurrentAnimation == 0)
        {
            animator.SetBool("isLaying_Lamp", true);
            timeMousePressed = 0; //so player need to press again or hold longer for second animation
        }
        else if (pCurrentAnimation >= 1)
        {
            //here let us increase the time to trigger the second animation
            animator.SetFloat("isLayingTime_Lamp", timeMousePressed);
            animator.SetBool("isDown_Lamp", true);
        }
        return;

    }

    private void OnMouseDown()
    {  
        if ((transform.position - player.position).magnitude < maxRange)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1)
            || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.E))
            {
                mousePressed = true;
            }
        }
        else
        {
            Debug.Log(gameObject.name + " out of range. Current distance is " + (this.transform.position - player.position).magnitude);
        }
   
    }

}
