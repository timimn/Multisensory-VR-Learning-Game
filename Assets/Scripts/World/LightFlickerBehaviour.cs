using UnityEngine;

public class LightFlickerBehaviour : MonoBehaviour {
    private Light theLight;
    private float targetIntensity;
    
    [SerializeField]
    private float minIntensity = 0.25f;
    [SerializeField]
    private float maxIntensity = 3f;
    [SerializeField]
    private float flickerSpeed = 0.3f;

    // Start is called before the first frame update
    void Start() {
        theLight = GetComponent<Light>();
        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }

    // Update is called once per frame
    void Update() {
        // Modify intensity based on the scale of the parent object
        float parentScaleAverage = (transform.parent.localScale.x + transform.parent.localScale.y + transform.parent.localScale.z) / maxIntensity;
        float scaledIntensity = Mathf.Clamp(parentScaleAverage, 0.1f, 10f);

        // Adjust the light intensity, using interpolation
        theLight.intensity = Mathf.Lerp(theLight.intensity, targetIntensity * scaledIntensity, flickerSpeed);

        // If the light intensity is close to the target intensity (scaled), set a new target intensity
        if (Mathf.Abs(theLight.intensity - (targetIntensity * scaledIntensity)) < 0.01f) {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}
