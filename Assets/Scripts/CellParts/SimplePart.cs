//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePart : MonoBehaviour {
    private float friction = .99f;

    protected Rigidbody2D body;
    private CellGroup cellGroup = null;

    public List<SpringJoint2D> joints;

    public void Start() {
        body = GetComponent<Rigidbody2D>();
        SetParent(transform.parent.GetComponent<CellGroup>());
        StartCoroutine(UpdateSprings());
    }

    // Always use this to change parents
    public void SetParent(CellGroup newCellGroup) {
        if (newCellGroup == cellGroup) {
            return;
        }
        transform.SetParent(newCellGroup.transform);
        foreach (SpringJoint2D joint in joints) {
            Destroy(joint);
        }
        joints = new List<SpringJoint2D>();
        if (transform.parent == null) {
            return;
        }
        cellGroup = transform.parent.GetComponent<CellGroup>();
        if (cellGroup == null) {
            return;
        }

        // Connect to all other close things
        // TODO: Don't connect everything, only close things
        foreach (Transform child in transform.parent) {
            if (child == transform) {
                continue;
            }
            //if (joints.Count == 4) {}
            // Make a spring from this to child
            SpringJoint2D conn = gameObject.AddComponent<SpringJoint2D>();
            conn.connectedBody = child.gameObject.GetComponent<Rigidbody2D>();
            conn.distance = Random.value*3;
            conn.frequency = 1;

            joints.Add(conn);
        }
    }

    public CellGroup GetCellGroup() {
        return cellGroup;
    }

    // This may be too silly
    IEnumerator<WaitForSeconds> UpdateSprings() {
        while (true) {
            foreach (SpringJoint2D conn in joints) {
                conn.distance += 1f*(1 - Random.value*2);
                conn.distance = Mathf.Clamp(conn.distance, .5f, 3);
            }
            yield return new WaitForSeconds(Random.value*10f);
        }
    }

    public void Update() {
        body.velocity = body.velocity*(friction);
    }
}
