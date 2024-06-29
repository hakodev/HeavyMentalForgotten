using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public static LevelLoader Ins { get; private set; }
    [SerializeField] private Image blackScreen;
    [SerializeField] private float fadeDuration;

    [Header("The set value below is ignored when transitioning from sound\n" +
            "scene to regular scene due to background calculations")]
    [SerializeField] private Levels nextLevelToLoad;

    public Levels NextLevelToLoad {
        get {
            return nextLevelToLoad;
        }
        set {
            nextLevelToLoad = value;
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

    public void LoadNextLevel() {
        blackScreen.DOFade(1f, fadeDuration).OnComplete(() => SceneManager.LoadScene((int)nextLevelToLoad));
    }

    public void QuitGame() {
        blackScreen.DOFade(1f, fadeDuration).OnComplete(() => Application.Quit());
    }

}
