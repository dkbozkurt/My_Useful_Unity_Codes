using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component marks the current GameObject as being paintable.
	/// NOTE: This GameObject must has a MeshFilter + MeshRenderer, or a SkinnedMeshRenderer.
	/// To actually paint your object, you must also add at least one <b>P3dPaintableTexture</b> component to specify which texture you want to paint.</summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Renderer))]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dPaintable")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Paintable")]
	public class P3dPaintable : P3dModel
	{
		public enum ActivationType
		{
			Awake,
			OnEnable,
			Start,
			OnFirstUse
		}

		public override P3dPaintable Paintable { set {  } get { paintable = this; return paintable; } }

		/// <summary>This allows you to control when this component actually activates and becomes ready for painting. You probably don't need to change this.</summary>
		public ActivationType Activation { set { activation = value; } get { return activation; } } [SerializeField] private ActivationType activation = ActivationType.Start;

		/// <summary>If you want the paintable texture width/height to be multiplied by the scale of this GameObject, this allows you to set the scale where you want the multiplier to be 1.</summary>
		public Vector3 BaseScale { set { baseScale = value; } get { return baseScale; } } [SerializeField] private Vector3 baseScale;

		/// <summary>If this material is used in multiple renderers, you can specify them here. This usually happens with different LOD levels.</summary>
		public List<Renderer> OtherRenderers { set { otherRenderers = value; } get { return otherRenderers; } } [SerializeField] private List<Renderer> otherRenderers;

		/// <summary>This event will be invoked before this component is activated.</summary>
		public UnityEvent OnActivating { get { if (onActivating == null) onActivating = new UnityEvent(); return onActivating; } } [SerializeField] private UnityEvent onActivating;

		/// <summary>This event will be invoked after this component is activated.</summary>
		public UnityEvent OnActivated { get { if (onActivated == null) onActivated = new UnityEvent(); return onActivated; } } [SerializeField] private UnityEvent onActivated;

		[SerializeField]
		private bool activated;

		[System.NonSerialized]
		private HashSet<P3dPaintableTexture> paintableTextures = new HashSet<P3dPaintableTexture>();

		[System.NonSerialized]
		private static List<P3dMaterialCloner> materialCloners = new List<P3dMaterialCloner>();

		[System.NonSerialized]
		private List<P3dPaintableTexture> tempPaintableTextures = new List<P3dPaintableTexture>();

		/// <summary>This lets you know if this paintable has been activated.
		/// Being activated means each associated P3dMaterialCloner and P3dPaintableTexture has been Activated.
		/// NOTE: If you manually add P3dMaterialCloner or P3dPaintableTexture components after activation, then you must manually Activate().</summary>
		public bool Activated
		{
			get
			{
				return activated;
			}
		}

		/// <summary>This gives you all P3dPaintableTexture components that have been activated.</summary>
		public HashSet<P3dPaintableTexture> PaintableTextures
		{
			get
			{
				return paintableTextures;
			}
		}

		/// <summary>This method will remove all <b>P3dPaintable</b>, <b>P3dMaterialCloner</b>, and <b>P3dPaintableTexture</b> components from this GameObject.</summary>
		public void RemoveComponents()
		{
			var paintableTextures = GetComponents<P3dPaintableTexture>();

			for (var i = paintableTextures.Length - 1; i >= 0; i--)
			{
				var paintableTexture = paintableTextures[i];

				paintableTexture.Deactivate();

				CwHelper.Destroy(paintableTexture);
			}

			var materialCloners = GetComponents<P3dMaterialCloner>();

			for (var i = materialCloners.Length - 1; i >= 0; i--)
			{
				var materialCloner = materialCloners[i];

				materialCloner.Deactivate();

				CwHelper.Destroy(materialCloner);
			}

			CwHelper.Destroy(this);
		}

		/// <summary>This will scale the specified width and height values based on the current BaseScale setting.</summary>
		public void ScaleSize(ref int width, ref int height)
		{
			if (baseScale != Vector3.zero)
			{
				var scale = transform.localScale.magnitude / baseScale.magnitude;

				width  = Mathf.CeilToInt(width  * scale);
				height = Mathf.CeilToInt(height * scale);
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Activate", true)]
		private bool ActivateValidate()
		{
			return Application.isPlaying == true && activated == false;
		}
#endif

		/// <summary>This allows you to manually activate all attached P3dMaterialCloner and P3dPaintableTexture components.</summary>
		[ContextMenu("Activate")]
		public void Activate()
		{
			if (onActivating != null)
			{
				onActivating.Invoke();
			}

			// Activate material cloners
			GetComponents(materialCloners);

			for (var i = materialCloners.Count - 1; i >= 0; i--)
			{
				materialCloners[i].Activate();
			}

			// Activate textures
			AddPaintableTextures(transform);

			foreach (var paintableTexture in paintableTextures)
			{
				paintableTexture.Activate();
			}

			activated = true;

			if (onActivated != null)
			{
				onActivated.Invoke();
			}
		}

		private void AddPaintableTextures(Transform root)
		{
			root.GetComponents(tempPaintableTextures);

			foreach (var paintableTexture in tempPaintableTextures)
			{
				paintableTextures.Add(paintableTexture);
			}

			for (var i = 0; i < root.childCount; i++)
			{
				var child = root.GetChild(i);

				if (child.GetComponent<P3dPaintable>() == null)
				{
					AddPaintableTextures(child);
				}
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Deactivate", true)]
		private bool DeactivateValidate()
		{
			return activated == true;
		}
#endif

		/// <summary>This reverses the material cloning.</summary>
		[ContextMenu("Deactivate")]
		public void Deactivate()
		{
			if (activated == true)
			{
				activated = false;

				foreach (var paintableTexture in paintableTextures)
				{
					if (paintableTexture != null)
					{
						paintableTexture.Deactivate();
					}
				}

				paintableTextures.Clear();

				foreach (var materialCloner in materialCloners)
				{
					if (materialCloner != null)
					{
						materialCloner.Deactivate();
					}
				}

				materialCloners.Clear();
			}
		}

		/// <summary>This allows you to clear the pixels of all activated P3dPaintableTexture components associated with this P3dPaintable with the specified color.</summary>
		public void ClearAll(Color color)
		{
			ClearAll(default(Texture), color);
		}

		/// <summary>This allows you to clear the pixels of all activated P3dPaintableTexture components associated with this P3dPaintable with the specified color and texture.</summary>
		public void ClearAll(Texture texture, Color color)
		{
			if (activated == true)
			{
				foreach (var paintableTexture in paintableTextures)
				{
					paintableTexture.Clear(texture, color);
				}
			}
		}

		/// <summary>This allows you to manually register a P3dPaintableTexture.</summary>
		public void Register(P3dPaintableTexture paintableTexture)
		{
			paintableTextures.Add(paintableTexture);
		}

		/// <summary>This allows you to manually unregister a P3dPaintableTexture.</summary>
		public void Unregister(P3dPaintableTexture paintableTexture)
		{
			paintableTextures.Remove(paintableTexture);
		}

		protected virtual void Awake()
		{
			if (activation == ActivationType.Awake && activated == false)
			{
				Activate();
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			if (activation == ActivationType.OnEnable && activated == false)
			{
				Activate();
			}

			P3dPaintableManager.GetOrCreateInstance();
		}

		protected virtual void Start()
		{
			if (activation == ActivationType.Start && activated == false)
			{
				Activate();
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dPaintable;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dPaintable_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			if (Any(tgts, t => t.Activated == true))
			{
				Info("This component has been activated.");
			}

			if (Any(tgts, t => t.Activated == true && Application.isPlaying == false))
			{
				Error("This component shouldn't be activated during edit mode. Deactivate it from the component context menu.");
			}

			Draw("activation", "This allows you to control when this component actually activates and becomes ready for painting. You probably don't need to change this.");

			Separator();

			if (Any(tgts, t => t.GetComponentInChildren<P3dPaintableTexture>() == null))
			{
				Warning("Your paintable doesn't have any paintable textures!");
			}

			if (Button("Add Material Cloner") == true)
			{
				Each(tgts, t => t.gameObject.AddComponent<P3dMaterialCloner>());
			}

			if (Button("Add Paintable Texture") == true)
			{
				Each(tgts, t => t.gameObject.AddComponent<P3dPaintableTexture>());
			}

			if (Button("Analyze Mesh") == true)
			{
				P3dMeshAnalysis.OpenWith(tgt.gameObject, tgt.PreparedMesh);
			}

			var mesh = P3dCommon.GetMesh(tgt.gameObject, tgt.PreparedMesh);

			if (mesh != null && mesh.isReadable == false)
			{
				Error("You must set the Read/Write Enabled setting in this object's Mesh import settings.");
			}

			Separator();

			if (DrawFoldout("Advanced", "Show advanced settings?") == true)
			{
				BeginIndent();
					Draw("baseScale", "If you want the paintable texture width/height to be multiplied by the scale of this GameObject, this allows you to set the scale where you want the multiplier to be 1.");
					Draw("includeScale", "Transform the mesh with its position, rotation, and scale? Some skinned mesh setups require this to be disabled.");
					Draw("useMesh", "This allows you to choose how the Mesh attached to the current Renderer is used when painting.\n\nAsIs = Use what is currently set in the renderer.\n\nAutoSeamFix = Use (or automatically generate) a seam-fixed version of the mesh currently set in the renderer.");
					Draw("hash", "The hash code for this model used for de/serialization of this instance.");
					Draw("otherRenderers", "If this material is used in multiple renderers, you can specify them here. This usually happens with different LOD levels.");
					Draw("onActivating");
					Draw("onActivated");
				EndIndent();
			}
		}
	}
}
#endif