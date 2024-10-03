using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Game.Task;

public class HandMenuController : MonoBehaviour {
    public GameObject handMenu;
    public GameObject pauseMenu;

    private InputDevice leftController;
    private TextMeshProUGUI taskText;
    private CanvasGroup canvasGroup;
    private bool fadeActive = false;

    [SerializeField]
    private const float fadeSpeed = 2.5f;

    // Initialize the tasks using the custom Task class
    private List<Task> tasks = new List<Task> {
        new Task("Pick up the fire extinguisher."),
        new Task("Visit both entrances of the building.", 2),
        new Task("Activate the flashlight."),
        new Task("Discover the keys to the universe.")
    };
    
    // Start is called before the first frame update
    void Start() {
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        taskText = this.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
        canvasGroup = handMenu.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        handMenu.SetActive(false);
        UpdateTasks();
    }

    // Update is called once per frame
    void Update() {
        // Ensure the controller stays valid
        if (!leftController.isValid) {
            leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }

        // Prevent displaying the hand menu if the game is paused
        if (pauseMenu != null && pauseMenu.activeSelf) {
            handMenu.SetActive(false);
            return;
        }
        bool secondaryButtonValue;
        bool secondaryButtonPressed = leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonValue) && secondaryButtonValue;

        // If the left trigger is held, handle the menu fading in
        if (secondaryButtonPressed && !fadeActive) {
            StartCoroutine(FadeIn());
        // If the left trigger is released and the menu is still visible, handle the menu fading out
        } else if (!secondaryButtonPressed && !fadeActive && canvasGroup.alpha > 0) {
            StartCoroutine(FadeOut());
        }
    }

    // Function for updating the task list
    public void UpdateTasks() {
        taskText.text = "Tasks:\n";

        foreach (Task task in tasks) {
            if (task.completed) {
                taskText.text += $"- <color=green>{task.name}{(task.subtasksTarget > 1 ? $" ({task.subtasksTarget}/{task.subtasksTarget})" : "")}</color>\n";
            } else if (task.subtasksCompleted > 0) {
                taskText.text += $"- <color=yellow>{task.name} ({task.subtasksCompleted}/{task.subtasksTarget})</color>\n";
            } else {
                taskText.text += $"- {task.name}{(task.subtasksTarget > 1 ? $" ({task.subtasksCompleted}/{task.subtasksTarget})" : "")}\n";
            }
        }
    }

    // Function for progressing a task based on its index
    public void ProgressTask(int index) {
        if (index >= 0 && index < tasks.Count) {
            Task task = tasks[index];
            task.subtasksCompleted++;

            if (task.AllSubtasksCompleted()) {
                task.completed = true;
            }
            UpdateTasks();
        }
    }

    // Coroutine for handling the menu fading in
    private IEnumerator FadeIn() {
        fadeActive = true;
        handMenu.SetActive(true);

        // While the menu is not fully visible, increment the alpha value and wait for the next frame
        while (canvasGroup.alpha < 1) {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 1;  // Ensure correct alpha value
        fadeActive = false;
    }

    // Coroutine for handling the menu fading out
    private IEnumerator FadeOut() {
        fadeActive = true;

        // While the menu is not fully hidden, decrement the alpha value and wait for the next frame
        while (canvasGroup.alpha > 0) {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 0;
        handMenu.SetActive(false);
        fadeActive = false;
    }
}
