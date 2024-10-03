using UnityEngine;

public class SafetyNetTriggerEvent : MonoBehaviour {
    private Transform gameCamera;
    
    [SerializeField]
    private float teleportDistance = 1f;

    // Start is called before the first frame update
    void Start() {
        gameCamera = GameObject.Find("Main Camera").transform;
    }

    // Event listener for when the trigger is entered
    private void OnTriggerEnter(Collider other) {
        // Teleport objects that have fallen through the ground in front of the player
        other.transform.position = gameCamera.position + gameCamera.forward * teleportDistance;
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null) rb.velocity = Vector3.zero;
    }
}
