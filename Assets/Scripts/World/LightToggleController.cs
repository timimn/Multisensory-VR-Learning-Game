using UnityEngine;
using System.Collections.Generic;

public class LightToggleController : MonoBehaviour {
    public void ToggleLights() {
        // Initialize an array of all objects tagged as "Toggleable"
        GameObject[] toggleableObjects = GameObject.FindGameObjectsWithTag("Toggleable");

        // Toggle all light components
        foreach (GameObject obj in toggleableObjects) {
            Light theLight = obj.GetComponent<Light>();
            if (theLight != null) theLight.enabled = !theLight.enabled;
        }
    }
}
