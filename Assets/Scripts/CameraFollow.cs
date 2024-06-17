using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float cameraDeadzoneLeft;
    [SerializeField] private float cameraDeadzoneRight;

    // not sure if these last two will be used

    //[SerializeField] private float cameraDeadzoneTop;
    //[SerializeField] private float cameraDeadzoneBottom;

    private void LateUpdate() {
        FollowPlayer();
    }

    private void FollowPlayer() {
        float playerPositionX = Mathf.Clamp(playerTransform.position.x, cameraDeadzoneLeft, cameraDeadzoneRight);
        //float playerPositionY = Mathf.Clamp(playerTransform.position.y, cameraDeadzoneBottom, cameraDeadzoneTop);

        this.transform.position = new Vector3(playerPositionX, this.transform.position.y, this.transform.position.z);
    }
}
