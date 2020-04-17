using UnityEngine;

public class Flagella : SimplePart {
    public float accel;

    new void Update() {
        // TODO: Make an empty cellgrouop, so that GetCellGroup is never null
        if (GetCellGroup() != null && GetCellGroup().isPlayer) {
            Vector3 force = new Vector3(accel*Input.GetAxis("Horizontal"), accel*Input.GetAxis("Vertical"));
            body.AddForce(force);
        }
        base.Update();
    }
}
