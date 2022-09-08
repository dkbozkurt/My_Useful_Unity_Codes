// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace Splines.Splines_Basics
{
    /// <summary>
    /// Linear Interpolation by hand.
    ///
    /// It is something like Bezier Path Creator.
    ///
    /// Ref : https://www.youtube.com/watch?v=7j_BNf9s0jM&ab_channel=CodeMonkey
    /// </summary>
    public class GenerateBasicSmoothCurveWithLinearInterpolation : MonoBehaviour
    {
        [Header("Static Points")]
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;
        [SerializeField] private Transform pointC;
        [SerializeField] private Transform pointD;
        
        [Header("Dynamic Points")]
        [SerializeField] private Transform pointAB;
        [SerializeField] private Transform pointBC;
        [SerializeField] private Transform pointCD;
        [SerializeField] private Transform pointAB_BC;
        [SerializeField] private Transform pointBC_CD;
        [SerializeField] private Transform pointABCD;

        private float _interpolateAmount;
        
        private void Update()
        {
            // Only by using Linear Interpolation Method
            /*
            // Interpolate A-B
            LinearInterpolation(pointA,pointB,pointAB);
            
            // Interpolate B-C
            LinearInterpolation(pointB,pointC,pointBC);
            
            // Interpolate C-D
            LinearInterpolation(pointC,pointD,pointCD);
            
            // Interpolate AB-BC
            LinearInterpolation(pointAB,pointBC,pointAB_BC);
            
            // Interpolate BC-CD
            LinearInterpolation(pointBC,pointCD,pointBC_CD);
            
            // Interpolate ABCD
            LinearInterpolation(pointAB_BC,pointBC_CD,pointABCD);
            */
            
            // Same Functions but refactored
            pointABCD.position = CubicLerp(pointA.position, pointB.position, pointC.position, pointD.position,_interpolateAmount);


        }
        
        private void LinearInterpolation(Transform firstPoint,Transform secondPoint, Transform interpolatePoint)
        {
            _interpolateAmount = (_interpolateAmount + Time.deltaTime) % 1f;
            interpolatePoint.position = Vector3.Lerp(firstPoint.position, secondPoint.position, _interpolateAmount);
        }

        private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 ab = Vector3.Lerp(a, b, t);
            Vector3 bc = Vector3.Lerp(b, c, t);

            return Vector3.Lerp(ab, bc, _interpolateAmount);
        }

        private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c,Vector3 d, float t)
        {
            Vector3 ab_bc = QuadraticLerp(a, b, c, t);
            Vector3 bc_cd = QuadraticLerp(b, c, d, t);

            return Vector3.Lerp(ab_bc, bc_cd, _interpolateAmount);
        }
    }
}
