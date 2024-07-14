using UnityEngine;

public class FootstepAudio : MonoBehaviour {
    private AudioSource footstepPlayer;
    [SerializeField] private AudioClip[] footstepSounds;

    private void Awake() {
        footstepPlayer = GetComponent<AudioSource>();
    }

    public void PlayFootstepSound() {
        int footstepID = Random.Range(0, footstepSounds.Length);

        footstepPlayer.PlayOneShot(footstepSounds[footstepID]);
    }
}
