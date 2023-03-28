using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AssetKits.ParticleImage.Editor
{
    public static class EditorUtilities
    {
        public static class ParticleImageAddMenu
        {
            private const int MenuPriority = 10;

            [MenuItem("GameObject/UI/Particle Image", priority = MenuPriority)]
            private static void CreateButton(MenuCommand menuCommand)
            {
                var canvas = Object.FindObjectsOfType(typeof(Canvas)).Cast<Canvas>().FirstOrDefault();

                if (canvas)
                {
                    // Create a custom game object
                    GameObject go = new GameObject("Particle Image");
                    ParticleImage pi = go.AddComponent<ParticleImage>();
                    pi.texture = AssetDatabase.GetBuiltinExtraResource<Texture2D>("Default-Particle.psd");
                    pi.canvasRect = canvas.GetComponent<RectTransform>();
                    if (menuCommand.context)
                    {
                        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
                    }
                    else
                    {
                        GameObjectUtility.SetParentAndAlign(go, canvas.gameObject);
                    }
                    
                    Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
                    Selection.activeObject = go;
                }
                else
                {
                    GameObject newCanvas = new GameObject("Canvas");
                    Canvas c = newCanvas.AddComponent<Canvas>();
                    c.renderMode = RenderMode.ScreenSpaceOverlay;
                    newCanvas.AddComponent<CanvasScaler>();
                    newCanvas.AddComponent<GraphicRaycaster>();
                    
                    // Create a custom game object
                    GameObject go = new GameObject("Particle Image");
                    ParticleImage pi = go.AddComponent<ParticleImage>();
                    pi.texture = AssetDatabase.GetBuiltinExtraResource<Texture2D>("Default-Particle.psd");
                    pi.canvasRect = newCanvas.GetComponent<RectTransform>();
                    GameObjectUtility.SetParentAndAlign(go, newCanvas);
                    
                    Undo.RegisterCreatedObjectUndo(newCanvas, "Create " + go.name);
                    Selection.activeObject = go;
                }
                
                var eventSystem = Object.FindObjectsOfType(typeof(EventSystem)).Cast<EventSystem>().FirstOrDefault();

                if (eventSystem == null)
                {
                    GameObject eSystem = new GameObject("EventSystem");
                    EventSystem e = eSystem.AddComponent<EventSystem>();
                    eSystem.AddComponent<StandaloneInputModule>();
                }
            }
        }
        
        /// <summary>
        /// Set the icon for this object.
        /// </summary>
        public static void SetIcon(this Object obj, Texture2D texture)
        {
            var ty = typeof(EditorGUIUtility);
            var mi = ty.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
            if (mi != null)
            {
                mi.Invoke(null, new object[] { obj, texture });
            }
        }

        /// <summary>
        /// Get the icon for this object.
        /// </summary>
        public static Texture2D GetIcon(this Object obj)
        {
            var ty = typeof(EditorGUIUtility);
            var mi = ty.GetMethod("GetIconForObject", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
            if (mi != null)
            {
                return mi.Invoke(null, new object[] { obj }) as Texture2D;
            }
            else
            {
                return null;
            }
        }

        public static void RemoveIcon(this Object obj)
        {
            SetIcon(obj, (Texture2D)null);
        }
    }
}

