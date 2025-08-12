using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilScripts
{
    public static Vector2 GetVectorToMouse(Vector3 position)
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition) - position;
    }
}
