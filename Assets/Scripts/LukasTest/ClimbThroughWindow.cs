using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClimbThroughWindow : MonoBehaviour
{
    [SerializeField]
    private float maxRange;
    private Transform player;

    [SerializeField]
    private Transform playerSpawnPosOutside;

    [Header("Insert sprite which fades in (fogOfWall in boy's room)")]
    [SerializeField]
    private SpriteRenderer fogOfWallSprite;
    [SerializeField]
    private float fadeDuration;
    
 
    private float timeAfterFade;
    [SerializeField]
    private float timeBeforePlayerTeleport;

    [Header("Should be false")]
    [SerializeField]
    private bool hasBeenTeleported;
    [SerializeField]
    private bool hasStartedToFade;
    [SerializeField]
    private int timesOfClick;



    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStartedToFade && !hasBeenTeleported)
        {
            timeAfterFade += Time.deltaTime;
            if (timeAfterFade >= timeBeforePlayerTeleport) 
            {
                //teleport player to outside
                player.transform.position = new Vector3(playerSpawnPosOutside.position.x, player.transform.position.y, player.transform.position.z);
                hasBeenTeleported = true;
            }
        }
    }

    private void OnMouseDown()
    {
        timesOfClick++;
        Debug.Log(gameObject.name + ": On mouse Down");
        if (Input.GetMouseButtonDown(0) && hasStartedToFade == false)
        {
            if ((transform.position - player.position).magnitude < maxRange)
            {
                fogOfWallSprite.DOFade(1f, fadeDuration);
                hasStartedToFade = true;
                
            }
            else
            {
                Debug.Log(gameObject.name + " out of range. Current distance is " + (this.transform.position - player.position).magnitude);
            }
        }

    }
}
