//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePart : MonoBehaviour {
    private float friction = .99f;

    protected Rigidbody2D body;
    private CellGroup cellGroup = null;

    //public List<SpringJoint2D> joints;

    public void Start() {
        body = GetComponent<Rigidbody2D>();
        SetParent(transform.parent.GetComponent<CellGroup>());
        StartCoroutine(UpdateSpringsCoroutine());
    }

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

    IEnumerator<WaitForSeconds> UpdateSpringsCoroutine(){
        while (true) {
            UpdateSprings();
            yield return new WaitForSeconds(1f);
        }
    }

    void UpdateSprings() {
        if (cellGroup != null) {
            foreach (SimplePart sibling in cellGroup.GetCellParts()) {
                if (sibling == this) {
                    continue;
                }
                if ((transform.position - sibling.transform.position).magnitude < 2) {
                    SpringJoint2D conn = cellGroup.MakeJoint(this, sibling);
                    //conn.distance += 1f*(1 - Random.value*2);
                    //conn.distance = Mathf.Clamp(conn.distance, .5f, 3);
                    conn.distance = 1.5f;
                    conn.frequency = 1;
                } else {
                    cellGroup.DestroyJoint(this, sibling);
                }
            }
        }
    }

    public void Update() {
        body.velocity = body.velocity*(friction);
    }
}
