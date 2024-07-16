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
    [SerializeField] private AudioClip audioA;
    [SerializeField] private AudioClip audioB;
    [SerializeField] private AudioClip audioC;
    [SerializeField] private AudioClip audioD;
    
    private int layerAOrbCount, layerBOrbCount, layerCOrbCount, layerDOrbCount;

    [SerializeField] private int memoryPlatesFillRequired;
    
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
        if (snappedObject != null)
        {
            snappedObject.transform.position = Vector3.Lerp(snappedObject.transform.position, transform.position, Time.deltaTime * snapSpeed);
            time -= Time.deltaTime;
            
            if(time <= 0f)
            {
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
        if(filledPlates.Count == memoryPlatesFillRequired) {
            foreach(GameObject plate in filledPlates) {
                DisconnectedSoundOrbHandler disconnectedSoundOrb = plate.GetComponent<DisconnectedSoundOrbHandler>();
                
                if(disconnectedSoundOrb.MemoryLayer > GameManager.Ins.CurrentMemoryLayer + 1) {
                    disconnectedSoundOrb.MemoryLayer = GameManager.Ins.CurrentMemoryLayer + 1;
                } else if(disconnectedSoundOrb.MemoryLayer < GameManager.Ins.CurrentMemoryLayer - 1) {
                    disconnectedSoundOrb.MemoryLayer = GameManager.Ins.CurrentMemoryLayer - 1; 
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

            // filledPlates.Clear();
            SelectNextLevel();
        }
    }

    private void SelectNextLevel() {
        if(LayerAOrbIsMajority()) {
            //result A
            GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerA);
        } else if(LayerBOrbIsMajority()) {
            GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerB);
        } else if(LayerCOrbIsMajority()) {
            GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerC);
        } else if(LayerDOrbIsMajority()) {
            GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerD);
        } else {
            //Stay in the current layer and load next scene
            switch(GameManager.Ins.CurrentMemoryLayer) {
                case MemoryLayers.A:
                    GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerA);
                    break;
                case MemoryLayers.B:
                    GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerB);
                    break;
                case MemoryLayers.C:
                    GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerC);
                    break;
                case MemoryLayers.D:
                    GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerD);
                    break;
            }
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
