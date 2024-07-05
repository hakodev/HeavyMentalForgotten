using UnityEngine;
using UnityEngine.Events;

public class DialogueEventSystem : MonoBehaviour {
    public static DialogueEventSystem Ins { get; private set; }

    [Header("Add whatever you want the game to do upon interacting with\n" +
            "something or someone. Like playing a sound, changing camera\n" +
            "position, destroying an object etc.")]
    [SerializeField] private UnityEvent onInteraction;

    public UnityEvent OnInteraction {
        get { return onInteraction; }
    }

    private void Awake() {
        Ins = this;
    }
}
