using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Ins { get; private set; }
    public MemoryLayers currentLayer;
    private Levels currentLevel;

    private void Awake() {
        Ins = this;
    }

    private void Start() {
        GetCurrentLevel();
        SetCurrentMemoryLayer();
    }

    private void SetCurrentMemoryLayer() {
        switch(currentLevel) {
            case Levels.SOUND1_1:
            case Levels.SOUND1_2_LAYER_A:
            case Levels.SCENE1_LAYER_A:
            case Levels.SOUND2_LAYER_A:
            case Levels.SCENE2_LAYER_A:
            case Levels.SOUND3_1_LAYER_A:
            case Levels.SCENE3_LAYER_A_B_C: // Can be layers A, B, C. Setting to A by default
            case Levels.SOUND4: // Can be any layer, setting to A for now
            case Levels.Epilogue_LAYER_A:
                currentLayer = MemoryLayers.A;
                break;

            // UNFINISHED SCRIPT, ASK ME (AHMET) BEFORE EDITING
        }
    }

    private void GetCurrentLevel() {
        currentLevel = GetLevelFromSceneIndex(SceneManager.GetActiveScene().buildIndex);
    }

    private Levels GetLevelFromSceneIndex(int sceneIndex) {
        return (Levels)sceneIndex;
    }
}
