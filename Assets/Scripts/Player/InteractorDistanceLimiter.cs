using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class InteractorDistanceLimiter : MonoBehaviour {
    private XRBaseInteractor interactor;
    private XRBaseInteractable currentInteractable;

    [SerializeField]
    private float cutoffDistance = 2.5f;

    // Start is called before the first frame update
    void Start() {
        interactor = this.GetComponent<XRBaseInteractor>();
        interactor.selectEntered.AddListener(OnSelectEntered);
        interactor.selectExited.AddListener(OnSelectExited);
    }

    // Update is called once per frame
    void Update() {
        // If nothing is grabbed, do nothing
        if (currentInteractable == null) return;

        float distance = Vector3.Distance(interactor.transform.position, currentInteractable.transform.position);

        // If the distance is greater than the designated cutoff distance, disconnect the interactor
        if (distance > cutoffDistance) {
            interactor.interactionManager.SelectExit((IXRSelectInteractor)interactor, (IXRSelectInteractable)currentInteractable);
            currentInteractable = null;
        }
    }

    // Event listener, for when an object is selected
    private void OnSelectEntered(SelectEnterEventArgs args) {
        currentInteractable = args.interactableObject.transform.GetComponent<XRBaseInteractable>();
    }

    // Event listener, for when an object is no longer selected
    private void OnSelectExited(SelectExitEventArgs args) {
        currentInteractable = null;
    }

    // Function for cleaning up listeners, should the object be destroyed
    private void OnDestroy() {
        interactor.selectEntered.RemoveListener(OnSelectEntered);
        interactor.selectExited.RemoveListener(OnSelectExited);
    }
}
