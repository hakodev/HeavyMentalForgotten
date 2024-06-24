using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    public static PlayerMovement Ins { get; private set; }

    private Rigidbody2D rigid2d;
    private float horizontalAxis;
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [field: SerializeField] public bool RunMode { get; set; }

    private void Awake() {
        Ins = this;
        rigid2d = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        moveSpeed = RunMode ? runSpeed : walkSpeed; // Run speed if run mode, otherwise walk speed
    }

    private void FixedUpdate() {
        rigid2d.velocity = new Vector2(horizontalAxis * moveSpeed, rigid2d.velocity.y);
    }
}
