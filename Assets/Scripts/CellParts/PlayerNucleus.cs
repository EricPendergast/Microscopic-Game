
using UnityEngine;

public class PlayerNucleus : Nucleus {
    public override void Update() {
        CellGroup cg = transform.parent.GetComponent<CellGroup>();
        if (cg != null && cg.isPlayer) {
            Vector3 force = new Vector3(accel*Input.GetAxis("Horizontal"), accel*Input.GetAxis("Vertical"));
            body.AddForce(force);
            if (Input.GetKeyDown(KeyCode.Space)) {
                isSticky = !isSticky;
            }
        }

        base.Update();
    }
}
