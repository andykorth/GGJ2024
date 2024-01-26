using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Contains methods that extend Unity's RectTransform class.
/// </summary>
static public class RectTransformExtensions
{
    /// <summary>
    /// Returns a bounding Rect that is in screen space (where each unit equals a pixel.)
    /// </summary>
    /// <param name="rt">The RectTransform to perform the transformation.</param>
    /// <param name="forCamera">The Camera that is in charge of rendering to screen space.</param>
    /// <returns>A bounding Rect that is in screen space.</returns>
    static public Rect GetScreenspaceRect(this RectTransform rt, Camera forCamera)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        IEnumerable<Vector3> cornerList = corners.Select(x => Camera.main.WorldToScreenPoint(x));

        Rect r = new Rect();
        r.xMin = cornerList.Select(x => x.x).Min();
        r.xMax = cornerList.Select(x => x.x).Max();
        r.yMin = cornerList.Select(x => x.y).Min();
        r.yMax = cornerList.Select(x => x.y).Max();

        return r;
    }
}
