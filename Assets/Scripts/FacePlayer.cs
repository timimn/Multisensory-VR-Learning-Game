using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Transform lookAtPlayer;

    // Start is called before the first frame update
    private void Start()
    { 
        lookAtPlayer = GameObject.Find("Camera").transform;      
    }

    // Update is called once per frame
    void Update()
    {
        if (lookAtPlayer != null)
        {
            // If the main camera is found, this calculates the direction 
            // from the floating window to the player
            Vector3 direction = lookAtPlayer.position - transform.position;

            // Makes the game object locked to the horizontal plane
            Vector3 horizLockedDir = Vector3.ProjectOnPlane(direction, Vector3.up);

            // Game object faces the target
            Quaternion targetRotation = Quaternion.LookRotation(-horizLockedDir);
            transform.rotation = targetRotation;
        }
    }

}
