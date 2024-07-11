using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemBehaviour : MonoBehaviour
{

    public GameObject itemAtStart;
    public GameObject itemAtPlayer;
    public GameObject itemAtEnd;

    [SerializeField]
    private bool wasAlreadyCollected;
    [SerializeField]
    private bool holdedByPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (!holdedByPlayer)
        {
            itemAtStart.SetActive(true);
            itemAtPlayer.SetActive(false);
            itemAtEnd.SetActive(false);
        }    
    }

    public void PlayerTakesItem()
    {
        if (!wasAlreadyCollected)
        {
            holdedByPlayer = true;

            itemAtStart.SetActive(false);
            itemAtPlayer.SetActive(true);
            itemAtEnd.SetActive(false);
        }    
    }

    public void PlayerLeavesItemAtPoint()
    {
        if (holdedByPlayer)
        {
            holdedByPlayer = false;
            wasAlreadyCollected = true;

            itemAtStart.SetActive(false);
            itemAtPlayer.SetActive(false);
            itemAtEnd.SetActive(true);
        }     
    }

    public void PlayerDropsItem()
    {
        if (holdedByPlayer)
        {
            holdedByPlayer = false;
            wasAlreadyCollected = true;

            itemAtStart.SetActive(false);
            itemAtPlayer.SetActive(false);
            itemAtEnd.transform.position = itemAtPlayer.transform.position;
            itemAtEnd.SetActive(true);
        }       
    }


    // Update is called once per frame
    void Update()
    {
        //HiddenTools
        if (Input.GetKey(KeyCode.O))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerTakesItem();            
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                PlayerLeavesItemAtPoint();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayerDropsItem();
            }
        }
    }
}

/*
        if (holdedByPlayer)
        {
            //setting item at Start inactive
            if (itemAtStart.gameObject.activeInHierarchy == false)
            {
                itemAtStart.gameObject.SetActive(false);
            }
            //setting item at Player active
            if (itemAtPlayer.gameObject.activeInHierarchy == false)
            {
                itemAtStart.gameObject.SetActive(true);
            }
        } 
        else if (wasAlreadyCollected)
        {
            //setting item at Player inactive
            if (itemAtPlayer.gameObject.activeInHierarchy == true)
            {
                itemAtStart.gameObject.SetActive(false);
            }
            //setting item at End active
            if (itemAtPlayer.gameObject.activeInHierarchy == false)
            {
                itemAtStart.gameObject.SetActive(true);
            }
        }
        else
        {
            //setting item at Player inactive
            if (itemAtPlayer.gameObject.activeInHierarchy == true)
            {
                Debug.Log("setting item at Player inactive");
                itemAtStart.SetActive(false);
            }
            //setting item at End inactive
            if (itemAtEnd.gameObject.activeInHierarchy == true)
            {
                Debug.Log("setting item at End inactive");
                itemAtStart.SetActive(false);
            }
            //setting item at Start active
            if (itemAtStart.gameObject.activeInHierarchy == false)
            {
                Debug.Log("setting item at Start active");
                itemAtStart.SetActive(true);
            }
        }
        */
