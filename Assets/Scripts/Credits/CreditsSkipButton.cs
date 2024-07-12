using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CreditsSkipButton : MonoBehaviour {
    private Image skipButtonImage;

    private void Awake() {
        skipButtonImage = GetComponent<Image>();
    }

    private void Start() {
        skipButtonImage.DOFade(1f, 1.5f);
    }

    public void SkipCredits() {
        SceneManager.LoadScene((int)Levels.MainMenu);
    }
}
