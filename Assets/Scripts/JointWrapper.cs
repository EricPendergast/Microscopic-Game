using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class JointWrapper : AwakeOnce {
    public UnityEvent onBreak;

    public SpringJoint2D joint;
    [SerializeField]
    private SimplePart attachedCellPart;
    
    public override void DoAwake() {
        joint = gameObject.AddComponent<SpringJoint2D>();
        attachedCellPart = GetComponent<SimplePart>();
        CellPartBalance.ConfigureJointConstants(joint);
        if (onBreak == null) {
            onBreak = new UnityEvent();
        }
    }

    public void SetConnected(SimplePart other) {
        Assert.IsNull(joint.connectedBody);
        other.OnConnectedTo(this);
        joint.connectedBody = other.GetComponent<Rigidbody2D>();
    }

    public SimplePart GetConnected() {
        if (joint.connectedBody == null) {
            return null;
        } else {
            return joint.connectedBody.GetComponent<SimplePart>();
        }
    }

    public SimplePart GetSource() {
        return attachedCellPart;
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
        if (broken != joint) {
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
