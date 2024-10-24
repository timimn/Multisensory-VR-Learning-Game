using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WindowBehaviour : MonoBehaviour {
    private HandMenuController handMenuController;
    private XRGrabInteractable grabInteractable;
    private GameObject infoCanvas;
    private Quaternion initialRotation;
    private bool isTaskCompleted = false;

    // Start is called before the first frame update
    void Start() {
        handMenuController = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Left Controller").GetComponent<HandMenuController>();
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        infoCanvas = this.GetComponentInChildren<Canvas>().gameObject;
        initialRotation = transform.rotation;
    }
    
    // Update is called once per frame
    void Update() {
        // Checks if the window is open
        float angleDifference = Quaternion.Angle(initialRotation, transform.rotation);

        if (angleDifference >= 10f && !isTaskCompleted) {
            CompleteTask();
        }
    }

    // Complete the task
    private void CompleteTask() {
        isTaskCompleted = true;

        if (handMenuController.TaskAvailable(1, 1)) {
            handMenuController.ProgressTask(1);
        }
    }
    // Doesn't revert completed task if the user closes the window. Can be implemented later.
}
