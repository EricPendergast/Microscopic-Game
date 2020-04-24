using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse {
    public static SimplePart currentlySelected = null;
    public static SimplePart currentlyOver = null;

    public static Vector2 WorldPosition() {
       return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public static bool LeftMouseDown() {
        return Input.GetMouseButtonDown(0);
    }

    public static bool RightMouseDown() {
        return Input.GetMouseButtonDown(1);
    }

    public static bool LeftMouseUp() {
        return Input.GetMouseButtonUp(0);
    }

    public static bool RightMouseUp() {
        return Input.GetMouseButtonUp(1);
    }

    public static bool LeftMouse() {
        return Input.GetMouseButton(0);
    }

    public static bool RightMouse() {
        return Input.GetMouseButton(1);
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
