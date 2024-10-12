using UnityEngine;
using System.Collections.Generic;

public class LightToggleController : MonoBehaviour {
    public Material emissiveMaterial;
    public Material solidMaterial;

    private bool isEmissive = true;
    
    public void ToggleLights() {
        // Initialize an array of all objects tagged as "Toggleable"
        GameObject[] toggleableObjects = GameObject.FindGameObjectsWithTag("Toggleable");

        // Toggle all light components and emissive materials
        foreach (GameObject obj in toggleableObjects) {
            Light theLight = obj.GetComponent<Light>();

            if (theLight != null) theLight.enabled = !theLight.enabled;
            if (obj.transform.parent == null) continue;

            // Get the first child of the parent, and its renderer component
            Transform objTransform = obj.transform.parent.GetChild(0).transform;
            Renderer objRenderer = objTransform.GetComponent<Renderer>();

            if (objRenderer == null) continue;

            Material[] materials = objRenderer.materials;

            // Toggle the material of a ceiling light
            if (objTransform.gameObject.name.Contains("ceilingLight")) {
                materials[1] = isEmissive ? solidMaterial : emissiveMaterial;
                objRenderer.materials = materials;
            }
        }
        isEmissive = !isEmissive;
    }
}
