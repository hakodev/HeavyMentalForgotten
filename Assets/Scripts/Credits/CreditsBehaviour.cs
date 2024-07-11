using UnityEngine;
using DG.Tweening;

public class CreditsBehaviour : MonoBehaviour {
    [SerializeField] private Transform creditsStartPosition;
    [SerializeField] private Transform creditsEndPosition;

    [SerializeField] private GameObject creditsText;

    [Header("How many seconds it will take for the credits to scroll to the end")]
    [SerializeField] private float scrollDuration;

    private void Start() {
        SetCreditsStartPosition();
        ScrollCredits();
    }

    private void SetCreditsStartPosition() {
        creditsText.transform.position = creditsStartPosition.position;
    }

    private void ScrollCredits() {
        creditsText.transform.DOMove(creditsEndPosition.position, scrollDuration);
    }
}
