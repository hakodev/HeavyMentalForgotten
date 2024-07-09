using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour {
    [Header("Buttons to highlight")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button howToPlayBackButton;
    [SerializeField] private Button settingsBackButton;

    [Header("Menu groups")]
    [SerializeField] private CanvasGroup mainMenuGroup;
    [SerializeField] private CanvasGroup howToPlayGroup;
    [SerializeField] private CanvasGroup settingsGroup;

    [Header("Set how long it will take for fade ins/outs,\n" +
            "will overwrite fade duration in Game Manager.")]
    [SerializeField] private float fadeDuration;

    private void Start() {
        GameManager.Ins.SetFadeDuration(this.fadeDuration);
        playButton.Select(); // So play button is highlighted for arrow key navigation
    }

    public void PlayButton() {
        mainMenuGroup.interactable = false;
        GameManager.Ins.LoadNextLevel(Levels.Introduction);
    }

    public void HowToPlayButton() {
        mainMenuGroup.interactable = false;
        mainMenuGroup.DOFade(0f, fadeDuration).OnComplete(() => {
            mainMenuGroup.gameObject.SetActive(false);
            howToPlayGroup.gameObject.SetActive(true);
            howToPlayGroup.alpha = 0f;
            howToPlayGroup.DOFade(1f, fadeDuration);
        });
        howToPlayGroup.interactable = true;
        howToPlayBackButton.Select(); // For enter key interaction
    }

    public void SettingsButton() {
        mainMenuGroup.interactable = false;
        mainMenuGroup.DOFade(0f, fadeDuration).OnComplete(() => {
            mainMenuGroup.gameObject.SetActive(false);
            settingsGroup.gameObject.SetActive(true);
            settingsGroup.alpha = 0f;
            settingsGroup.DOFade(1f, fadeDuration);
        });
        settingsGroup.interactable = true;
        settingsBackButton.Select(); // Ditto
    }

    public void CreditsButton() {
        mainMenuGroup.interactable = false;
        GameManager.Ins.LoadNextLevel(Levels.Credits);
    }

    public void BackToMenuFromHowToPlay() {
        howToPlayGroup.interactable = false;
        howToPlayGroup.DOFade(0f, fadeDuration).OnComplete(() => {
            howToPlayGroup.gameObject.SetActive(false);
            mainMenuGroup.gameObject.SetActive(true);
            mainMenuGroup.alpha = 0f;
            mainMenuGroup.DOFade(1f, fadeDuration);
        });
        mainMenuGroup.interactable = true;
        playButton.Select();
    }

    public void BackToMenuFromSettings() {
        settingsGroup.interactable = false;
        settingsGroup.DOFade(0f, fadeDuration).OnComplete(() => {
            settingsGroup.gameObject.SetActive(false);
            mainMenuGroup.gameObject.SetActive(true);
            mainMenuGroup.alpha = 0f;
            mainMenuGroup.DOFade(1f, fadeDuration);
        });
        mainMenuGroup.interactable = true;
        playButton.Select();
    }

    public void QuitButton() {
        mainMenuGroup.interactable = false;
        GameManager.Ins.QuitGame();
    }
}
