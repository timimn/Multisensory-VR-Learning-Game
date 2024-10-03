using UnityEngine;

public class FireLightBehaviour : MonoBehaviour {
    private Light pointLight;
    private float targetIntensity;
    
    [SerializeField]
    private float minIntensity = 0.25f;
    [SerializeField]
    private float maxIntensity = 3f;
    [SerializeField]
    private float flickerSpeed = 0.3f;

    // Start is called before the first frame update
    void Start() {
        pointLight = GetComponent<Light>();
        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }

    // Update is called once per frame
    void Update() {
        pointLight.intensity = Mathf.Lerp(pointLight.intensity, targetIntensity, flickerSpeed);

        if (Mathf.Abs(pointLight.intensity - targetIntensity) < 0.01f) {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}
