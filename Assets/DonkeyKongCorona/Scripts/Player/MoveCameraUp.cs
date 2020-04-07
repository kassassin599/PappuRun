using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraUp : MonoBehaviour
{
    public float timeTakenDuringLerp = 1f;

    Vector3 cameraStartPos;
    Vector3 cameraEndPos;

    public bool isLerping;

    float timeStartedLerping;

    public void StartLerping()
    {
        isLerping = true;
        timeStartedLerping = Time.time;
        
        cameraStartPos = FindObjectOfType<Camera>().transform.position;
        cameraEndPos = new Vector3(transform.position.x, transform.position.y + 0.5f, cameraStartPos.z);
    }

    private void FixedUpdate()
    {
        if (isLerping)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            FindObjectOfType<Camera>().transform.position = Vector3.Lerp(cameraStartPos, cameraEndPos, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f)
            {
                isLerping = false;
            }
        }
    }
}
