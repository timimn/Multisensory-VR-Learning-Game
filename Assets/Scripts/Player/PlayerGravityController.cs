using UnityEngine;

public class PlayerGravityController : MonoBehaviour {
    private CharacterController characterController;
    private Vector3 velocity;

    [SerializeField]
    private float downwardForce = -1f;

    // Start is called before the first frame update
    void Start() {
        characterController = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        // If the player is grounded, apply a small constant downforce, otherwise apply gravity
        if (characterController.isGrounded) {
            velocity.y = downwardForce;
        } else {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }
        characterController.Move(velocity * Time.deltaTime);
    }
}
