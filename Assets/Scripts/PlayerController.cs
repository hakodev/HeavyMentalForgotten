using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveSpeed;
    private float horizontalAxis;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        horizontalAxis = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate() {
        rb2d.velocity = new Vector2(horizontalAxis * moveSpeed, rb2d.velocity.y);
    }
}
