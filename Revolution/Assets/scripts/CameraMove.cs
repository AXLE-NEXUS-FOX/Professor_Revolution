using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform[] pathPoints; // Assign waypoints in the Inspector
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

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

        // Increase progress based on moveSpeed
        progress += moveSpeed * Time.deltaTime / Vector3.Distance(pathPoints[currentPoint].position, pathPoints[currentPoint + 1].position);
        progress = Mathf.Clamp01(progress);

        // Smoothly interpolate position and rotation
        transform.position = Vector3.Lerp(
            pathPoints[currentPoint].position,
            pathPoints[currentPoint + 1].position,
            progress
        );

        transform.rotation = Quaternion.Slerp(
            pathPoints[currentPoint].rotation,
            pathPoints[currentPoint + 1].rotation,
            progress
        );

        // Move to next segment if finished
        if (progress >= 1f)
        {
            currentPoint++;
            progress = 0f;
        }
    }
}
