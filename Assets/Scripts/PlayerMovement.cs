using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    private Rigidbody2D rigid2d;
    private float horizontalAxis;
    [SerializeField] private float moveSpeed;

    private void Awake() {
        rigid2d = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        horizontalAxis = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate() {
        rigid2d.velocity = new Vector2(horizontalAxis * moveSpeed, rigid2d.velocity.y);
    }
}
