using UnityEngine;

public class TaskAreaTriggerEvent : MonoBehaviour {
    private HandMenuController handMenuController;
    private string triggerName;
    private bool notTriggeredEntrance = true;
    private bool notTriggeredEntrance2 = true;

    // Start is called before the first frame update
    void Start() {
        handMenuController = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Left Controller").GetComponent<HandMenuController>();
        triggerName = this.gameObject.name;
    }

    // Event listener for when the trigger is entered
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            // Handle each functionality seperately
            HandleEntranceTrigger();
            HandleEntranceTrigger2();
        }
    }

    private void HandleEntranceTrigger() {
        if (triggerName == "EntranceTrigger" && handMenuController.TaskAvailable(1) && notTriggeredEntrance) {
            handMenuController.ProgressTask(1, 0);
            notTriggeredEntrance = false;
        }
    }

    private void HandleEntranceTrigger2() {
        if (triggerName == "EntranceTrigger2" && handMenuController.TaskAvailable(1) && notTriggeredEntrance2) {
            handMenuController.ProgressTask(1, 0);
            notTriggeredEntrance2 = false;
        }
    }
}
