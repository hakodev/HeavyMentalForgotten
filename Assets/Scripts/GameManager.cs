using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Ins { get; private set; }
    public MemoryLayers CurrentLayer { get; private set; }
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
                CurrentLayer = MemoryLayers.A;
                break;

            case Levels.SOUND1_2_LAYER_B:
            case Levels.SCENE1_LAYER_B:
            case Levels.SOUND2_LAYER_B:
            case Levels.SCENE2_LAYER_B:
            case Levels.SOUND3_1_LAYER_B:
            case Levels.SCENE3_LAYER_B:
            case Levels.SOUND4_LAYER_B:
            case Levels.Epilogue_LAYER_B:
                CurrentLayer = MemoryLayers.B;
                break;

            case Levels.SOUND2_LAYER_C:
            case Levels.SCENE2_LAYER_C:
            case Levels.SOUND3_1_LAYER_C:
            case Levels.SOUND3_3_LAYER_C:
            case Levels.SCENE3_LAYER_C:
            case Levels.SOUND4_LAYER_C:
            case Levels.Epilogue_LAYER_C:
                CurrentLayer = MemoryLayers.C;
                break;

            case Levels.SOUND3_2_LAYER_D:
            case Levels.SOUND3_3_LAYER_D:
            case Levels.SCENE3_LAYER_D:
            case Levels.SOUND4_LAYER_D:
            case Levels.Epilogue_LAYER_D:
                CurrentLayer = MemoryLayers.D;
                break;

            default:
                CurrentLayer = MemoryLayers.A;
                break;
        }
    }
}
