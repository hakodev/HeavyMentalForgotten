using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Ins { get; private set; }
    [field: SerializeField] public MemoryLayers CurrentLayer { get; private set; } // Set this in the inspector
    public Levels CurrentLevel { get; private set; }
    [field: SerializeField] public bool RunMode { get; set; }

    private void Awake() {
        Ins = this;
        CurrentLevel = GetCurrentLevel();
    }

    private Levels GetCurrentLevel() {
        return (Levels)SceneManager.GetActiveScene().buildIndex; // Using scene index, so order the scenes accordingly
    }
}
