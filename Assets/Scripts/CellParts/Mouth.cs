//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Mouth : SimplePart {
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent(out SimplePart part)) {
            part.transform.parent = GetCellGroup().transform;
        }
    }
}
