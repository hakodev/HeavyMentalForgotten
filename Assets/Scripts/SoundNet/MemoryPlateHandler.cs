using UnityEngine;
using System.Collections.Generic;

public class MemoryPlateHandler : MonoBehaviour
{
    public GameObject snappedObject;
    [SerializeField] private float snapSpeed = 5f;
    private SpriteRenderer spriteRenderer;
    private float time = 1.5f;
    private bool isSnapped = false;
    public static List<GameObject> filledPlates = new();
    
    private int layerAOrbCount, layerBOrbCount, layerCOrbCount, layerDOrbCount;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        layerAOrbCount = 0;
        layerBOrbCount = 0;
        layerCOrbCount = 0;
        layerDOrbCount = 0;
    }

    private void Update()
    {
        foreach (GameObject plate in filledPlates)
        {
            Debug.Log("Filled Plates:");
            Debug.Log(this.gameObject.name + plate.name);
        }
        
        if (snappedObject != null)
        {
            snappedObject.transform.position = Vector3.Lerp(snappedObject.transform.position, transform.position, Time.deltaTime * snapSpeed);
            time -= Time.deltaTime;
            
            if(time <= 0f)
            {
                Debug.Log("Snapped");
                snappedObject.GetComponent<DisconnectedSoundOrbHandler>().enabled = true;
                // snappedObject.GetComponent<CircleCollider2D>().enabled = false;
                // this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                isSnapped = true;
                
                if (!filledPlates.Contains(snappedObject))
                {
                    filledPlates.Add(snappedObject);
                }
            }
        }

        CalculateOrbs();
    }

    private void CalculateOrbs() {
        if(filledPlates.Count == 3) {
            foreach(GameObject plate in filledPlates) {
                DisconnectedSoundOrbHandler disconnectedSoundOrb = plate.GetComponent<DisconnectedSoundOrbHandler>();
                Debug.Log(disconnectedSoundOrb.isPlacedOnSnap + " check if its getting called or not");
                if(disconnectedSoundOrb.MemoryLayer > GameManager.Ins.CurrentLayer + 1) {
                    disconnectedSoundOrb.MemoryLayer = GameManager.Ins.CurrentLayer + 1;
                } else if(disconnectedSoundOrb.MemoryLayer < GameManager.Ins.CurrentLayer - 1) {
                    disconnectedSoundOrb.MemoryLayer = GameManager.Ins.CurrentLayer - 1;
                }

                switch(disconnectedSoundOrb.MemoryLayer) {
                    case MemoryLayers.A:
                        layerAOrbCount++;
                        break;
                    case MemoryLayers.B:
                        layerBOrbCount++;
                        break;
                    case MemoryLayers.C:
                        layerCOrbCount++;
                        break;
                    case MemoryLayers.D:
                        layerDOrbCount++;
                        break;
                }
            }

            filledPlates.Clear();
            SelectNextLevel();
        }
    }

    private void SelectNextLevel() {
        if(LayerAOrbIsMajority()) {
            //Load the A layer of the next scene
        } else if(LayerBOrbIsMajority()) {
            //Load the B layer of the next scene
        } else if(LayerCOrbIsMajority()) {
            //Load the C layer of the next scene
        } else if(LayerDOrbIsMajority()) {
            //Load the D layer of the next scene
        }
    }

    private bool LayerAOrbIsMajority() {
        return layerAOrbCount > layerBOrbCount && layerAOrbCount > layerCOrbCount && layerAOrbCount > layerDOrbCount;
    }

    private bool LayerBOrbIsMajority() {
        return layerBOrbCount > layerAOrbCount && layerBOrbCount > layerCOrbCount && layerBOrbCount > layerDOrbCount;
    }

    private bool LayerCOrbIsMajority() {
        return layerCOrbCount > layerAOrbCount && layerCOrbCount > layerBOrbCount && layerCOrbCount > layerDOrbCount;
    }

    private bool LayerDOrbIsMajority() {
        return layerDOrbCount > layerAOrbCount && layerDOrbCount > layerBOrbCount && layerDOrbCount > layerCOrbCount;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isSnapped)
        {
            return;
        }
        
        DisconnectedSoundOrbHandler disconnectedSoundOrbHandler = other.GetComponent<DisconnectedSoundOrbHandler>();

        if (other.gameObject.CompareTag("SoundOrbDisconnected") && disconnectedSoundOrbHandler.isHovering)
        {
            snappedObject = other.gameObject;
            spriteRenderer.material.color = Color.gray;
            disconnectedSoundOrbHandler.isPlacedOnSnap = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (isSnapped)
        {
            return;
        }
        
        DisconnectedSoundOrbHandler disconnectedSoundOrbHandler = other.GetComponent<DisconnectedSoundOrbHandler>();
        if (other.gameObject.CompareTag("SoundOrbDisconnected"))
        {
            time = 3f;
            if (!isSnapped)
            {
                snappedObject = null;
            }
            disconnectedSoundOrbHandler.isPlacedOnSnap = false;
            spriteRenderer.material.color = Color.black;
        }
    }
}
