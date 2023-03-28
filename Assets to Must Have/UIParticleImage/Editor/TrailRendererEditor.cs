using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssetKits.ParticleImage.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ParticleTrailRenderer))]
    public class TrailRendererEditor : UnityEditor.Editor
    {
        void Awake()
        {
            MonoScript.FromMonoBehaviour(target as ParticleTrailRenderer).SetIcon(Resources.Load<Texture2D>("TrailIcon"));
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Particle Image trail renderer.", MessageType.Info);
        }
    }
}

