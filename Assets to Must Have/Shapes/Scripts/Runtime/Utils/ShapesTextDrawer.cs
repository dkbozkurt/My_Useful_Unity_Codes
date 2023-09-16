using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;

// Shapes © Freya Holmér - https://twitter.com/FreyaHolmer/
// Website & Documentation - https://acegikmo.com/shapes/
namespace Shapes {

	public class ShapesTextDrawer : MonoBehaviour {

		static ShapesTextDrawer instance;
		public static ShapesTextDrawer Instance {
			get {
				if( instance == null ) {
					instance = Object.FindObjectOfType<ShapesTextDrawer>();
					if( instance == null )
						instance = ShapesTextDrawer.Create();
				}

				return instance;
			}
		}

		public TextMeshPro tmp;

		static ShapesTextDrawer Create() {
			GameObject holder = new GameObject( "TEXT DRAWER" );
			if( Application.isPlaying )
				DontDestroyOnLoad( holder ); // might be a lil gross, not sure
			ShapesTextDrawer text = holder.AddComponent<ShapesTextDrawer>();
			text.tmp = holder.AddComponent<TextMeshPro>();
			text.tmp.enableWordWrapping = false;
			text.tmp.overflowMode = TextOverflowModes.Overflow;

			// mesh renderer should exist now due to TMP requiring the component
			holder.GetComponent<MeshRenderer>().enabled = false;

			Hide( holder );
			return text;
		}

		static void Hide( params Object[] objs ) => objs.ForEach( o => o.hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy );

	}

}