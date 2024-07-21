using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationOrphanageHandler : MonoBehaviour
{
    [Header("Check only:")]
    [SerializeField] private float HorizontalAxis;
    [SerializeField] private bool runMode;

    [SerializeField] private bool lighterOnFire;
    [SerializeField] private bool lighterEmitsSpark;
    [SerializeField] private bool burnedPaper;


    [SerializeField] private bool docsHoldedByPlayer;
    [SerializeField] private bool docsWereAlreadyCollected;

    [Header("Level Specific:")]
    [SerializeField] private bool isSneakingInsteadWalking;
    [SerializeField] private bool lighterON;

    [Header("Assign references:")]
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private LighterActivation lighterActivation;
    [SerializeField] private ItemBehaviour itemBehaviour;

    //Animator relevant
    private const string PLAYER_IS_WALKING = "isWalking";
    private const string PLAYER_IS_RUNNING = "isRunning";
    private const string PLAYER_IS_SNEAKING = "isSneaking";
    private const string PLAYER_VELOCITY = "velocity";

    private const string IS_TAKING_DOCS = "isTakingDocs";
    private const string DOCS_TAKEN = "DocsTaken";
    private const string HOLD_DOCS_TIME = "HoldDocsTime";

    private const string LIGHTER_USE_TIME = "LighterUseTime";
    private const string LIGHTER_ON = "LighterON";
    private const string LIGHTER_ACTIVE = "LighterActive";

    private const string BURN_DOCS_TIME = "BurnDocsTime";
    private const string DOCS_BURNED = "DocsBurned";

    private Animator animator;


    [Header("Calculations: check only:")]
    //needed for calculation
    [SerializeField] private float lighterUseTime;
    [SerializeField] private float burnDocsTime;
    [SerializeField] private float holdDocsTime;






    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetValues();
        SetMovementAnimationValues();
        SetLighterAnimationValues();
        SetDocsAnimationValues();
    }

    private void GetValues()
    {
        //player
        HorizontalAxis = Mathf.Abs(playerMovementScript.HorizontalAxis);
        runMode = GameManager.Ins.RunMode;
        
        if (lighterActivation !=null)
        {
            //lighter
            lighterEmitsSpark = lighterActivation.isEmmittingSpark;
            lighterOnFire = lighterActivation.onFire;
            burnedPaper = lighterActivation.burnedPaper;
        }      
        if (itemBehaviour != null)
        {
            //docs
            docsHoldedByPlayer = itemBehaviour.holdedByPlayer;
            docsWereAlreadyCollected = itemBehaviour.wasAlreadyCollected;
        }     
    }

    private void SetDocsAnimationValues()
    {
        if (docsHoldedByPlayer)
        {
            holdDocsTime += Time.deltaTime;
            animator.SetFloat(HOLD_DOCS_TIME, holdDocsTime);
            animator.SetBool(DOCS_TAKEN, true);
        }

        if (burnedPaper)
        {
            burnDocsTime += Time.deltaTime;
            animator.SetBool(DOCS_BURNED, true);
            animator.SetFloat(BURN_DOCS_TIME, burnDocsTime);
        }
    }
    private void SetLighterAnimationValues()
    {
        
        if (lighterOnFire
            || lighterEmitsSpark   //to be tested if it looks nice
            )
        {
            lighterUseTime += Time.deltaTime;
            animator.SetBool(LIGHTER_ACTIVE, true);
        }
        else
        {
            lighterUseTime = 0;
            animator.SetBool(LIGHTER_ACTIVE, false);
        }
        animator.SetFloat(LIGHTER_USE_TIME, lighterUseTime);
        animator.SetBool(LIGHTER_ON, lighterON);

    }

    private void SetMovementAnimationValues()
    {
        //Movement
        if (HorizontalAxis > 0)
        {
            if (runMode)
            {
                animator.SetBool(PLAYER_IS_RUNNING, true);

                animator.SetBool(PLAYER_IS_SNEAKING, false);
                animator.SetBool(PLAYER_IS_WALKING, false);
            }
            else
            {

                if (isSneakingInsteadWalking)
                {
                    animator.SetBool(PLAYER_IS_SNEAKING, true);

                    animator.SetBool(PLAYER_IS_WALKING, false);
                    animator.SetBool(PLAYER_IS_RUNNING, false);
                }
                else
                {
                    animator.SetBool(PLAYER_IS_WALKING, true);
                    animator.SetBool(PLAYER_IS_SNEAKING, false);
                    animator.SetBool(PLAYER_IS_RUNNING, false);
                }
            }
        }
        else
        {
            animator.SetBool(PLAYER_IS_RUNNING, false);
            animator.SetBool(PLAYER_IS_SNEAKING, false);
            animator.SetBool(PLAYER_IS_WALKING, false);
        }
        animator.SetFloat(PLAYER_VELOCITY, HorizontalAxis);
    }
    
}
