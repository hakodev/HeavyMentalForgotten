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

    private void GetCurrentLevel() {
        currentLevel = GetLevelFromSceneIndex(SceneManager.GetActiveScene().buildIndex);
    }

    private Levels GetLevelFromSceneIndex(int sceneIndex) {
        return (Levels)sceneIndex;
    }

    private void SetCurrentMemoryLayer() {
        switch(currentLevel) {
            case Levels.SOUND1_1_LAYER_A:
            case Levels.SOUND1_2_LAYER_A:
            case Levels.SCENE1_LAYER_A:
            case Levels.SOUND2_LAYER_A:
            case Levels.SCENE2_LAYER_A:
            case Levels.SOUND3_1_LAYER_A:
            case Levels.SCENE3_LAYER_A:
            case Levels.SOUND4_LAYER_A:
            case Levels.Epilogue_LAYER_A:
                currentLayer = MemoryLayers.A;
                break;

            case Levels.SOUND1_2_LAYER_B:
            case Levels.SCENE1_LAYER_B:
            case Levels.SOUND2_LAYER_B:
            case Levels.SCENE2_LAYER_B:
            case Levels.SOUND3_1_LAYER_B:
            case Levels.SCENE3_LAYER_B:
            case Levels.SOUND4_LAYER_B:
            case Levels.Epilogue_LAYER_B:
                currentLayer = MemoryLayers.B;
                break;

            case Levels.SOUND2_LAYER_C:
            case Levels.SCENE2_LAYER_C:
            case Levels.SOUND3_1_LAYER_C:
            case Levels.SOUND3_3_LAYER_C:
            case Levels.SCENE3_LAYER_C:
            case Levels.SOUND4_LAYER_C:
            case Levels.Epilogue_LAYER_C:
                currentLayer = MemoryLayers.C;
                break;

            case Levels.SOUND3_2_LAYER_D:
            case Levels.SOUND3_3_LAYER_D:
            case Levels.SCENE3_LAYER_D:
            case Levels.SOUND4_LAYER_D:
            case Levels.Epilogue_LAYER_D:
                currentLayer = MemoryLayers.D;
                break;

            default:
                currentLayer = MemoryLayers.A;
                break;
        }
    }
}
