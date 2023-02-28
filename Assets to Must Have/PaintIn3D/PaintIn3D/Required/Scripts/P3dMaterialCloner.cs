using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component allows you to duplicate a material before you paint on it. This is useful if the material is shared between multiple GameObjects (e.g. prefabs).</summary>
	[RequireComponent(typeof(Renderer))]
	[RequireComponent(typeof(P3dPaintable))]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dMaterialCloner")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Material Cloner")]
	public class P3dMaterialCloner : MonoBehaviour
	{
		/// <summary>The material index that will be cloned. This matches the Materials list in your MeshRenderer/SkinnedMeshRenderer, where 0 is the first material.</summary>
		public int Index { set { index = value; } get { return index; } } [SerializeField] private int index;

		[System.NonSerialized]
		private Renderer cachedRenderer;

		[System.NonSerialized]
		private bool cachedRendererSet;

		[System.NonSerialized]
		private P3dPaintable cachedPaintable;

		[System.NonSerialized]
		private bool cachedPaintableSet;

		[SerializeField]
		private bool activated;

		[SerializeField]
		private Material current;

		[SerializeField]
		private Material original;

		public Renderer CachedRenderer
		{
			get
			{
				if (cachedRendererSet == false)
				{
					cachedRenderer    = GetComponent<Renderer>();
					cachedRendererSet = true;
				}

				return cachedRenderer;
			}
		}

		public P3dPaintable CachedPaintable
		{
			get
			{
				if (cachedPaintableSet == false)
				{
					cachedPaintable    = GetComponent<P3dPaintable>();
					cachedPaintableSet = true;
				}

				return cachedPaintable;
			}
		}

		public Material Original
		{
			get
			{
				return original;
			}
		}

		public Material Current
		{
			get
			{
				return current;
			}
		}

		/// <summary>This lets you know if this component has already been activated and has executed.</summary>
		public bool Activated
		{
			get
			{
				return activated;
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Activate", true)]
		private bool ActivateValidate()
		{
			return Application.isPlaying == true && activated == false;
		}
#endif

		/// <summary>This allows you to manually activate this component, cloning the specified material.
		/// NOTE: This will automatically be called from P3dPaintable to clone the material.</summary>
		[ContextMenu("Activate")]
		public void Activate()
		{
			if (activated == false && index >= 0)
			{
				var materials = CachedRenderer.sharedMaterials;

				if (index < materials.Length)
				{
					original = materials[index];

					if (original != null)
					{
						activated = true;
						current   = Instantiate(original);

						ReplaceAll(original, current);
					}
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

				ReplaceAll(current, original);

				current = CwHelper.Destroy(current);
			}
		}

		public void ReplaceAll(Material from, Material to)
		{
			var paintable = CachedPaintable;

			Replace(CachedRenderer, CachedRenderer.sharedMaterials, from, to);

			if (paintable.OtherRenderers != null)
			{
				for (var i = paintable.OtherRenderers.Count - 1; i >= 0; i--)
				{
					var otherRenderer = paintable.OtherRenderers[i];

					if (otherRenderer != null)
					{
						Replace(otherRenderer, otherRenderer.sharedMaterials, from, to);
					}
				}
			}
		}

		private void Replace(Renderer renderer, Material[] materials, Material oldMaterial, Material newMaterial)
		{
			var replaced = false;

			for (var i = materials.Length - 1; i >= 0; i--)
			{
				var material = materials[i];

				if (material == oldMaterial)
				{
					materials[i] = newMaterial;

					replaced = true;
				}
			}

			if (replaced == true)
			{
				renderer.sharedMaterials = materials;
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dMaterialCloner;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dMaterialCloner_Editor : CwEditor
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

			if (Any(tgts, t => t.Activated == false && t.CachedPaintable.Activated == true))
			{
				Warning("This component isn't activated, but the P3dPaintable has been, so you must manually activate this.");
			}

			BeginError(Any(tgts, t => t.Index < 0 || t.Index >= t.GetComponent<Renderer>().sharedMaterials.Length));
				var showMaterial = DrawExpand("index", "The material index that will be cloned. This matches the Materials list in your MeshRenderer/SkinnedMeshRenderer, where 0 is the first material.");
			EndError();
			if (showMaterial == true)
			{
				BeginIndent();
					BeginDisabled();
						EditorGUILayout.ObjectField(new GUIContent("Material", "This is the current material at the specified material index."), P3dCommon.GetMaterial(tgt.CachedRenderer, tgt.Index), typeof(Material), false);
					EndDisabled();
				EndIndent();
			}
		}
	}
}
#endif