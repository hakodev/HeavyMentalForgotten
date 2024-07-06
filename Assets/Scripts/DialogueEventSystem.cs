using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DialogueEventSystem : MonoBehaviour {
    // TO DO: Implement proper end delay

    public static DialogueEventSystem Ins { get; private set; }

    [Header("Add whatever you want the game to do upon interacting with\n" +
            "something or someone. Like playing a sound, changing camera\n" +
            "position, destroying an object etc.")]
    [SerializeField] private UnityEvent onInteraction;

    private void Awake() {
        Ins = this;
    }

    public void InvokeEvent(float startDelay = 0f, float endDelay = 0f) {
        Invoke(nameof(InvokeEventActual), startDelay);
        //if(endDelay > 0f) {
        //    StartCoroutine(WaitGivenSeconds(endDelay));
        //}
    }

    private void InvokeEventActual() {
        onInteraction?.Invoke();
    }

    //private IEnumerator WaitGivenSeconds(float delay) {
    //    yield return new WaitForSeconds(delay);
    //}
}
