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
    private int hoveringControllerCount = 0;
    private Vector3 canvasOffset;

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
    private float canvasDistance = 0f;

    // Start is called before the first frame update
    void Start() {
        gameCamera = GameObject.Find("Camera").transform;
        grabInteractable = this.gameObject.GetComponent<XRGrabInteractable>();
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
            Vector3 directionToCamera = (gameCamera.position - this.transform.position).normalized;
            Vector3 newCanvasPosition = this.transform.position + canvasOffset + directionToCamera * canvasDistance;
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
