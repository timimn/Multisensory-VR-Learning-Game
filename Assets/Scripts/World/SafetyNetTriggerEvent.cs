using UnityEngine;

public class SafetyNetTriggerEvent : MonoBehaviour {
    private Transform gameCamera;
    
    [SerializeField]
    private float teleportDistance = 1f;

    // Start is called before the first frame update
    void Start() {
        gameCamera = GameObject.Find("Camera").transform;
    }

    // Event listener for when the trigger is entered
    private void OnTriggerEnter(Collider other) {
        // If a player falls through the ground, teleport them to the spawn point
        if (other.CompareTag("Player")) {
            other.transform.position = new Vector3(0f, 5f, 0f);
            Debug.Log("Player fell through the map.");
        } else {
            // Teleport objects that have fallen through the ground in front of the player
            other.transform.position = gameCamera.position + gameCamera.forward * teleportDistance;
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null && !rb.isKinematic) rb.velocity = Vector3.zero;  // Reset the velocity of the object
            Debug.Log($"{other.gameObject.name} fell through the map.");
        }
    }
}
