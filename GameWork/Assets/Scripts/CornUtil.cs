using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class containing methods used to perform actions on the corn.
public class CornUtil : MonoBehaviour {
    
    const float startLaunch = 0.4f;
    public CornManager cm;
    public Vector3 initialPosition = new Vector3(4.3f, -6.3f);
    public Vector3 centerPosition = new Vector3(0.0f, 0.3f);
    public Vector3 endingPosition;
    public Vector3 startingPosition;
    public float launchTime = startLaunch;
    private float startTime;

    // On start get the initial position, start time, and ending position.
    private void Start () {
        startingPosition = initialPosition;
        startTime = Time.time;
        endingPosition = centerPosition;
    }

    // Set the initial position and initial time for use in slerp.
    public void InitializeStartingPosition (Vector3 newStartPosition, Vector3 newEndPosition, float newLaunchTime = startLaunch) {
        
        // Setting initial position and starting time.
        startingPosition = newStartPosition;
        startTime = Time.time;

        // Set launching speed and ending position.
        launchTime = newLaunchTime;
        endingPosition = newEndPosition;
    }

    // Function that moves corn position based on difference between start time and current time.
    public void ExtrapolateCurrentPosition (Vector3 endingPosition) {

        // Get the center position moved down to add circular look (apparently).
        Vector3 center = (endingPosition - startingPosition) * 0.5f;
        center -= new Vector3(0, 1);

        // Get start and end positions relative to the center.
        Vector3 riseRelCenter = startingPosition - center;
        Vector3 setRelCenter = endingPosition - center;

        // Compute the fraction of movement completed based on launch time. Then perform Slerp.
        float fracComplete = (Time.time - startTime) / launchTime;
        cm.Corn.transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        cm.Corn.transform.position += center;
    }
}
