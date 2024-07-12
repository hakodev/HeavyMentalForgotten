using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    public static PlayerMovement Ins { get; private set; }

    private Rigidbody2D rigid2d;
    private float horizontalAxis;
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private void Awake() {
        Ins = this;
        rigid2d = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        ProcessInput();
    }

    private void FixedUpdate() {
        rigid2d.velocity = new Vector2(horizontalAxis * moveSpeed, rigid2d.velocity.y);
    }

    private void ProcessInput() {
        if(GameManager.Ins.LockMovement) {
            horizontalAxis = 0f;
            return;
        }

        horizontalAxis = Input.GetAxisRaw("Horizontal");
        moveSpeed = GameManager.Ins.RunMode ? runSpeed : walkSpeed; // Run speed if run mode, otherwise walk speed
    }
}
