using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Game.Task;

public class HandMenuController : MonoBehaviour {
    public GameObject pauseMenu;

    private InputDevice leftController;
    private GameObject handMenu;
    private TextMeshProUGUI taskText;
    private InputFeatureUsage<bool> resolvedButton;
    private CanvasGroup canvasGroup;
    private bool fadeActive = false;
    private int currentTaskIndex = 0;
    private int currentSceneIndex = 1;

    [SerializeField]
    private float fadeSpeed = 2.5f;

    // Initialize the tasks using the custom Task class
    private Dictionary<int, List<Task>> tasks = new Dictionary<int, List<Task>> {
        {
            1, new List<Task> {
                new Task("Enter classroom 202."),
                new Task("Open a window."),
                new Task("Turn on PC 1."),
                new Task("Get some work done."),
                new Task("Locate a flashlight."),
                new Task("Turn off running equipment.", new List<Subtask> {
                    new Subtask("Turn off PCs.", 9)
                }),
                new Task("Wait in the classroom for the power to come back on for 1 minute."),
                new Task("Leave the classroom."),
                //new Task("Look for people in need of assistance.", new List<Subtask> {
                //    new Subtask("Check nearby elevators.", 2)
                //}),
                new Task("Look around for maintenance staff's phone number."),
                new Task("Dial the phone number.")
            }
        },
        {
            2, new List<Task> {
                new Task("Fire response placeholder task."),
            }
        },
        {
            3, new List<Task> {
                new Task("Gas hazard placeholder task."),
            }
        }
    };

    // Awake is called when an enabled script instance is being loaded
    void Awake() {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log($"Loaded scene {currentSceneIndex}.");
    }
    
    // Start is called before the first frame update
    void Start() {
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        handMenu = this.gameObject.GetComponentInChildren<Canvas>().gameObject;
        taskText = this.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
        resolvedButton = ButtonAvailable(CommonUsages.primaryButton) ? CommonUsages.primaryButton : CommonUsages.primary2DAxisClick;
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
        bool primaryButtonValue;
        bool primaryButtonPressed = leftController.TryGetFeatureValue(resolvedButton, out primaryButtonValue) && primaryButtonValue;

        // If the left trigger is held, handle the menu fading in
        if (primaryButtonPressed && !fadeActive) {
            StartCoroutine(FadeIn());
        // If the left trigger is released and the menu is still visible, handle the menu fading out
        } else if (!primaryButtonPressed && !fadeActive && canvasGroup.alpha > 0) {
            StartCoroutine(FadeOut());
        }
    }

    // Function for updating the task list
    public void UpdateTasks() {
        taskText.text = "Tasks:\n";

        // Display any previously completed tasks
        for (int i = 0; i < currentTaskIndex; i++) {
            Task task = tasks[currentSceneIndex][i];

            if (task.completed) {
                taskText.text += $"> <color=green>{task.name}</color>\n";
            }
        }

        // Display the currently active task
        if (currentTaskIndex < tasks[currentSceneIndex].Count) {
            Task task = tasks[currentSceneIndex][currentTaskIndex];

            if (task.completed) {
                taskText.text += $"> <color=green>{task.name}</color>\n";
            } else {
                string taskColor = (task.SubtasksPartlyCompleted()) ? "yellow" : "white";
                taskText.text += $"> <color={taskColor}>{task.name}</color>\n";

                // If the task consists of subtasks, display them as well
                if (task.subtasks != null) {
                    foreach (Subtask subtask in task.subtasks) {
                        string subtaskColor = subtask.completed ? "green" : (subtask.partsCompleted > 0 ? "yellow" : "white");
                        int formattedPartsCompleted = Math.Min(subtask.partsCompleted, subtask.parts);
                        string progress = subtask.parts > 1 ? $" ({formattedPartsCompleted}/{subtask.parts})" : "";
                        taskText.text += $"  - <color={subtaskColor}>{subtask.name}{progress}</color>\n";
                    }
                }
            }
        }
    }

    // Function for progressing a task based on its index
    public void ProgressTask(int index, int subtaskIndex = -1) {
        Task task = tasks[currentSceneIndex][index];

        // If the task consists of subtasks, progress them first, otherwise just complete the task
        if (subtaskIndex > -1 && subtaskIndex < task.subtasks?.Count) {
            Subtask subtask = task.subtasks[subtaskIndex];
            subtask.partsCompleted++;

            if (subtask.AllPartsCompleted()) subtask.completed = true;

            if (task.AllSubtasksCompleted()) {
                task.completed = true;
                if (currentTaskIndex + 1 < tasks[currentSceneIndex].Count) currentTaskIndex++;
            }
        } else {
            task.completed = true;
            if (currentTaskIndex + 1 < tasks[currentSceneIndex].Count) currentTaskIndex++;
        }
        UpdateTasks();  // Display updated information on the task list
    }

    // Function for reverting a task based on its index
    public void RevertTask(int index, int subtaskIndex) {
        Task task = tasks[currentSceneIndex][index];

        // Only revert the given subtask
        if (subtaskIndex > -1 && subtaskIndex < task.subtasks?.Count) {
            Subtask subtask = task.subtasks[subtaskIndex];
            
            // If the number of completed parts would go negative, increase the total parts instead
            if (subtask.partsCompleted - 1 < 0) {
                subtask.SetParts(subtask.parts + 1);
            } else {
                subtask.partsCompleted--;
            }
        }
        UpdateTasks();
    }

    // Function for checking if a task is available
    public bool TaskAvailable(int index, int sceneIndex) {
        return (index == currentTaskIndex && sceneIndex == currentSceneIndex);
    }

    // Function for checking if a button is available on the controller
    private bool ButtonAvailable(InputFeatureUsage<bool> button) {
        bool buttonValue;
        return leftController.TryGetFeatureValue(button, out buttonValue);
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
