using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse {
    //public static SimplePart currentlySelected;
    public static SimplePart currentlyOver;

    public static Vector2 WorldPosition() {
       return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    //public static void OnMouseDragCellPart(SimplePart part) {
    //    currentlySelected = part;
    //}
    // TODO: This may be better handled by OnMouseDrag
    //public static void MouseDownOnCellPart(SimplePart part) {
    //    if (currentlySelected == null) {
    //        currentlySelected = part;
    //    }
    //}
    //
    //public static void MouseUpOnCellPart(SimplePart part) {
    //    if (Mouse.currentlySelected == part) {
    //        Mouse.currentlySelected = null;
    //    }
    //}

    public static void OnMouseOverCellPart(SimplePart part) {
        currentlyOver = part;
    }
}
