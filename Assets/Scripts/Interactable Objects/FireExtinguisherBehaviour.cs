using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;

public class FireExtinguisherBehaviour : MonoBehaviour {
    public XRBaseInteractor leftControllerInteractor;
    public XRBaseInteractor rightControllerInteractor;
    public ParticleSystem foamParticleSystem;
    
    private InputDevice leftController;
    private InputDevice rightController;
    private Transform gameCamera;
    private HandMenuController handMenuController;
    private XRGrabInteractable grabInteractable;
    private GameObject infoCanvas;
    private bool rightControllerGrabbing = true;
    private bool pinRemoved = false;

    // Start is called before the first frame update
    void Start() {
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        gameCamera = GameObject.Find("Camera").transform;
        handMenuController = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Left Controller").GetComponent<HandMenuController>();
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        infoCanvas = this.GetComponentInChildren<Canvas>().gameObject;
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
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
        
        if (grabInteractable.isSelected) {
            float triggerValue = 0f;
            bool triggerPressed = false;

            // Resolve, which trigger should activate the particle system
            if (rightControllerGrabbing) {
                triggerPressed = rightController.TryGetFeatureValue(CommonUsages.trigger, out triggerValue) && triggerValue > 0.5f;
            } else {
                triggerPressed = leftController.TryGetFeatureValue(CommonUsages.trigger, out triggerValue) && triggerValue > 0.5f;
            }

            // If the corresponding trigger is held, activate the particle system, otherwise deactivate it
            if (pinRemoved && triggerPressed) {
                if (!foamParticleSystem.isPlaying) foamParticleSystem.Play();
            } else {
                if (foamParticleSystem.isPlaying) foamParticleSystem.Stop();
            }
        } else if (foamParticleSystem.isPlaying) {
            foamParticleSystem.Stop();
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

    // Event listener, for when the collider stops colliding with another
    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.name == "Fire_Extinguisher_Pin") {
            pinRemoved = true;
        }
    }

    // Function for cleaning up listeners, should the object be destroyed
    private void OnDestroy() {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
    }
}
