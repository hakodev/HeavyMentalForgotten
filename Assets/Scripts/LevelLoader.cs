using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public static LevelLoader Ins { get; private set; }
    [SerializeField] private Image blackScreen;
    [SerializeField] private float fadeDuration;

    [Header("Set the next possible levels the player can go to.\n" +
            "Set to NULL or leave blank if that layer is unreachable.")]
    [SerializeField] private Levels nextLevelLayerA;
    [SerializeField] private Levels nextLevelLayerB;
    [SerializeField] private Levels nextLevelLayerC;
    [SerializeField] private Levels nextLevelLayerD;

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
    }

    private void Start() {
        blackScreen.DOFade(0f, fadeDuration);
    }

    public void SetFadeDuration(float fadeDuration) {
        this.fadeDuration = fadeDuration;
    }

    public void LoadNextLevel(Levels nextLevel) {
        blackScreen.DOFade(1f, fadeDuration).OnComplete(() => SceneManager.LoadScene((int)nextLevel));
    }

    public void QuitGame() {
        blackScreen.DOFade(1f, fadeDuration).OnComplete(() => Application.Quit());
    }

}
