using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cytoskeleton : SimplePart {
    new void Start() {
        base.Start();
    }

    new void OnMouseUp() {
        Debug.Log("Cytoskeleton mouse up over [" + Mouse.currentlyOver+ "] ["+ null+ "]");
        // TODO: Make this toggle the joint
        if (Mouse.currentlyOver != null) {
            var joint = gameObject.AddComponent<RelativeJoint2D>();
            joint.connectedBody = Mouse.currentlyOver.gameObject.GetComponent<Rigidbody2D>();
            //joint.linearOffset = joint.linearOffset/2;
            joint.autoConfigureOffset = false;
            joint.maxForce = 10;
            joint.enableCollision = true;
        }
    }

    new void OnMouseDrag() {
        //Debug.Log("Cytoskeleton mouse up");
        //Mouse.OnMouseDragCellPart(this);
    }

    //new void 

    void OnTriggerEnter2D(Collider2D collision) {
        //if (collision.gameObject.TryGetComponent(out SimplePart part)) {
        //    if (part is Cytoskeleton && this.GetInstanceID() < part.GetInstanceID()) {
        //        return;
        //    }
        //    //Debug.Log("TRIGGER ENTER");
        //    var joint = gameObject.AddComponent<RelativeJoint2D>();
        //    joint.connectedBody = part.gameObject.GetComponent<Rigidbody2D>();
        //    Debug.Log(joint.linearOffset + " " + joint.linearOffset.magnitude);
        //    joint.linearOffset = joint.linearOffset/2;
        //    joint.autoConfigureOffset = false;
        //    joint.maxForce = 10;
        //    Debug.Log(joint.linearOffset + " " + joint.linearOffset.magnitude);
        //    joint.enableCollision = true;
        //    Debug.Log(gameObject.GetComponents<RelativeJoint2D>());
        //}
    }

    void OnTriggerExit2D(Collider2D collision) {
        Debug.Log(gameObject.GetComponents<RelativeJoint2D>());
        foreach (RelativeJoint2D joint in gameObject.GetComponents<RelativeJoint2D>()) {
            if (joint.connectedBody.gameObject == collision.gameObject) {
                Destroy(joint);
            }
        }
    }
}
