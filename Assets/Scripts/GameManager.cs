using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour {
    public static GameManager Ins { get; private set; }
    [Header("Enable if the player should be running")]
    [SerializeField] private bool runMode;
    [field: SerializeField] public MemoryLayers CurrentMemoryLayer { get; private set; } // Set this in the inspector
    public Levels CurrentLevel { get; private set; }

    [Header("Attach the UI Canvas prefab to the scene, then drag\n" +
            "or select the BlackScreenImage game object below.")]
    [SerializeField] private Image blackScreen;
    [SerializeField] private float fadeDuration;

    [Header("Set the next possible levels the player can go to.\n" +
            "Set to NULL if that layer is unreachable.")]
    [SerializeField] private Levels nextLevelLayerA;
    [SerializeField] private Levels nextLevelLayerB;
    [SerializeField] private Levels nextLevelLayerC;
    [SerializeField] private Levels nextLevelLayerD;

    public bool RunMode {
        get {
            return runMode;
        }
        set {
            runMode = value;
        }
    }

    public Levels NextLevelLayerA {
        get {
            return nextLevelLayerA;
        }
    }

    public Levels NextLevelLayerB {
        get {
            return nextLevelLayerB;
        }
    }

    public Levels NextLevelLayerC {
        get {
            return nextLevelLayerC;
        }
    }

    public Levels NextLevelLayerD {
        get {
            return nextLevelLayerD;
        }
    }

    private void Awake() {
        Ins = this;
        CurrentLevel = GetCurrentLevel();
    }

    private void Start() {
        blackScreen.DOFade(0f, fadeDuration);
    }

    public float GetFadeDuration() {
        return fadeDuration;
    }

    public void SetFadeDuration(float fadeDuration) {
        this.fadeDuration = fadeDuration;
    }

    private Levels GetCurrentLevel() {
        return (Levels)SceneManager.GetActiveScene().buildIndex; // Using scene index, so order the scenes accordingly
    }

    public void LoadNextLevel(Levels nextLevel) {
        blackScreen.DOFade(1f, fadeDuration).OnComplete(() => SceneManager.LoadScene((int)nextLevel));
    }

    public void QuitGame() {
        blackScreen.DOFade(1f, fadeDuration).OnComplete(() => Application.Quit());
    }
}
