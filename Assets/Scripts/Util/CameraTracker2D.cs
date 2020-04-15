using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker2D : MonoBehaviour
{
    public GameObject trackedObject;
    public float trackSpeed;
    public float maxDistance;

    void Start() {
    }

    void FixedUpdate() {
        Vector3 currPos = transform.position - trackedObject.transform.position;
        Vector2 currPos2D = currPos;
        if (currPos2D.magnitude > maxDistance) {
            currPos2D.Normalize();
            currPos2D *= maxDistance;
        }

        currPos2D *= 1-trackSpeed;

        currPos.x = currPos2D.x;
        currPos.y = currPos2D.y;
        transform.position = currPos + trackedObject.transform.position;
    }
}
