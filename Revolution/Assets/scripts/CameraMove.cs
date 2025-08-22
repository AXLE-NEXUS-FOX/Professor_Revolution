using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform[] pathPoints; // Assign waypoints in the Inspector
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;
    public float[] rollAngles; // Roll in degrees for each path point

    private int currentPoint = 0;
    private float progress = 0f;

    void Start()
    {
        if (pathPoints.Length > 0)
        {
            transform.position = pathPoints[0].position;
            transform.rotation = pathPoints[0].rotation;
        }
    }

    void Update()
    {
        if (pathPoints.Length < 2 || currentPoint >= pathPoints.Length - 1) return;

        progress += moveSpeed * Time.deltaTime / Vector3.Distance(pathPoints[currentPoint].position, pathPoints[currentPoint + 1].position);
        progress = Mathf.Clamp01(progress);

        transform.position = Vector3.Lerp(
            pathPoints[currentPoint].position,
            pathPoints[currentPoint + 1].position,
            progress
        );

        // Interpolate roll
        float startRoll = (rollAngles != null && rollAngles.Length > currentPoint) ? rollAngles[currentPoint] : 0f;
        float endRoll = (rollAngles != null && rollAngles.Length > currentPoint + 1) ? rollAngles[currentPoint + 1] : 0f;
        float roll = Mathf.Lerp(startRoll, endRoll, progress);

        // Interpolate rotation and apply roll
        Quaternion baseRot = Quaternion.Slerp(
            pathPoints[currentPoint].rotation,
            pathPoints[currentPoint + 1].rotation,
            progress
        );
        Quaternion rollRot = Quaternion.AngleAxis(roll, Vector3.forward);
        transform.rotation = baseRot * rollRot;

        if (progress >= 1f)
        {
            currentPoint++;
            progress = 0f;
        }
    }
}
