using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MemoryPlateHandler : MonoBehaviour
{
    public GameObject snappedObject;
    [SerializeField] private float snapSpeed = 5f;
    private SpriteRenderer spriteRenderer;
    private float time = 1.5f;
    public bool isSnapped = false;
    public static List<GameObject> filledPlates = new();
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioA;
    [SerializeField] private AudioClip audioB;
    [SerializeField] private AudioClip audioC;
    [SerializeField] private AudioClip audioD;

    private static bool audioPlayed = false;

    private int layerAOrbCount, layerBOrbCount, layerCOrbCount, layerDOrbCount;

    [SerializeField] private int memoryPlatesFillRequired;
    
    private void Awake()
    {
        audioPlayed = false;
        filledPlates.Clear();
        audioSource = GetComponent<AudioSource>();
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
        if(audioSource.isPlaying)
        {
            Debug.Log("AudioSource is currently playing.");
        }
        else
        {
            Debug.Log("AudioSource is not playing any audio.");
        }
        
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
                { ;
                    filledPlates.Add(snappedObject);
                    Debug.Log(filledPlates.Count + " filled plates");
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
        }
    }

    public void SelectNextLevel() {

        if (!audioPlayed)
        {
            Debug.Log("Selecting next level.");
            if(LayerAOrbIsMajority()) {
                //result A
                StartCoroutine(PlayAudio(audioA, GameManager.Ins.NextLevelLayerA));
            } else if(LayerBOrbIsMajority()) {
                StartCoroutine(PlayAudio(audioB, GameManager.Ins.NextLevelLayerB));
            } else if(LayerCOrbIsMajority()) {
                StartCoroutine(PlayAudio(audioC, GameManager.Ins.NextLevelLayerC));
            } else if(LayerDOrbIsMajority()) {
                StartCoroutine(PlayAudio(audioD, GameManager.Ins.NextLevelLayerD));
            } else {
                //Stay in the current layer and load next scene
                switch(GameManager.Ins.CurrentMemoryLayer) {
                    case MemoryLayers.A:
                        StartCoroutine(PlayAudio(audioA, GameManager.Ins.NextLevelLayerA));
                        break;
                    case MemoryLayers.B:
                        StartCoroutine(PlayAudio(audioB, GameManager.Ins.NextLevelLayerB));
                        break;
                    case MemoryLayers.C:
                        StartCoroutine(PlayAudio(audioC, GameManager.Ins.NextLevelLayerC));
                        break;
                    case MemoryLayers.D:
                        StartCoroutine(PlayAudio(audioD, GameManager.Ins.NextLevelLayerD));
                        break;
                }
            }
            audioPlayed = true;
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
    
    private IEnumerator PlayAudio(AudioClip audioClip, Levels nextLevel)
    {
        audioSource.PlayOneShot(audioClip, 1f);
        yield return new WaitForSeconds(audioClip.length);
        GameManager.Ins.LoadNextLevel(nextLevel);
    }
}
