using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandMenuTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    private bool isTimerRunning = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    // Updates the timer if the timer is running.
    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime % 60F);
            timerText.text = string.Format("TIME: {0:00}:{1:00}", minutes, seconds);
        }
    }
}
