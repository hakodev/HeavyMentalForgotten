using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    public static PlayerMovement Ins { get; private set; }

    private Rigidbody2D rigid2d;
    private Animator animator;
    public float HorizontalAxis { get; private set; }
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private const string PLAYER_IS_WALKING = "isWalking";

    private void Awake() {
        Ins = this;
        rigid2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        ProcessInput();
    }

    private void FixedUpdate() {
        rigid2d.velocity = new Vector2(HorizontalAxis * moveSpeed, rigid2d.velocity.y);
    }

    private void ProcessInput() {
        if(GameManager.Ins.LockMovement) {
            HorizontalAxis = 0;
            return;
        }

        HorizontalAxis = Input.GetAxisRaw("Horizontal");

        if(HorizontalAxis < 0) {
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        } else if(HorizontalAxis > 0) {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        animator.SetBool(PLAYER_IS_WALKING, HorizontalAxis != 0);
        //Debug.Log(animator.GetBool(PLAYER_IS_WALKING));
        moveSpeed = GameManager.Ins.RunMode ? runSpeed : walkSpeed; // Run speed if run mode, otherwise walk speed
    }
}
