using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;

public class InteractionPromptController : MonoBehaviour {
    private Transform gameCamera;
    private XRGrabInteractable grabInteractable;
    private GameObject infoCanvas;
    private TextMeshProUGUI infoText;
    private Vector3 canvasOffset;
    private Collider objectCollider;
    private int hoveringControllerCount = 0;

    [SerializeField]
    private string objectName = "";
    [SerializeField]
    private string objectDescription = "";
    [SerializeField]
    private float canvasOffsetX = 0f;
    [SerializeField]
    private float canvasOffsetY = 0f;
    [SerializeField]
    private float canvasOffsetZ = 0f;
    [SerializeField]
    private float canvasOffsetForward = 0f;

    // Start is called before the first frame update
    void Start() {
        gameCamera = GameObject.Find("Camera").transform;
        grabInteractable = this.gameObject.GetComponent<XRGrabInteractable>();
        objectCollider = this.gameObject.GetComponent<Collider>();
        infoCanvas = this.gameObject.GetComponentInChildren<Canvas>().gameObject;
        infoText = infoCanvas.GetComponentInChildren<TextMeshProUGUI>();
        grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        grabInteractable.hoverExited.AddListener(OnHoverExited);
        canvasOffset = new Vector3(canvasOffsetX, canvasOffsetY, canvasOffsetZ);
    }

    // Update is called once per frame
    void Update() {
        // If the object is being hovered over, update the canvas to face the camera
        if (hoveringControllerCount > 0) {
            // Calculate vectors for:
            // Top point of the collider relative to world coordinates
            // Camera direction relative to collider top point
            // Updated canvas position relative to collider top point and defined offsets
            Vector3 directionToCamera = Vector3.zero;
            Vector3 colliderTop = objectCollider.bounds.center + new Vector3(0, objectCollider.bounds.extents.y, 0);
            if (canvasOffsetForward != 0f) directionToCamera = (gameCamera.position - colliderTop).normalized;
            Vector3 newCanvasPosition = colliderTop + canvasOffset + (directionToCamera * canvasOffsetForward);

            // Update canvas position and turn it towards the camera
            infoCanvas.transform.position = newCanvasPosition;
            infoCanvas.transform.LookAt(gameCamera.position);
            infoCanvas.transform.Rotate(0, 180, 0);
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
}
