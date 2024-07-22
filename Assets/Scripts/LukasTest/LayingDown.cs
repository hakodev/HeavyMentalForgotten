using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.Events;


public class LayingDown : MonoBehaviour
{

    [SerializeField] private float requiredMousePressTime = 1f;
    //[SerializeField] 
    private float timeMousePressed;

    [SerializeField] private bool mousePressed;

    [SerializeField] private int currentAnimation = 0;

    [Header("Put player in here!")]
    [SerializeField] private Animator animator;
    //[SerializeField] 
    private float animatorIsLayingTimeLamp;

    [SerializeField] private Transform player;
    [Header("(!)Range is calculated from the pivot of the object")]
    [SerializeField] private float maxRange;

    [Header("Level Progression: ")]
    [SerializeField] private float timeAfterAnimToEnd;
    public float timeSinceAnim;
    public bool animationFinished = false;

    public UnityEvent ProgressionEvent;
    private bool eventPlayed;



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
            }
        }
        CheckForLevelProgression();
    }

    private void CheckForLevelProgression()
    {
        if (animationFinished)
        {
            if (!eventPlayed)
            {
                ProgressionEvent.Invoke();
                eventPlayed = true;
            }

            timeSinceAnim += Time.deltaTime;
            if (timeSinceAnim >= timeAfterAnimToEnd)
            {
                GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerA);
            }
        }
    }



    private void LayDown(int pCurrentAnimation)
    {
        if (pCurrentAnimation == 0)
        {
            animator.SetFloat("layingAnimationNumber", currentAnimation + 1f);
            //animator.SetBool("isLaying_Lamp", true);
        }
        else if (pCurrentAnimation >= 1)
        {
            animator.SetFloat("layingAnimationNumber", currentAnimation + 1f);
            //animator.SetBool("isDown_Lamp", true);

            animationFinished = true;

        }
        timeMousePressed = 0; //so player need to press again or hold longer for second animation
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
