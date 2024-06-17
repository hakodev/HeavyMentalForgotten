using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    [SerializeField] private Image blackScreen;
    [SerializeField] private float fadeDuration;
    [SerializeField] private Levels nextLevelToLoad;

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
