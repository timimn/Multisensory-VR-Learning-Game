using UnityEngine;

public class CollisionController : MonoBehaviour {
    [SerializeField]
    private float scaleReductionRate = 0.1f;
    [SerializeField]
    private float minScaleThreshold = 0.1f;

    private void OnParticleCollision(GameObject other) {
        ParticleSystem otherParticle = other.GetComponent<ParticleSystem>();

        // If the scale of the object is over the given threshold, scale the object down
        if (otherParticle != null && other.transform.localScale.magnitude > minScaleThreshold) {
            Collider collider = other.GetComponent<Collider>();
            
            if (collider != null) {
                float currentHeight = collider.bounds.size.y;

                other.transform.localScale -= Vector3.one * scaleReductionRate * Time.deltaTime;

                // If the scale of the object reaches the given threshold, disable it
                if (other.transform.localScale.magnitude <= minScaleThreshold) {
                    other.transform.localScale = Vector3.one * minScaleThreshold;
                    other.SetActive(false);
                }
                float newHeight = collider.bounds.size.y;

                // Adjust the position of the object based on the height difference
                float heightDifference = (currentHeight - newHeight) * 0.5f;
                Vector3 newPosition = other.transform.position;
                newPosition.y -= heightDifference;
                other.transform.position = newPosition;
            }
        }
    }
}
