using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePart : MonoBehaviour {
    protected Rigidbody2D body;
    private CellGroup cellGroup = null;
    
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

    public void Start() {
        body = GetComponent<Rigidbody2D>();
        OnTransformParentChanged();
        StartCoroutine(UpdateSpringsCoroutine());
    }

    public void OnTransformParentChanged() {
        cellGroup = transform.parent.GetComponent<CellGroup>();

        UpdateSprings();
    }

    public virtual void OnConnectedTo(JointWrapper joint) {}

    public CellGroup GetCellGroup() {
        return cellGroup;
    }

    IEnumerator UpdateSpringsCoroutine(){
        while (true) {
            UpdateSprings();
            yield return new WaitForSeconds(.5f + Random.value * CellPartBalance.i.springUpdateTime);
        }
    }

    public virtual void UpdateSprings() {
        foreach (SimplePart sibling in GetNearby(CellPartBalance.i.springMaxDist)) {
            JointWrapper joint = GetCellGroup().MakeJoint(this, sibling);
        }

        foreach (var joint in GetComponents<JointWrapper>()) {
            joint.Reconfigure();
        }
    }

    public void Update() {
        body.velocity = body.velocity*(CellPartBalance.i.friction);
    }

    // Returns an integer representing how much this cell part wants to control
    // the joint from it to the other cell part
    public virtual float JointDesire(SimplePart other) {
        return 0;
    }

    public List<SimplePart> GetNearby(float distance) {
        if (GetCellGroup() == null) {
            return new List<SimplePart>();
        } else {
            return GetCellGroup().GetNearby(this, distance);
        }
    }

    public float Distance(SimplePart sp1) {
        return (transform.position - sp1.transform.position).magnitude;
    }
}
