//  Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace DKBozkurt.Utils
{
    /// <summary>
    /// My own Utils class for Creating.
    /// 
    /// </summary>
    
    public static partial class DKBozkurtUtils 
    {
        
        /// <summary>
        /// Creates a Sprite in the World, no parent.
        /// </summary>
        /// <param name="name"> Name of the object.</param>
        /// <param name="sprite">Sprite to assign into Sprite Renderer component.</param>
        /// <param name="position">World position of the object.</param>
        /// <param name="localScale">Scale of the object.</param>
        /// <param name="sortingOrder">Order in the hierarchy.</param>
        /// <param name="color">Initial color of the sprite.</param>
        /// <returns> GameObject that contains Sprite in it.</returns>
        public static GameObject CreateWorldSprite(string name, Sprite sprite, Vector3 position, Vector3 localScale,
            int sortingOrder, Color color)
        {
            return CreateWorldSprite(null, name, sprite, position, localScale, sortingOrder, color);
        }
        
        /// <summary>
        /// Creates a Sprite in the World.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="name"> Name of the object.</param>
        /// <param name="sprite">Sprite to assign into Sprite Renderer component.</param>
        /// <param name="position">World position of the object.</param>
        /// <param name="localScale">Scale of the object.</param>
        /// <param name="sortingOrder">Order in the hierarchy.</param>
        /// <param name="color">Initial color of the sprite.</param>
        /// <returns> GameObject that contains Sprite in it.</returns>
        /// <returns></returns>
        public static GameObject CreateWorldSprite(Transform parent, string name, Sprite sprite, Vector3 localPosition, Vector3 localScale, int sortingOrder, Color color) {
            GameObject gameObject = new GameObject(name, typeof(SpriteRenderer));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.color = color;
            return gameObject;
        }

        /// <summary>
        /// Quickly destroy all child objects.
        /// </summary>
        /// <param name="t">Parent object's transform </param>
        public static void DeleteChildren(this Transform t)
        {
            foreach (Transform child in t) { Object.Destroy(child.gameObject); }
        }
        
    }
}