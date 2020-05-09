using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePart : AwakeOnce {
    protected Rigidbody2D body;
    public int updateSpringsCoroutineCount = 0;

    public override void DoAwake() {
        NearbyDetector.Create(this);
    }

    public void Start() {
        body = GetComponent<Rigidbody2D>();
        transform.position += new Vector3(0,0,Random.value);
        OnTransformParentChanged();
        StartCoroutine(UpdateSpringsCoroutine());
    }
    
    public void OnMouseUp() {
        if (Mouse.LeftMouseUp()) {
            Destroy(GetComponent<TargetJoint2D>());
        }
    }

    public void OnMouseOver() {
        Mouse.OnMouseOverCellPart(this);
    }

    public void OnMouseDrag() {
        if (Mouse.LeftMouse()) {
            //Mouse.OnMouseDragCellPart(this);
            var mouseSpring = GetComponent<TargetJoint2D>();
            if (mouseSpring == null) {
                mouseSpring = gameObject.AddComponent<TargetJoint2D>();
            }
            mouseSpring.target = Mouse.WorldPosition();
        }
    }

    public void OnTransformParentChanged() {
        UpdateSprings();
    }

    IEnumerator UpdateSpringsCoroutine(){
        while (true) {
            UpdateSprings();
            updateSpringsCoroutineCount++;
            yield return new WaitForSeconds(.5f + Random.value * CellPartBalance.i.springUpdateTime);
        }
    }

    public void Update() {
        body.velocity = body.velocity*(CellPartBalance.i.friction);
    }

    public List<SimplePart> GetNearby(float distance) {
        CellGroup cg = transform.parent.GetComponent<CellGroup>();
        if (cg == null) {
            return new List<SimplePart>();
        } else {
            return cg.GetNearby(this, distance);
        }
    }

    public float Distance(SimplePart sp1) {
        return (transform.position - sp1.transform.position).magnitude;
    }


    public virtual void OnConnectedTo(JointWrapper joint) {}

    public virtual void ConfigureJointConstants(JointWrapper joint) {
        joint.joint.distance = joint.GetSource().GetNearbyRadius() + joint.GetConnected().GetRadius();
        joint.joint.frequency = CellPartBalance.i.springFreq;
        joint.joint.breakForce = CellPartBalance.i.springBreakForce;
        joint.joint.autoConfigureDistance = false;
        joint.joint.enableCollision = true;
    }

    // Called on the joint owner when a joint breaks
    public virtual void OnOwnedJointBroke(JointWrapper joint) {}

    // Called on the other end of the joint when a joint breaks
    public virtual void OnUnownedJointBroke(JointWrapper joint) {}


    public virtual void UpdateSprings() {
        foreach (var joint in GetComponents<JointWrapper>()) {
            joint.Reconfigure();
        }
    }

    // Returns an integer representing how much this cell part wants to control
    // the joint from it to the other cell part
    public virtual float JointDesire(SimplePart other) {
        return 0;
    }

    public virtual float GetRadius() {
        CircleCollider2D c = GetComponent<CircleCollider2D>();
        return c.radius*c.transform.lossyScale.x;
    }

    public virtual float GetNearbyRadius() {
        return GetRadius() + CellPartBalance.i.springDist;
    }

    public virtual void OnCellPartEnterNearby(SimplePart cp) {
        if (Distance(cp) < CellPartBalance.i.springMaxDist) {
            JointWrapper.MakeJoint(this, cp);
        }
    }

    public virtual void OnCellPartExitNearby(SimplePart cp) {}
}
