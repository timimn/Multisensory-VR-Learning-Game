using UnityEngine;

public class SwitchBehaviour : MonoBehaviour {
    public Material emissiveMaterial;
    public Material solidMaterial;

    private HingeJoint joint;
    private GameObject targetObject;
    private Renderer objRenderer;
    private HandMenuController handMenuController;
    private bool isEmissive = false;
    private const int offAngleThreshold = 0;
    private const int onAngleThreshold = -74;

    [SerializeField]
    private bool completesSecondTask = false;
    [SerializeField]
    private bool reversed = false;

    // Start is called before the first frame update
    void Start() {
        joint = this.GetComponentInChildren<HingeJoint>();
        targetObject = this.transform.parent.transform.Find("PC_monitor").gameObject;
        objRenderer = targetObject.GetComponent<Renderer>();
        handMenuController = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Left Controller").GetComponent<HandMenuController>();
    }

    // Update is called once per frame
    void Update() {
        float jointAngle = joint.angle;

        // Normal behaviour, when the switch is right side up
        if (!reversed) {
            // Toggle the emissive material, when each threshold is exceeded
            if (isEmissive && jointAngle >= offAngleThreshold) {
                SetMaterial(solidMaterial);
                isEmissive = false;
            } else if (!isEmissive && jointAngle <= onAngleThreshold) {
                SetMaterial(emissiveMaterial);
                isEmissive = true;

                // Complete the second task, if this switch is designated as the correct one
                if (completesSecondTask && handMenuController.TaskAvailable(1)) {
                    handMenuController.ProgressTask(1);
                }
            }
        // Reversed behaviour, when the switch is physically reversed (admittedly lazy, yet efficient)
        } else {
            if (isEmissive && jointAngle <= onAngleThreshold) {
                SetMaterial(solidMaterial);
                isEmissive = false;
            } else if (!isEmissive && jointAngle >= offAngleThreshold) {
                SetMaterial(emissiveMaterial);
                isEmissive = true;
            }
        }
    }

    // Function for setting the material of the target object
    private void SetMaterial(Material material) {
        Material[] materials = objRenderer.materials;
        materials[1] = material;
        objRenderer.materials = materials;
    }
}
