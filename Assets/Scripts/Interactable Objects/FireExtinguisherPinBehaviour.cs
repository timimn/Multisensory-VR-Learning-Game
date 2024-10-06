using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FireExtinguisherPinBehaviour : MonoBehaviour {
    private XRGrabInteractable grabInteractable;
    private FixedJoint joint;

    // Start is called before the first frame update
    void Start() {
        joint = this.GetComponent<FixedJoint>();
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
    }

    // Event listener, for when the object is selected
    private void OnSelectEntered(SelectEnterEventArgs args) {
        Destroy(joint);  // Destroy the connected joint to allow free movement
    }

    // Function for cleaning up listeners, should the object be destroyed
    private void OnDestroy() {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
    }
}
