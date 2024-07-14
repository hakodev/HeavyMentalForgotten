using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform followTarget;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float cameraDeadzoneLeft;
    [SerializeField] private float cameraDeadzoneRight;

    // not sure if these last two will be used

    //[SerializeField] private float cameraDeadzoneTop;
    //[SerializeField] private float cameraDeadzoneBottom;

    [SerializeField] private float followSpeed;

    private void Start() {
        StartCameraInBoundaries();
    }

    private void FixedUpdate() {
        ProcessFollowTargetPosition();
        FollowTarget();
    }

    private void StartCameraInBoundaries() {
        followTarget.localPosition = new Vector3(distanceFromPlayer, followTarget.transform.localPosition.y, followTarget.transform.localPosition.z);

        if(this.transform.position.x < cameraDeadzoneLeft) {
            this.transform.position = new Vector3(cameraDeadzoneLeft, this.transform.position.y, this.transform.position.z);
        }
        if(this.transform.position.x > cameraDeadzoneRight) {
            this.transform.position = new Vector3(cameraDeadzoneRight, this.transform.position.y, this.transform.position.z);
        }
    }

    private void ProcessFollowTargetPosition() {
        if(PlayerMovement.Ins.HorizontalAxis > 0) {
            followTarget.localPosition = new Vector3(distanceFromPlayer, followTarget.transform.localPosition.y, followTarget.transform.localPosition.z);
        } else if(PlayerMovement.Ins.HorizontalAxis < 0) {
            followTarget.localPosition = new Vector3(-distanceFromPlayer, followTarget.transform.localPosition.y, followTarget.transform.localPosition.z);
        }
    }

    private void FollowTarget() {
        float clampedPositionX = Mathf.Clamp(followTarget.position.x, cameraDeadzoneLeft, cameraDeadzoneRight);
        //float clampedPositionY = Mathf.Clamp(playerTransform.position.y, cameraDeadzoneBottom, cameraDeadzoneTop);

        Vector3 targetPosition = new(clampedPositionX, this.transform.position.y, this.transform.position.z);
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, followSpeed);
    }
}
