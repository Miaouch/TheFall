using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotation : MonoBehaviour
{

    public Transform playerPivot;

    public float timeToRotate = 2f;
    private bool rotating = false;

    void OnTriggerEnter(Collider other)
    {
        print("Rotation ?");
        if (other.CompareTag("RotationTrigger") && !rotating)
        {
            StartCoroutine(RotateAroundTheEdge(other.transform));
        }
    }

    IEnumerator RotateAroundTheEdge(Transform pivotPoint) {

        Plane plane = new Plane(pivotPoint.up - pivotPoint.right, pivotPoint.position);
        Ray downRay = new Ray(playerPivot.position, -playerPivot.up);

        float dist;
        plane.Raycast(downRay, out dist);

        Vector3 rotationPoint = downRay.GetPoint(dist);

        float startTime = Time.time;
        rotating = true;
        while (Time.time <= timeToRotate + startTime) {
            playerPivot.RotateAround(rotationPoint, pivotPoint.forward, -90 / timeToRotate * Time.deltaTime);
            yield return true;
        }

        rotating = false;
    }

}