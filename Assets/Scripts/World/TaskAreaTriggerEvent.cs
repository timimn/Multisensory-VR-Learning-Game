using UnityEngine;

public class TaskAreaTriggerEvent : MonoBehaviour {
    private HandMenuController handMenuController;
    private string triggerName;
    private bool classroomEntered = false;
    private bool classroomExited = false;
    private bool maintenanceFound = false;


    // Start is called before the first frame update
    void Start() {
        handMenuController = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Left Controller").GetComponent<HandMenuController>();
        triggerName = this.gameObject.name;
    }

    // Event listener for when the trigger is entered
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            // Handle each functionality seperately
            HandleClassroomEnter();
            HandleClassroomExit();
            HandleFoundMaintenance();
        }
    }

    private void HandleClassroomEnter() {
        if (triggerName.Contains("ClassroomTrigger") && handMenuController.TaskAvailable(0, 1) && !classroomEntered) {
            classroomEntered = true;
            handMenuController.ProgressTask(0);
        }
    }

    private void HandleClassroomExit() {
        if (triggerName.Contains("ClassroomExitTrigger") && handMenuController.TaskAvailable(7, 1) && !classroomExited) {
            classroomExited = true;
            handMenuController.ProgressTask(7);
        }
    }

    private void HandleFoundMaintenance() {
        if (triggerName.Contains("MaintenanceFoundTrigger") && handMenuController.TaskAvailable(8, 1) && !maintenanceFound) {
            maintenanceFound = true;
            handMenuController.ProgressTask(8);
        }
    }
}
