using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {
    public GameObject pauseMenu;
    public GameObject teleportInteractor;
    
    private Transform gameCamera;
    private InputDevice leftController;
    private float offsetZ;
    private bool menuVisible = false;
    private bool previousButtonState = false;

    [SerializeField]
    private float followSpeed = 2f;
    [SerializeField]
    private float rotationSpeed = 5f;
    [SerializeField]
    private float gapDistanceModifier = 0.25f;

    // Start is called before the first frame update
    void Start() {
        gameCamera = GameObject.Find("Main Camera").transform;
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        offsetZ = pauseMenu.transform.localPosition.z;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        // Ensure the controller stays valid
        if (!leftController.isValid) {
            leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }
        bool menuButtonValue;
        bool menuButtonPressed = leftController.TryGetFeatureValue(CommonUsages.menuButton, out menuButtonValue) && menuButtonValue;

        // Toggle the menu only when the state of the left menu button changes
        if (menuButtonPressed && !previousButtonState) {
            menuVisible = !menuVisible;
            pauseMenu.SetActive(menuVisible);

            // Enable / disable teleporting
            if (menuVisible) {
                DisableComponents(teleportInteractor);
            } else {
                EnableComponents(teleportInteractor);
            }
            Time.timeScale = menuVisible ? 0f : 1f;  // Pause the game when the menu is on, otherwise unpause
        }
        previousButtonState = menuButtonPressed;

        if (menuVisible) {
            PositionMenu();
        }
    }


    // Function for disabling all components of a given GameObject
    public void DisableComponents(GameObject obj) {
        Component[] components = obj.GetComponents<Component>();

        foreach (Component component in components) {
            if (component is Behaviour behaviour) {
                behaviour.enabled = false;
            }
        }
    }

    // Function for enabling all components of a given GameObject
    public void EnableComponents(GameObject obj) {
        Component[] components = obj.GetComponents<Component>();

        foreach (Component component in components) {
            if (component is Behaviour behaviour) {
                behaviour.enabled = true;
            }
        }
    }

    // Callback function for the resume button
    public void GameResumeCallback() {
        menuVisible = false;
        pauseMenu.SetActive(menuVisible);
        EnableComponents(teleportInteractor);
        Time.timeScale = 1f;
    }

    // Callback function for the restart button
    public void GameRestartCallback() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    // Callback function for the quit button
    public void GameQuitCallback() {
        Application.Quit();

        // Preprocessor directive for exiting play mode in editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // Function to correctly position the menu, depending on the position of the camera
    private void PositionMenu() {
        // Position the menu in front of the camera, relative to the value of its z-axis in the editor
        Vector3 targetPosition = gameCamera.position + gameCamera.forward * offsetZ;
        targetPosition.y = gameCamera.position.y;  // Keep the menu leveled to the height of the camera

        // Detect objects between the camera and the position of the menu
        RaycastHit hit;
        Vector3 directionToMenu = (targetPosition - gameCamera.position).normalized;
        float distanceToMenu = Vector3.Distance(gameCamera.position, targetPosition);
        
        // If an object is detected, adjust the position of the menu
        if (Physics.Raycast(gameCamera.position, directionToMenu, out hit, distanceToMenu)) {
            targetPosition = hit.point - directionToMenu * gapDistanceModifier;
        }
        pauseMenu.transform.position = Vector3.Lerp(pauseMenu.transform.position, targetPosition, Time.unscaledDeltaTime * followSpeed);

        // Keep the rotation of the menu upright, and horizontally leveled using interpolation
        Vector3 lookDirection = gameCamera.forward;
        lookDirection.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        pauseMenu.transform.rotation = Quaternion.Slerp(pauseMenu.transform.rotation, targetRotation, Time.unscaledDeltaTime * rotationSpeed);
    }
}
