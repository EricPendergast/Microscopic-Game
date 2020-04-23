using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePart : MonoBehaviour {
    protected Rigidbody2D body;
    private CellGroup cellGroup = null;
    //public List<SpringJoint2D> joints;
    
    public void OnMouseUp() {
        if (Input.GetMouseButtonUp(0)) {
            Destroy(GetComponent<TargetJoint2D>());
        }
    }

    public void OnMouseOver() {
        Mouse.OnMouseOverCellPart(this);
    }

    public void OnMouseDrag() {
        if (Input.GetMouseButton(0)) {
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
        SetParent(transform.parent.GetComponent<CellGroup>());
        StartCoroutine(UpdateSpringsCoroutine());
    }

    // TODO: This can get called by the event OnTransformParentChanged
    // Always use this to change parents
    public void SetParent(CellGroup newCellGroup) {
        if (newCellGroup == cellGroup) {
            return;
        }
        if (cellGroup != null) {
            foreach (SimplePart sibling in cellGroup.GetCellParts()) {
                cellGroup.DestroyJoint(this, sibling);
            }
        }
        transform.SetParent(newCellGroup.transform);
        cellGroup = transform.parent.GetComponent<CellGroup>();

        UpdateSprings();
    }

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
        foreach (SimplePart sibling in GetCellGroup().GetNearby(this, CellPartBalance.i.springMaxDist)) {
            if (sibling == this || this != GetCellGroup().WhoControlsJoint(this, sibling)) {
                continue;
            }
            SpringJoint2D joint = cellGroup.MakeJoint(this, sibling);
            CellPartBalance.ConfigureJointConstants(joint);
        }

        foreach (var joint in GetComponents<SpringJoint2D>()) {
            if (this == GetCellGroup().WhoControlsJoint(this, joint.connectedBody.GetComponent<SimplePart>())) {
                CellPartBalance.ConfigureJointConstants(joint);
            }
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

    //public List<SimplePart> GetAll() {
    //    var siblings = new List<SimplePart>();
    //    if (GetCellGroup() == null) {
    //        return siblings;
    //    }
    //
    //    foreach (SimplePart sibling in GetCellGroup().GetCellParts()) {
    //        siblings.Add(sibling);
    //    }
    //    
    //    return siblings;
    //}

    public float Distance(SimplePart sp1) {
        return (transform.position - sp1.transform.position).magnitude;
    }
}
