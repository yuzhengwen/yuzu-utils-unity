using UnityEngine;

namespace YuzuValen.Utils
{
    public static class MathUtils
    {
        public static Vector3[] GetArcPositions(Vector3 startPos, int count, float arcRadius, float arcSpreadAngle,
            Vector3 arcDir)
        {
            Vector3[] positions = new Vector3[count];
            Quaternion arcRotation = Quaternion.LookRotation(Vector3.forward, arcDir.normalized);
            float startAngle = 0;
            float angleIncrement = arcSpreadAngle / (count - 1);

            for (int i = 0; i < count; i++)
            {
                float angle = startAngle + i * angleIncrement;
                // generates an arc with 0 degrees facing right
                Vector3 localOffset = new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad) * arcRadius,
                    Mathf.Sin(angle * Mathf.Deg2Rad) * arcRadius,
                    0f
                );
                // rotates and offsets such that 0 degrees faces up
                positions[i] = startPos + arcRotation * localOffset;
            }

            return positions;
        }

        public static int Round(this int value, RoundType type, int magnitudeOfTen)
        {
            var factor = (int)Mathf.Pow(10, magnitudeOfTen);
            return type switch
            {
                RoundType.Nearest => Mathf.RoundToInt((float)value / factor) * factor,
                RoundType.Up => Mathf.CeilToInt((float)value / factor) * factor,
                RoundType.Down => Mathf.FloorToInt((float)value / factor) * factor,
                _ => value
            };
        }
    }

    public enum RoundType
    {
        Nearest,
        Up,
        Down
    }
}