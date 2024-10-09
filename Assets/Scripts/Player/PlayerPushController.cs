using UnityEngine;

public class PlayerPushController : MonoBehaviour {
    private CharacterController characterController;

    [SerializeField]
    private float pushStrength = 1.75f;

    // Start is called before the first frame update
    void Start() {
        characterController = this.GetComponent<CharacterController>();
    }

    // Event listener, for when the character controller's collider hits another
    void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody rb = hit.collider.attachedRigidbody;

        // Only push objects that contain a non-kinematic rigidbody
        if (rb != null && !rb.isKinematic) {
            // Push the object in the direction the player is moving in
            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z).normalized;
            rb.AddForce(pushDirection * pushStrength, ForceMode.Impulse);
        }
    }
}
