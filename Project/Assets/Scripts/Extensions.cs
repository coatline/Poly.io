using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static float AngleFromPosition(Vector3 pivotPosition, Vector3 pos)
    {
        float angleRad = Mathf.Atan2(pos.y - pivotPosition.y, pos.x - pivotPosition.x);
        float angleDeg = (180 / Mathf.PI) * angleRad - 90;
        return angleDeg;
    }
}
