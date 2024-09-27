using UnityEngine;
using UnityEngine.XR;
using System.Collections;

public class HandMenuController : MonoBehaviour {
    public GameObject handMenu;
    public GameObject pauseMenu;

    private InputDevice leftHandDevice;
    private CanvasGroup canvasGroup;
    private bool fadeActive = false;
    private const float fadeSpeed = 2.5f;
    
    // Start is called before the first frame update
    void Start() {
        leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        canvasGroup = handMenu.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        handMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        // Ensure the device stays valid
        if (!leftHandDevice.isValid) {
            leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }

        // Prevent displaying the hand menu if the game is paused
        if (pauseMenu != null && pauseMenu.activeSelf) {
            handMenu.SetActive(false);
            return;
        }
        float triggerValue = 0f;
        bool leftTriggerPressed = leftHandDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue) && triggerValue > 0.5f;

        // If the left trigger is held, handle the menu fading in
        if (leftTriggerPressed && !fadeActive) {
            StartCoroutine(FadeIn());
        // If the left trigger is released and the menu is still visible, handle the menu fading out
        } else if (!leftTriggerPressed && !fadeActive && canvasGroup.alpha > 0) {
            StartCoroutine(FadeOut());
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
