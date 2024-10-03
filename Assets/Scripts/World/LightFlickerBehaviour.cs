using UnityEngine;

public class FireLightBehaviour : MonoBehaviour {
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
        theLight.intensity = Mathf.Lerp(theLight.intensity, targetIntensity, flickerSpeed);

        if (Mathf.Abs(theLight.intensity - targetIntensity) < 0.01f) {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}
