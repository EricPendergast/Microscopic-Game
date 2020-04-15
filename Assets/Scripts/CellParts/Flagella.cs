using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flagella : SimplePart {
    public float accel;

    //new void Start() {
    //    base.Start();
    //}

    new void Update() {
        Vector3 force = new Vector3(accel*Input.GetAxis("Horizontal"), accel*Input.GetAxis("Vertical"));
        body.AddForce(force);
        base.Update();
    }
}
