using System;
using UnityEngine;

namespace Line.Draw_Line_Between_Points
{
    /// <summary>
    ///
    /// Attach this script onto empty gameobject.
    ///
    /// Use this script with "PointToPointLineDrawer.cs"
    /// 
    /// Ref : https://www.youtube.com/watch?v=5ZBynjAsfwI
    /// </summary>

    public class PointToPointLineDrawManager : MonoBehaviour
    {
        [SerializeField] private Transform[] points;
        [SerializeField] private PointToPointLineDrawer pointToPointLineDrawer;

        private void Start()
        {
            pointToPointLineDrawer.SetUpLine(points);
        }
    }
}