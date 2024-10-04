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
        // Implement different functionality based on trigger name
        if (triggerName == "EntranceTrigger" && notTriggeredEntrance && other.CompareTag("Player")) {
            handMenuController.ProgressTask(1);
            notTriggeredEntrance = false;
        } else if (triggerName == "EntranceTrigger2" && notTriggeredEntrance2 && other.CompareTag("Player")) {
            handMenuController.ProgressTask(1);
            notTriggeredEntrance2 = false;
        }
    }
}
