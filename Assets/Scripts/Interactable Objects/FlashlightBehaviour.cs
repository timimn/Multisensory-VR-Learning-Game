using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;

public class FlashlightBehaviour : MonoBehaviour {
    public Transform gameCamera;
    public Light flashLight;
    public XRGrabInteractable grabInteractable;
    public XRBaseInteractor leftControllerInteractor;
    public XRBaseInteractor rightControllerInteractor;
    public GameObject infoCanvas;

    private InputDevice leftController;
    private InputDevice rightController;
    private HandMenuController handMenuController;
    private TextMeshProUGUI infoText;
    private bool rightControllerGrabbing = true;
    private bool firstTimeGrabbed = false;
    private int hoveringControllerCount = 0;
    private Vector3 canvasOffset = new Vector3(0f, 0.2f, 0f);
    private const string objectName = "Flashlight";
    private const string objectDescription = "Grab: [<sprite=8>/<sprite=10>]";

    // Start is called before the first frame update
    void Start() {
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        handMenuController = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Left Controller").GetComponent<HandMenuController>();
        infoText = infoCanvas.GetComponentInChildren<TextMeshProUGUI>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        grabInteractable.hoverExited.AddListener(OnHoverExited);
    }

    // Update is called once per frame
    void Update() {
        // Ensure the controllers stay valid
        if (!leftController.isValid) {
            leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }

        if (!rightController.isValid) {
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        // If the object is being hovered over, update the canvas to face the camera
        if (hoveringControllerCount > 0) {
            infoCanvas.transform.position = this.transform.position + canvasOffset;
            infoCanvas.transform.rotation = Quaternion.LookRotation(gameCamera.forward);
        } else if (grabInteractable.isSelected) {
            float triggerValue = 0f;
            bool triggerPressed = false;

            // Resolve, which trigger should activate the light
            if (rightControllerGrabbing) {
                triggerPressed = rightController.TryGetFeatureValue(CommonUsages.trigger, out triggerValue) && triggerValue > 0.5f;
            } else {
                triggerPressed = leftController.TryGetFeatureValue(CommonUsages.trigger, out triggerValue) && triggerValue > 0.5f;
            }

            // Complete a demonstration task
            if (!firstTimeGrabbed && triggerPressed) {
                handMenuController.CompleteTask(2);
                firstTimeGrabbed = true;
            }
            // If the corresponding trigger is held, activate the light, otherwise deactivate it
            flashLight.enabled = triggerPressed;
        } else if (flashLight.enabled) {
            flashLight.enabled = false;
        }
    }

    // Event listener, for when the object is hovered over
    private void OnHoverEntered(HoverEnterEventArgs args) {
        hoveringControllerCount++;

        // If the object is being hovered over without being grabbed, display object information
        if (hoveringControllerCount > 0 && !grabInteractable.isSelected) {
            infoText.text = $"{objectName}\n{objectDescription}";
            infoCanvas.SetActive(true);
        }
    }

    // Event listener, for when the object is no longer hovered over
    private void OnHoverExited(HoverExitEventArgs args) {
        hoveringControllerCount--;

        if (hoveringControllerCount < 1) {
            infoCanvas.SetActive(false);
        }
    }

    // Event listener, for when the object is selected
    private void OnSelectEntered(SelectEnterEventArgs args) {
        // Get the interactor that selected the object
        XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;

        // Resolve, which controller is grabbing the object
        if (interactor == rightControllerInteractor) {
            rightControllerGrabbing = true;
        } else if (interactor == leftControllerInteractor) {
            rightControllerGrabbing = false;
        }
        infoCanvas.SetActive(false);
    }

    // Function for cleaning up listeners, should the object be destroyed
    private void OnDestroy() {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.hoverEntered.RemoveListener(OnHoverEntered);
        grabInteractable.hoverExited.RemoveListener(OnHoverExited);
    }
}