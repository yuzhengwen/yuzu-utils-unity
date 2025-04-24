using System.Collections.Generic;
using UnityEngine;

namespace YuzuValen.Utils
{
    public static class MiscExtensions
    {
        public static LayerMask RemoveLayer(this LayerMask layerMask, string layerName)
        {
            int layerToRemove = LayerMask.NameToLayer(layerName);
            int layerMaskValue = 1 << layerToRemove;
            return layerMask &= ~layerMaskValue;
        }

        public static LayerMask AddLayer(this LayerMask layerMask, string layerName)
        {
            int layerToAdd = LayerMask.NameToLayer(layerName);
            int layerMaskValue = 1 << layerToAdd;
            return layerMask |= layerMaskValue;
        }

        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                (ts[i], ts[r]) = (ts[r], ts[i]);
            }
        }
    }

    public static class MyDebug
    {
        public static void DrawCircle(Vector3 position, float radius, int segments, Color color)
        {
            // If either radius or number of segments are less or equal to 0, skip drawing
            if (radius <= 0.0f || segments <= 0)
            {
                return;
            }

            // Single segment of the circle covers (360 / number of segments) degrees
            float angleStep = (360.0f / segments);

            // Result is multiplied by Mathf.Deg2Rad constant which transforms degrees to radians
            // which are required by Unity's Mathf class trigonometry methods

            angleStep *= Mathf.Deg2Rad;

            // lineStart and lineEnd variables are declared outside of the following for loop
            Vector3 lineStart = Vector3.zero;
            Vector3 lineEnd = Vector3.zero;

            for (int i = 0; i < segments; i++)
            {
                // Line start is defined as starting angle of the current segment (i)
                lineStart.x = Mathf.Cos(angleStep * i);
                lineStart.y = Mathf.Sin(angleStep * i);

                // Line end is defined by the angle of the next segment (i+1)
                lineEnd.x = Mathf.Cos(angleStep * (i + 1));
                lineEnd.y = Mathf.Sin(angleStep * (i + 1));

                // Results are multiplied so they match the desired radius
                lineStart *= radius;
                lineEnd *= radius;

                // Results are offset by the desired position/origin 
                lineStart += position;
                lineEnd += position;

                // Points are connected using DrawLine method and using the passed color
                UnityEngine.Debug.DrawLine(lineStart, lineEnd, color);
            }
        }
    }
}