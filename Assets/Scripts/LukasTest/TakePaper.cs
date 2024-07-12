using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TakePaper : MonoBehaviour
{
    public float maxRange;
    [SerializeField]
    private ItemBehaviour itemBehaviourScript;
    [SerializeField]
    private Transform player;
    [Header("What happens after the player took the item?")]
    public GameObject EventSystem;


    // Start is called before the first frame update
    void Awake()
    {
        itemBehaviourScript = FindObjectOfType<ItemBehaviour>().gameObject.GetComponent<ItemBehaviour>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if ((transform.position - player.position).magnitude < maxRange)
        {
            //shwowing max range visually
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if ((transform.position - player.position).magnitude < maxRange)
            {
                itemBehaviourScript.PlayerTakesItem();
            }
            else
            {
                Debug.Log(gameObject.name + " out of range. Current distance is " + (this.transform.position - player.position).magnitude);
            }
        }
    }
}
