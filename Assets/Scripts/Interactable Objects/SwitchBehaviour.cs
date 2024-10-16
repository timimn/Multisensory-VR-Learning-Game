using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchBehaviour : MonoBehaviour {
    public Material emissiveMaterial;
    public Material solidMaterial;
    
    private HingeJoint joint;
    private GameObject targetObject;
    private Renderer objRenderer;
    private HandMenuController handMenuController;
    private LightToggleController lightToggleController;
    private int switchID;
    private static HashSet<int> switchIDs = new HashSet<int>();
    private bool isEmissive = false;
    private float elapsedTime = 0f;
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
        switchID = this.gameObject.GetInstanceID();
        if (!reversed && !completesSecondTask) switchIDs.Add(switchID); 
        handMenuController = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Left Controller").GetComponent<HandMenuController>();
        lightToggleController = this.GetComponent<LightToggleController>();
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
                HandleTaskProgression();
            } else if (!isEmissive && jointAngle <= onAngleThreshold) {
                SetMaterial(emissiveMaterial);
                isEmissive = true;
                HandleTaskReversion();

                // Complete the second task, if this switch is designated as the correct one
                if (completesSecondTask && handMenuController.TaskAvailable(1)) {
                    handMenuController.ProgressTask(1);
                    StartCoroutine(PowerCutCountdown());
                }
            }
        // Reversed behaviour, when the switch is physically reversed (admittedly lazy, yet efficient)
        } else {
            if (isEmissive && jointAngle <= onAngleThreshold) {
                SetMaterial(solidMaterial);
                isEmissive = false;
                HandleTaskProgression();
            } else if (!isEmissive && jointAngle >= offAngleThreshold) {
                SetMaterial(emissiveMaterial);
                isEmissive = true;
                HandleTaskReversion();
            }
        }
    }

    // Function for setting the material of the target object
    private void SetMaterial(Material material) {
        Material[] materials = objRenderer.materials;
        materials[1] = material;
        objRenderer.materials = materials;
    }

    // Function for handling the task progression for switches
    private void HandleTaskProgression() {
        if (!switchIDs.Contains(switchID)) {
            handMenuController.ProgressTask(4, 0);
            switchIDs.Add(switchID);

            if (handMenuController.TaskAvailable(5)) {
                StartCoroutine(WaitingCountdown());
            }
        }
    }

    // Function for handling the task reversion for switches
    private void HandleTaskReversion() {
        if (switchIDs.Contains(switchID)) {
            handMenuController.RevertTask(4, 0);
            switchIDs.Remove(switchID);
        }
    }

    // Coroutine for handling the power cut countdown
    private IEnumerator PowerCutCountdown() {
        elapsedTime = 0f;

        // Wait for 5 seconds before activating the script
        while (elapsedTime < 5f) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        lightToggleController.ToggleLights();
        handMenuController.ProgressTask(2);
    }

    // Coroutine for handling the waiting countdown
    private IEnumerator WaitingCountdown() {
        elapsedTime = 0f;

        // Wait for 1 minute before progressing the task
        while (elapsedTime < 60f) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        handMenuController.ProgressTask(5);
    }
}
