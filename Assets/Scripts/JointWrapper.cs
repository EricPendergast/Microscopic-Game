using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class JointWrapper : MonoBehaviour {
    public UnityEvent onBreak;

    private SpringJoint2D joint;
    private SimplePart attachedCellPart;
    
    void Awake() {
        joint = gameObject.AddComponent<SpringJoint2D>();
        attachedCellPart = GetComponent<SimplePart>();
        CellPartBalance.ConfigureJointConstants(joint);
        onBreak = new UnityEvent();
    }

    public void SetConnected(SimplePart other) {
        Assert.IsNull(joint.connectedBody);
        joint.connectedBody = other.GetComponent<Rigidbody2D>();
    }

    public SimplePart GetConnected() {
        if (joint.connectedBody == null) {
            return null;
        } else {
            return joint.connectedBody.GetComponent<SimplePart>();
        }
    }

    public void Reconfigure() {
        if (joint != null) {
            CellPartBalance.ConfigureJointConstants(joint);
        }
    }

    //void Update() {
    //  update rendering
    //}
    
    void OnJointBreak2D(Joint2D broken) {
        Debug.Log("Joint broke part 1 [" + joint + "]");
        if (joint != broken) {
            return;
        }

        onBreak.Invoke();
        Destroy(this);
    }
    
    void OnDestroy() {
    // What do we do here?
    // Clean up rendering possibly
        if (joint != null) {
            Destroy(joint);
        }
    }
}
