using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cytoskeleton : SimplePart {
    new void Start() {
        base.Start();
    }

    new void OnMouseOver() {
        base.OnMouseOver();
        if (Mouse.RightMouseDown()) {
            Debug.Log("Right button down");
            StartCoroutine(DragCoroutine());
        }
    }

    IEnumerator DragCoroutine() {
        while (Mouse.RightMouse()) {
            yield return null;
        }

        Debug.Log("Mouse released");

        if (Mouse.currentlyOver != null) {
            var joint = gameObject.AddComponent<RelativeJoint2D>();
            joint.connectedBody = Mouse.currentlyOver.gameObject.GetComponent<Rigidbody2D>();
            joint.autoConfigureOffset = false;
            joint.maxForce = 100;
            joint.enableCollision = true;
        }
    }
}
