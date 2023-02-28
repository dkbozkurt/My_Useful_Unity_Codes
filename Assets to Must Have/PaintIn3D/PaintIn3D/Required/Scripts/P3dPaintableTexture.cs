using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component allows you to make one texture on the attached Renderer paintable.
	/// NOTE: If the texture or texture slot you want to paint is part of a shared material (e.g. prefab material), then I recommend you add the P3dMaterialCloner component to make it unique.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dPaintableTexture")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Paintable Texture")]
	public class P3dPaintableTexture : MonoBehaviour
	{
		public enum UndoRedoType
		{
			None,
			FullTextureCopy,
			LocalCommandCopy
		}

		public enum SaveLoadType
		{
			Manual,
			Automatic,
			SemiManual
		}

		public enum MipType
		{
			Auto,
			On,
			Off
		}

		public enum FilterType
		{
			Auto = -1,
			Point,
			Bilinear,
			Trilinear
		}

		public enum AnisoType
		{
			Auto = -1,
			Off = 0,
			One = 1,
			Four = 4,
			Eight = 8,
			Sixteen = 16
		}

		public enum WrapType
		{
			Auto = -1,
			Repeat,
			Clamp,
			Mirror,
			MirrorOnce
		}

		public enum ExistingType
		{
			Ignore,
			Use,
			UseAndKeep
		}

		public enum ConversionType
		{
			None,
			Normal,
			Premultiply
		}

		[System.Serializable] public class PaintableTextureEvent : UnityEvent<P3dPaintableTexture> {}

		/// <summary>The material index and shader texture slot name that this component will paint.</summary>
		public P3dSlot Slot { set { slot = value; } get { return slot; } } [SerializeField] private P3dSlot slot = new P3dSlot(0, "_MainTex");

		/// <summary>The UV channel this texture is mapped to.</summary>
		public P3dCoord Coord { set { coord = value; } get { return coord; } } [UnityEngine.Serialization.FormerlySerializedAs("channel")] [SerializeField] private P3dCoord coord;

		/// <summary>The group you want to associate this texture with. Only painting components with a matching group can paint this texture. This allows you to paint multiple textures at the same time with different settings (e.g. Albedo + Normal).</summary>
		public P3dGroup Group { set { group = value; } get { return group; } } [SerializeField] private P3dGroup group;

		/// <summary>This allows you to set how this texture's state is stored, allowing you to perform undo and redo operations.
		/// FullTextureCopy = A full copy of your texture will be copied for each state. This allows you to quickly undo and redo, and works with animated skinned meshes, but it uses up a lot of texture memory.
		/// LocalCommandCopy = Each paint command will be stored in local space for each state. This allows you to perform unlimited undo and redo states with minimal memory usage, because the object will be repainted from scratch. However, performance will depend on how many states must be redrawn, and it may not work well with skinned meshes.</summary>
		public UndoRedoType UndoRedo { set { undoRedo = value; } get { return undoRedo; } } [UnityEngine.Serialization.FormerlySerializedAs("state")] [SerializeField] private UndoRedoType undoRedo;

		/// <summary>The amount of times this texture can have its paint operations undone.</summary>
		public int StateLimit { set { stateLimit = value; } get { return stateLimit; } } [SerializeField] private int stateLimit = 10;

		/// <summary>This allows you to control how this texture is loaded and saved.
		/// Manual = You must manually call <b>Save(SaveName)</b> and <b>Load(SaveName)</b> from code.
		/// Automatic = <b>Save(SaveName)</b> is called in <b>Deactivate/OnDestroy</b>, and <b>Load(SaveName)</b> is called in <b>Activate</b>.
		/// SemiManual = You can manually call the <b>Save()</b> and <b>Load()</b> methods from code or editor event, and the current <b>SaveName</b> will be used.</summary>
		public SaveLoadType SaveLoad { set { saveLoad = value; } get { return saveLoad; } } [SerializeField] private SaveLoadType saveLoad;

		/// <summary>If you want this texture to automatically save & load, then you can set the unique save name for it here.
		/// NOTE: This name should be unique, so this setting won't work properly with prefab spawning since all clones will share the same <b>SaveName</b>.</summary>
		public string SaveName { set { saveName = value; } get { return saveName; } } [SerializeField] private string saveName;

		/// <summary>The base width of the created texture.</summary>
		public int Width { set { width = value; } get { return width; } } [SerializeField] private int width = 512;

		/// <summary>The base height of the created texture.</summary>
		public int Height { set { height = value; } get { return height; } } [SerializeField] private int height = 512;

		/// <summary>When activated or cleared, this paintable texture will be given this texture, and then multiplied/tinted by the <b>Color</b>.
		/// None = White.</summary>
		public Texture Texture { set { texture = value; } get { return texture; } } [SerializeField] private Texture texture;

		/// <summary>When activated or cleared, this paintable texture will be given this color.
		/// NOTE: If <b>Texture</b> is set, then each pixel RGBA value will be multiplied/tinted by this color.</summary>
		public Color Color { set { color = value; } get { return color; } } [SerializeField] private Color color = Color.white;

		/// <summary>The format of the created texture.</summary>
		public RenderTextureFormat Format { set { format = value; } get { return format; } } [SerializeField] private RenderTextureFormat format;

		/// <summary>The <b>useMipMap</b> mode of the created texture.
		/// Auto = Copied from the <b>Texture</b>.</summary>
		public MipType MipMaps { set { mipMaps = value; } get { return mipMaps; } } [SerializeField] private MipType mipMaps = MipType.Auto;

		/// <summary>The <b>filterMode</b> of the created texture.
		/// Auto = Copied from the <b>Texture</b>.</summary>
		public FilterType Filter { set { filter = value; } get { return filter; } } [SerializeField] private FilterType filter = FilterType.Auto;

		/// <summary>The <b>anisoLevel</b> of the created texture.
		/// Auto = Copied from the <b>Texture</b>.</summary>
		public AnisoType Aniso { set { aniso = value; } get { return aniso; } } [SerializeField] private AnisoType aniso = AnisoType.Auto;

		/// <summary>The <b>wrapModeU</b> mode of the created texture.
		/// Auto = Copied from the <b>Texture</b>.</summary>
		public WrapType WrapU { set { wrapU = value; } get { return wrapU; } } [SerializeField] private WrapType wrapU = WrapType.Auto;

		/// <summary>The wrapModeV of the created texture.
		/// Auto = Copied from the <b>Texture</b>.</summary>
		public WrapType WrapV { set { wrapV = value; } get { return wrapV; } } [SerializeField] private WrapType wrapV = WrapType.Auto;

		/// <summary>If this component's <b>Texture</b> setting is none/null but the <b>Slot</b> texture is not none/null when this component activates, what should happen?
		/// Ignore = Nothing will happen.
		/// Use = This paintable texture will activate with the <b>Slot</b> texture.
		/// UseAndKeep = The same as <b>Use</b>, but the <b>Slot</b> texture will also be stored in the <b>Texture</b> setting.</summary>
		public ExistingType Existing { set { existing = value; } get { return existing; } } [SerializeField] private ExistingType existing = ExistingType.UseAndKeep;

		/// <summary>If you're painting special textures then they may need to be converted before use.
		/// Normal = Convert texture to be a normal map.
		/// Premultiply = Premultiply the RGB values.</summary>
		public ConversionType Conversion { set { conversion = value; } get { return conversion; } } [SerializeField] private ConversionType conversion;

		/// <summary>This allows you to restrict painting to specific areas.
		/// NOTE: This mask texture should match the UV mapping of the texture itself.</summary>
		public Texture LocalMaskTexture { set { localMaskTexture = value; } get { return localMaskTexture; } } [SerializeField] private Texture localMaskTexture;

		/// <summary>This allows you to specify which channel from the <b>LocalMaskTexture</b> texture will be used.</summary>
		public P3dChannel LocalMaskChannel { set { localMaskChannel = value; } get { return localMaskChannel; } } [SerializeField] private P3dChannel localMaskChannel;

		/// <summary>Some shaders require specific shader keywords to be enabled when adding new textures. If there is no texture in your selected slot then you may need to set this keyword.</summary>
		public string ShaderKeyword { set { shaderKeyword = value; } get { return shaderKeyword; } } [SerializeField] private string shaderKeyword;

		/// <summary>The hash code for this model used for de/serialization of this instance.</summary>
		public P3dHash Hash { set { hash = value; P3dSerialization.TryRegister(this, hash); } get { return hash; } } [SerializeField] protected P3dHash hash;

		/// <summary>If you want to paint a texture but don't want it to be applied to the specified material texture slot, then enable this.</summary>
		public bool IsDummy { set { isDummy = value; } get { return isDummy; } } [SerializeField] private bool isDummy;

		/// <summary>When you export this texture as a PNG asset from the in-editor painting window, the exported texture's GUID will be stored here so you can easily re-export it to the same file.</summary>
		public string Output { set { output = value; } get { return output; } } [SerializeField] private string output;

#if UNITY_EDITOR
		public Texture2D OutputTexture
		{
			get
			{
				return UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(UnityEditor.AssetDatabase.GUIDToAssetPath(output));
			}
		}
#endif

		/// <summary>This event is called after a paint command has been added to this texture. These commands will be executed at the end of the frame.</summary>
		public event System.Action<P3dCommand> OnAddCommand;

		/// <summary>This event is called after a paint command has been added to this texture. These commands will be executed at the end of the frame.</summary>
		public static event System.Action<P3dPaintableTexture, P3dCommand> OnAddCommandGlobal;

		/// <summary>This is invoked every time a paintable texture component is enabled.</summary>
		public static System.Action<P3dPaintableTexture> OnInstanceAdded;

		/// <summary>This is invoked every time a paintable texture component is disabled.</summary>
		public static System.Action<P3dPaintableTexture> OnInstanceRemoved;

		/// <summary>This event is called after this texture has been painted, allowing you to perform actions like counting the pixels after modification.
		/// Bool = Preview painting.</summary>
		public event System.Action<bool> OnModified;

		[SerializeField]
		private bool activated;

		[SerializeField]
		private RenderTexture current;

		[SerializeField]
		private RenderTexture preview;

		[System.NonSerialized]
		private List<P3dPaintableState> paintableStates = new List<P3dPaintableState>();

		[System.NonSerialized]
		private int stateIndex;

		[System.NonSerialized]
		private P3dPaintable paintable;

		[System.NonSerialized]
		private bool paintableSet;

		[System.NonSerialized]
		private Material material;

		[System.NonSerialized]
		private bool materialSet;

		[System.NonSerialized]
		private Texture oldTexture;

		[System.NonSerialized]
		private List<P3dCommand> paintCommands = new List<P3dCommand>();

		[System.NonSerialized]
		private List<P3dCommand> previewCommands = new List<P3dCommand>();

		[System.NonSerialized]
		private List<P3dCommand> localCommands = new List<P3dCommand>();

		[System.NonSerialized]
		private static List<P3dPaintableTexture> tempPaintableTextures = new List<P3dPaintableTexture>();

		/// <summary>This stores all active and enabled instances in the open scenes.</summary>
		public static LinkedList<P3dPaintableTexture> Instances { get { return instances; } } private static LinkedList<P3dPaintableTexture> instances = new LinkedList<P3dPaintableTexture>(); private LinkedListNode<P3dPaintableTexture> instancesNode;

		private static int _Buffer     = Shader.PropertyToID("_Buffer");
		private static int _BufferSize = Shader.PropertyToID("_BufferSize");

		/// <summary>This lets you know if this texture is activated and ready for painting. Activation is controlled by the associated P3dPaintable component.</summary>
		public bool Activated
		{
			get
			{
				return activated;
			}
		}

		/// <summary>This lets you know if there is at least one undo state this texture can be undone into.</summary>
		public bool CanUndo
		{
			get
			{
				return undoRedo != UndoRedoType.None && stateIndex > 0;
			}
		}

		/// <summary>This lets you know if there is at least one redo state this texture can be redone into.</summary>
		public bool CanRedo
		{
			get
			{
				return undoRedo != UndoRedoType.None && stateIndex < paintableStates.Count - 1;
			}
		}

		/// <summary>This property returns a list of all stored undo/redo states.</summary>
		public List<P3dPaintableState> States
		{
			get
			{
				return paintableStates;
			}
		}

		/// <summary>This tells you which undo/redo state is currently active inside the <b>States</b> list.</summary>
		public int StateIndex
		{
			get
			{
				return stateIndex;
			}
		}

		/// <summary>This tells you which material this paintable texture is currently stored in.
		/// NOTE: You may have to call <b>UpdateMaterial</b> if this is out of date.</summary>
		public Material Material
		{
			get
			{
				if (materialSet == false)
				{
					UpdateMaterial();
				}

				return material;
			}
		}

		/// <summary>This quickly gives you the P3dPaintable component associated with this paintable texture.</summary>
		public P3dPaintable Paintable
		{
			get
			{
				if (paintable == null)
				{
					paintable = GetComponent<P3dPaintable>();
				}

				return paintable;
			}
		}

		/// <summary>This gives you the current state of this paintable texture.
		/// NOTE: This will only exist if your texture is activated.
		/// NOTE: This is a <b>RenderTexture</b>, so you can't directly read it. Use the <b>GetReadableCopy()</b> method if you need to.
		/// NOTE: This doesn't include any preview painting information, access the Preview property if you need to.</summary>
		public RenderTexture Current
		{
			set
			{
				current = value;

				if (isDummy == false)
				{
					Material.SetTexture(slot.Name, current);
				}
			}

			get
			{
				return current;
			}
		}

		/// <summary>This gives you the current state of this paintable texture including any preview painting information.</summary>
		public RenderTexture Preview
		{
			get
			{
				return preview;
			}
		}

		/// <summary>This allows you to get a list of all paintable textures on a P3dModel/P3dPaintable with the specified group.</summary>
		public static List<P3dPaintableTexture> FilterAll(P3dModel model, P3dGroup group)
		{
			tempPaintableTextures.Clear();

			if (model.Paintable != null)
			{
				foreach (var paintableTexture in model.Paintable.PaintableTextures)
				{
					if (paintableTexture.group == group)
					{
						tempPaintableTextures.Add(paintableTexture);
					}
				}
			}

			return tempPaintableTextures;
		}

		/// <summary>This will clear all undo/redo texture states.</summary>
		[ContextMenu("Clear States")]
		public void ClearStates()
		{
			if (paintableStates != null)
			{
				for (var i = paintableStates.Count - 1; i >= 0; i--)
				{
					paintableStates[i].Pool();
				}

				paintableStates.Clear();

				stateIndex = 0;
			}
		}

		/// <summary>This will store a texture state so that it can later be undone. This should be called before you perform texture modifications.</summary>
		[ContextMenu("Store State")]
		public void StoreState()
		{
			if (activated == true)
			{
				// If this is the latest state, then don't store or trim future
				if (stateIndex != paintableStates.Count - 1)
				{
					TrimFuture();

					AddState();
				}

				if (undoRedo == UndoRedoType.FullTextureCopy)
				{
					TrimPast();
				}

				stateIndex = paintableStates.Count;
			}
		}

		/// <summary>This will revert the texture to a previous state, if you have an undo state stored.</summary>
		[ContextMenu("Undo")]
		public void Undo()
		{
			if (CanUndo == true)
			{
				// If we're undoing for the first time, store the current state so we can redo back to it
				if (stateIndex == paintableStates.Count)
				{
					AddState();
				}

				ClearCommands();

				stateIndex -= 1;

				switch (undoRedo)
				{
					case UndoRedoType.FullTextureCopy:
					{
						var paintableState = paintableStates[stateIndex];

						Replace(paintableState.Texture, Color.white);
					}
					break;

					case UndoRedoType.LocalCommandCopy:
					{
						RebuildFromCommands();
					}
					break;
				}

				NotifyOnModified(false);
			}
		}

		/// <summary>This will restore a previously undone texture state, if you've performed an undo.</summary>
		[ContextMenu("Redo")]
		public void Redo()
		{
			if (CanRedo == true)
			{
				ClearCommands();

				stateIndex += 1;

				switch (undoRedo)
				{
					case UndoRedoType.FullTextureCopy:
					{
						var paintableState = paintableStates[stateIndex];

						Replace(paintableState.Texture, Color.white);
					}
					break;

					case UndoRedoType.LocalCommandCopy:
					{
						RebuildFromCommands();
					}
					break;
				}

				NotifyOnModified(false);
			}
		}

		/// <summary>This allows you to set the <b>Color</b> based on an HTML style string (#FFF, #FF0055, yellow, red)</summary>
		public void SetColor(string html)
		{
			ColorUtility.TryParseHtmlString(html, out color);
		}

		public Vector2 GetCoord(ref P3dHit hit)
		{
			switch (coord)
			{
				case P3dCoord.First: return hit.First;
				case P3dCoord.Second: return hit.Second;
			}

			return default(Vector2);
		}

		public Vector2 GetCoord(ref RaycastHit hit)
		{
			switch (coord)
			{
				case P3dCoord.First: return hit.textureCoord;
				case P3dCoord.Second: return hit.textureCoord2;
			}

			return default(Vector2);
		}

		private bool StatesContainTextureOrCommands()
		{
			if (stateIndex >= 0 && stateIndex < paintableStates.Count)
			{
				for (var i = 0; i <= stateIndex; i++)
				{
					var paintableState = paintableStates[i];

					if (paintableState.Texture != null || paintableState.Commands.Count > 0)
					{
						return true;
					}
				}
			}

			return false;
		}

		private bool LastStateWithTextureOr0(ref int startIndex)
		{
			for (var i = paintableStates.Count - 1; i >= 0; i--)
			{
				if (paintableStates[i].Texture != null)
				{
					startIndex = i;

					return false;
				}
			}

			startIndex = 0;

			return true;
		}

		/// <summary>If this texture has local command copies stored, this method will rebuild the painted texture using these commands.
		/// This is useful if you want to change the base <b>Texture/Color</b>, and then re-apply your paint.</summary>
		private void RebuildFromCommands()
		{
			if (StatesContainTextureOrCommands() == true)
			{
				var startIndex = default(int);
				var startBlank = LastStateWithTextureOr0(ref startIndex);

				if (startBlank == true)
				{
					Clear(texture, color, false);
				}

				var posMatrix = transform.localToWorldMatrix;
				var rotMatrix = Matrix4x4.Rotate(posMatrix.rotation);

				for (var i = startIndex; i <= stateIndex; i++)
				{
					var paintableState = paintableStates[i];

					if (paintableState.Texture != null)
					{
						Clear(paintableState.Texture, Color.white, false);
					}
					else
					{
						var commandCount = paintableState.Commands.Count;

						if (commandCount > 0)
						{
							// We need to sort the commands for each state separately rather than use ExecuteCommands, otherwise the added commands will get interwoven
							paintableState.Commands.Sort(P3dCommand.Compare);

							for (var j = 0; j < commandCount; j++)
							{
								var worldCommand = paintableState.Commands[j].SpawnCopy();

								// Manually transform the command into world space with cached values
								//var worldCommand = paintableState.Commands[j].SpawnCopyWorld(transform);
								worldCommand.Transform(posMatrix, rotMatrix);

								// Manually add the command, bypassing notifications etc
								paintCommands.Add(worldCommand);
								//AddCommand(worldCommand);
							}
						}
					}
				}

				ExecuteCommands(false, false);
			}
			else
			{
				Clear(texture, color);
			}

			NotifyOnModified(false);
		}

		private void AddState()
		{
			var paintableState       = P3dPaintableState.Pop();
			var forceFullTextureCopy = false;

			if (paintableStates.Count == 0 && saveLoad == SaveLoadType.Automatic)
			{
				forceFullTextureCopy = true;
			}

			switch (undoRedo)
			{
				case UndoRedoType.FullTextureCopy:
				{
					paintableState.Write(current);
				}
				break;

				case UndoRedoType.LocalCommandCopy:
				{
					if (forceFullTextureCopy == true)
					{
						paintableState.Write(current, localCommands);
					}
					else
					{
						paintableState.Write(localCommands);
					}

					localCommands.Clear();
				}
				break;
			}

			paintableStates.Add(paintableState);
		}

		private void TrimFuture()
		{
			for (var i = paintableStates.Count - 1; i >= stateIndex; i--)
			{
				var paintableState = paintableStates[i];

				if (i == stateIndex)
				{
					localCommands.AddRange(paintableState.Commands);

					paintableState.Commands.Clear();
				}

				paintableState.Pool();

				paintableStates.RemoveAt(i);
			}
		}

		private void TrimPast()
		{
			for (var i = paintableStates.Count - stateLimit - 1; i >= 0; i--)
			{
				paintableStates[i].Pool();

				paintableStates.RemoveAt(i);
			}
		}

		/// <summary>You should call this after painting this paintable texture.</summary>
		public void NotifyOnModified(bool preview)
		{
			if (OnModified != null)
			{
				OnModified.Invoke(preview);
			}
		}

		/// <summary>This method returns a <b>Texture2D</b> copy of the current texture state, allowing you to read pixel values, etc.
		/// NOTE: This method can be slow if your texture is large.
		/// NOTE: A new texture is allocated each time you call this, so you must manually delete it when finished.</summary>
		public Texture2D GetReadableCopy(bool convertBack = false)
		{
			var copy = default(Texture2D);

			if (activated == true)
			{
				copy = P3dCommon.GetReadableCopy(current);
			}
			else
			{
				var desc         = new RenderTextureDescriptor(width, height, format, 0);
				var temp         = P3dCommon.GetRenderTexture(desc);
				var finalTexture = texture;

				if (finalTexture == null && existing != ExistingType.Ignore)
				{
					UpdateMaterial();

					finalTexture = material.GetTexture(slot.Name);
				}

				P3dCommandReplace.Blit(temp, finalTexture, color);

				copy = P3dCommon.GetReadableCopy(temp);

				P3dCommon.ReleaseRenderTexture(temp);
			}

			if (convertBack == true && conversion == ConversionType.Normal)
			{
				for (var y = 0; y < copy.height; y++)
				{
					for (var x = 0; x < copy.width; x++)
					{
						var color = copy.GetPixel(x, y);

						copy.SetPixel(x, y, new Color(color.r, color.g, color.b, 1.0f));
					}
				}

				copy.Apply();
			}

			return copy;
		}

		/// <summary>This method returns the current texture state as a PNG byte array.
		/// NOTE: This method can be slow if your texture is large.</summary>
		public byte[] GetPngData(bool convertBack = false)
		{
			var tempTexture = GetReadableCopy(convertBack);

			if (tempTexture != null)
			{
				var data = tempTexture.EncodeToPNG();

				CwHelper.Destroy(tempTexture);

				return data;
			}

			return null;
		}

		/// <summary>This method will clear the current texture state with the current <b>Texture</b> and <b>Color</b> values.
		/// NOTE: This component must be activated, and this method will not resize the current texture.</summary>
		[ContextMenu("Clear")]
		public void Clear()
		{
			Clear(texture, color);
		}

		/// <summary>This method will clear the current texture state with the specified texture and color.
		/// NOTE: This component must be activated, and this method will not resize the current texture.</summary>
		public void Clear(Texture texture, Color tint, bool updateMips = true)
		{
			if (activated == true)
			{
				if (conversion == ConversionType.Normal)
				{
					P3dBlit.Normal(current, texture);
				}
				else if (conversion == ConversionType.Premultiply)
				{
					P3dBlit.Premultiply(current, texture, tint);
				}
				else
				{
					P3dCommandReplace.Blit(current, texture, tint);
				}

				if (updateMips == true && current.useMipMap == true) current.GenerateMips();
			}
		}

		/// <summary>This method will replace the current texture state with the current <b>Texture</b> and <b>Color</b> values, including size.
		/// NOTE: This component must be activated</summary>
		[ContextMenu("Replace")]
		public void Replace()
		{
			Replace(texture, color);
		}

		/// <summary>This method will resize the current texture state based on the specified texture, and then replace its contents with the specified texture and color.
		/// NOTE: This component must be activated.</summary>
		public void Replace(Texture texture, Color tint)
		{
			if (texture != null)
			{
				Resize(texture.width, texture.height, false);
			}
			else
			{
				Resize(width, height, false);
			}

			Clear(texture, tint);
		}

		/// <summary>This method will resize the current texture state with the specified width and height.
		/// NOTE: This component must be activated.</summary>
		public bool Resize(int width, int height, bool copyContents = true)
		{
			if (activated == true)
			{
				if (current.width != width || current.height != height)
				{
					var descriptor = current.descriptor;

					descriptor.width  = width;
					descriptor.height = height;

					var newCurrent = P3dCommon.GetRenderTexture(descriptor, current);

					if (copyContents == true)
					{
						P3dCommandReplace.Blit(newCurrent, current, Color.white);

						if (newCurrent.useMipMap == true) newCurrent.GenerateMips();
					}

					P3dCommon.ReleaseRenderTexture(current);

					current = newCurrent;

					return true;
				}
			}

			return false;
		}

		/// <summary>This method will save the current texture state to PlayerPrefs using the current <b>SaveName</b>.</summary>
		[ContextMenu("Save")]
		public void Save()
		{
			Save(saveName);
		}

		/// <summary>This will save the current texture state with the specified save name.</summary>
		public void Save(string saveName)
		{
			if (activated == true && string.IsNullOrEmpty(saveName) == false)
			{
				P3dCommon.SaveBytes(saveName, GetPngData());
			}
		}

		/// <summary>This method will replace the current texture state with the data saved at <b>SaveName</b>.</summary>
		[ContextMenu("Load")]
		public void Load()
		{
			Load(saveName);
		}

		/// <summary>This method will replace the current texture state with the data saved at the specified save name.</summary>
		public void Load(string saveName, bool replace = true)
		{
			if (activated == true)
			{
				LoadFromData(P3dCommon.LoadBytes(saveName));
			}
		}

		/// <summary>This method will replace the current texture state with the specified image data (e.g. png).</summary>
		public void LoadFromData(byte[] data, bool allowResize = true)
		{
			if (data != null && data.Length > 0)
			{
				var tempTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false, false);

				tempTexture.LoadImage(data);

				if (allowResize == true)
				{
					Replace(tempTexture, Color.white);
				}
				else
				{
					Clear(tempTexture, Color.white);
				}

				CwHelper.Destroy(tempTexture);
			}
		}

		/// <summary>If you last painted using preview painting and you want to hide the preview painting, you can call this method to force the texture to go back to its current state.</summary>
		public void HidePreview()
		{
			if (activated == true && current != null && material != null && isDummy == false)
			{
				material.SetTexture(slot.Name, current);
			}
		}

		/// <summary>This automatically calls <b>HidePreview</b> on all active and enabled paintable textures.</summary>
		public static void HideAllPreviews()
		{
			foreach (var instance in instances)
			{
				instance.HidePreview();
			}
		}

		/// <summary>This will clear save data with the current <b>SaveName</b>.</summary>
		[ContextMenu("Clear Save")]
		public void ClearSave()
		{
			P3dCommon.ClearSave(saveName);
		}

		/// <summary>This will clear save data with the specified save name.</summary>
		public static void ClearSave(string saveName)
		{
			P3dCommon.ClearSave(saveName);
		}

		/// <summary>If you modified the slot material index, then call this to update the cached material.</summary>
		[ContextMenu("Update Material")]
		public void UpdateMaterial()
		{
			if (Paintable != null)
			{
				material    = P3dCommon.GetMaterial(paintable.CachedRenderer, slot.Index);
				materialSet = true;
			}
			else
			{
				materialSet = false;
			}
		}

		/// <summary>If the current slot has a texture, this allows you to copy the width and height from it.</summary>
		[ContextMenu("Copy Size")]
		public void CopySize()
		{
			var texture = Slot.FindTexture(gameObject);

			if (texture != null)
			{
				width  = texture.width;
				height = texture.height;
			}
		}

		/// <summary>This copies the texture from the current slot.</summary>
		[ContextMenu("Copy Texture")]
		public void CopyTexture()
		{
			Texture = Slot.FindTexture(gameObject);
		}

#if UNITY_EDITOR
		[ContextMenu("Activate", true)]
		private bool ActivateValidate()
		{
			return Application.isPlaying == true && activated == false;
		}
#endif

		/// <summary>This allows you to manually activate this paintable texture.
		/// NOTE: This will automatically be called by the associated P3dPaintable component when it activates.</summary>
		[ContextMenu("Activate")]
		public void Activate()
		{
			if (activated == false)
			{
				paintable = GetComponentInParent<P3dPaintable>();

				if (paintable != null)
				{
					UpdateMaterial();

					if (material != null)
					{
						oldTexture = material.GetTexture(slot.Name);

						var finalWidth   = width;
						var finalHeight  = height;
						var finalTexture = texture;

						paintable.ScaleSize(ref finalWidth, ref finalHeight);

						if (finalTexture == null && existing != ExistingType.Ignore)
						{
							finalTexture = oldTexture;

							if (existing == ExistingType.UseAndKeep)
							{
								texture = oldTexture;
							}
						}

						if (string.IsNullOrEmpty(shaderKeyword) == false)
						{
							material.EnableKeyword(shaderKeyword);
						}

						var desc = new RenderTextureDescriptor(width, height, format, 0);

						desc.autoGenerateMips = false;

						if (mipMaps == MipType.Auto)
						{
							if (finalTexture != null)
							{
								desc.useMipMap = P3dCommon.HasMipMaps(finalTexture);
							}
						}
						else
						{
							desc.useMipMap = mipMaps == MipType.On;
						}

						current = P3dCommon.GetRenderTexture(desc);

						if (filter == FilterType.Auto)
						{
							if (finalTexture != null)
							{
								current.filterMode = finalTexture.filterMode;
							}
						}
						else
						{
							current.filterMode = (FilterMode)filter;
						}

						if (aniso == AnisoType.Auto)
						{
							if (finalTexture != null)
							{
								current.anisoLevel = finalTexture.anisoLevel;
							}
						}
						else
						{
							current.anisoLevel = (int)aniso;
						}

						if (wrapU == WrapType.Auto)
						{
							if (finalTexture != null)
							{
								current.wrapModeU = finalTexture.wrapModeU;
							}
						}
						else
						{
							current.wrapModeU = (TextureWrapMode)wrapU;
						}

						if (wrapV == WrapType.Auto)
						{
							if (finalTexture != null)
							{
								current.wrapModeV = finalTexture.wrapModeV;
							}
						}
						else
						{
							current.wrapModeV = (TextureWrapMode)wrapV;
						}

						activated = true;

						Clear(finalTexture, color);

						if (isDummy == false)
						{
							material.SetTexture(slot.Name, current);
						}

						if (saveLoad == SaveLoadType.Automatic && string.IsNullOrEmpty(saveName) == false)
						{
							Load();
						}

						NotifyOnModified(false);
					}
				}
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Deactivate", true)]
		private bool DeactivateValidate()
		{
			return activated;
		}
#endif

		[ContextMenu("Deactivate")]
		public void Deactivate()
		{
			if (activated == true)
			{
				if (saveLoad == SaveLoadType.Automatic)
				{
					Save();
				}

				activated = false;

				if (material != null)
				{
					if (isDummy == false)
					{
						material.SetTexture(slot.Name, oldTexture);
					}

					current = P3dCommon.ReleaseRenderTexture(current);
					preview = P3dCommon.ReleaseRenderTexture(preview);
				}

				ClearCommands();
				ClearStates();
			}
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);

			if (OnInstanceAdded != null)
			{
				OnInstanceAdded.Invoke(this);
			}

			P3dSerialization.TryRegister(this, hash);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode); instancesNode = null;

			if (OnInstanceRemoved != null)
			{
				OnInstanceRemoved.Invoke(this);
			}
		}

		protected virtual void OnApplicationPause(bool paused)
		{
			if (paused == true)
			{
				if (activated == true)
				{
					if (saveLoad == SaveLoadType.Automatic && string.IsNullOrEmpty(saveName) == false)
					{
						Save();
					}
				}
			}
		}

		protected virtual void OnDestroy()
		{
			if (activated == true)
			{
				if (saveLoad == SaveLoadType.Automatic && string.IsNullOrEmpty(saveName) == false)
				{
					Save();
				}

				P3dCommon.ReleaseRenderTexture(current);
				P3dCommon.ReleaseRenderTexture(preview);

				ClearStates();
			}

			P3dSerialization.TryRegister(this, default(P3dHash));
		}

		/// <summary>This will add a paint command to this texture's paint stack. The paint stack will be executed at the end of the current frame.</summary>
		public void AddCommand(P3dCommand command)
		{
			if (command.Preview == true)
			{
				command.Index = previewCommands.Count;

				previewCommands.Add(command);
			}
			else
			{
				command.Index = paintCommands.Count;

				paintCommands.Add(command);

				if (undoRedo == UndoRedoType.LocalCommandCopy && command.Preview == false)
				{
					var localCommand = command.SpawnCopyLocal(transform);

					localCommand.Index = localCommands.Count;

					localCommands.Add(localCommand);
				}
			}

			if (OnAddCommand != null)
			{
				OnAddCommand.Invoke(command);
			}

			if (OnAddCommandGlobal != null)
			{
				OnAddCommandGlobal.Invoke(this, command);
			}
		}

		/// <summary>This lets you know if there are paint commands in this paintable texture's paint stack.</summary>
		public bool CommandsPending
		{
			get
			{
				return paintCommands.Count + previewCommands.Count > 0;
			}
		}

		public void ClearCommands()
		{
			for (var i = previewCommands.Count - 1; i >= 0; i--)
			{
				previewCommands[i].Pool();
			}

			previewCommands.Clear();

			for (var i = paintCommands.Count - 1; i >= 0; i--)
			{
				paintCommands[i].Pool();
			}

			paintCommands.Clear();

			for (var i = localCommands.Count - 1; i >= 0; i--)
			{
				localCommands[i].Pool();
			}

			localCommands.Clear();
		}

		/// <summary>This allows you to manually execute all commands in the paint stack.
		/// This is useful if you need to modify the state of your object before the end of the frame.</summary>
		public void ExecuteCommands(bool sendNotifications, bool doSort)
		{
			if (activated == true)
			{
				var hidePreview = true;

				if (CommandsPending == true)
				{
					var oldActive = RenderTexture.active;

					// Paint
					if (paintCommands.Count > 0)
					{
						if (doSort == true)
						{
							paintCommands.Sort(P3dCommand.Compare);
						}

						ExecuteCommands(paintCommands, sendNotifications, current, ref preview);
					}

					var swap = preview;

					preview = null;

					// Preview
					if (previewCommands.Count > 0)
					{
						preview = swap;
						swap    = null;

						if (preview == null)
						{
							preview = P3dCommon.GetRenderTexture(current);
						}

						hidePreview = false;

						preview.DiscardContents();

						Graphics.Blit(current, preview);

						if (doSort == true)
						{
							previewCommands.Sort(P3dCommand.Compare);
						}

						ExecuteCommands(previewCommands, sendNotifications, preview, ref swap);
					}

					P3dCommon.ReleaseRenderTexture(swap);

					RenderTexture.active = oldActive;
				}

				if (hidePreview == true)
				{
					preview = P3dCommon.ReleaseRenderTexture(preview);
				}

				if (isDummy == false && Material != null)
				{
					material.SetTexture(slot.Name, preview != null ? preview : current); // NOTE: Property
				}
			}
		}

		private void ExecuteCommands(List<P3dCommand> commands, bool sendNotifications, RenderTexture main, ref RenderTexture swap)
		{
			RenderTexture.active = main;

			// Loop through all commands
			foreach (var command in commands)
			{
				P3dModel commandModel;
				Material commandMaterial;

				if (command.Model.TryGetInstance(out commandModel) == true && command.Material.TryGetInstance(out commandMaterial) == true)
				{
					var preparedMesh    = default(Mesh);
					var preparedMatrix  = Matrix4x4.identity;
					var preparedSubmesh = 0;
					var preparedCoord   = P3dCoord.First;

					if (command.RequireMesh == true)
					{
						commandModel.GetPrepared(ref preparedMesh, ref preparedMatrix, coord);

						preparedSubmesh = command.Submesh;
						preparedCoord   = coord;
					}
					else
					{
						preparedMesh = P3dCommon.GetQuadMesh();
					}

					//if (doubleBuffer == true)
					{
						if (swap == null)
						{
							swap = P3dCommon.GetRenderTexture(main);
						}

						P3dBlit.Texture(swap, preparedMesh, preparedSubmesh, main, preparedCoord);

						commandMaterial.SetTexture(_Buffer, swap);
						commandMaterial.SetVector(_BufferSize, new Vector2(swap.width, swap.height));
					}

					command.Apply(commandMaterial);

					RenderTexture.active = main;

					P3dCommon.Draw(commandMaterial, command.Pass, preparedMesh, preparedMatrix, preparedSubmesh, preparedCoord);
				}

				command.Pool();
			}

			commands.Clear();

			if (main.useMipMap == true)
			{
				main.GenerateMips();
			}

			if (sendNotifications == true)
			{
				NotifyOnModified(commands == previewCommands);
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dPaintableTexture;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dPaintableTexture_Editor : CwEditor
	{
		private static List<P3dPaintableTexture> tempPaintableTextures = new List<P3dPaintableTexture>();

		public void TestOverlap(P3dPaintableTexture[] tgts, ref bool slot, ref bool group)
		{
			foreach (var tgt in tgts)
			{
				tgt.GetComponents(tempPaintableTextures);

				foreach (var other in tempPaintableTextures)
				{
					if (other != tgt)
					{
						if (other.Slot == tgt.Slot && other.IsDummy == false && tgt.IsDummy == false)
						{
							slot |= true;
						}

						if (other.Slot.Index == tgt.Slot.Index && other.Group == tgt.Group)
						{
							group |= true;
						}
					}
				}
			}
		}

		public bool SlotHasTilingOrOffset(P3dPaintableTexture[] tgts)
		{
			foreach (var tgt in tgts)
			{
				if (tgt.Slot.IsTransformed(tgt.gameObject) == true)
				{
					return true;
				}
			}

			return false;
		}

		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			var overlapSlot  = false;
			var overlapGroup = false;

			TestOverlap(tgts, ref overlapSlot, ref overlapGroup);

			if (Any(tgts, t => t.Activated == true))
			{
				Info("This component has been activated.");
			}

			if (Any(tgts, t => t.Activated == true && Application.isPlaying == false))
			{
				Error("This component shouldn't be activated during edit mode. Deactivate it from the component context menu.");
			}

			if (Any(tgts, t => t.Activated == false && t.Paintable != null && t.Paintable.Activated == true))
			{
				Warning("This component isn't activated, but the P3dPaintable has been, so you must manually activate this.");
			}

			var slotTitle   = Any(tgts, t => t.IsDummy) == true ? "Slot (DUMMY)" : "Slot";
			var showTexture = DrawExpand("slot", "The material index and shader texture slot name that this component will paint.", slotTitle);
			if (SlotHasTilingOrOffset(tgts) == true)
			{
				Warning("This slot uses Tiling and/or Offset values, which are not supported.");
			}
			if (overlapSlot == true)
			{
				Error("This slot is used by multiple P3dPaintableTexture components.");
			}
			if (showTexture == true)
			{
				BeginIndent();
					BeginDisabled();
						EditorGUI.ObjectField(Reserve(18), new GUIContent("Texture", "This is the current texture in the specified texture slot."), tgt.Slot.FindTexture(tgt.gameObject), typeof(Texture), false);
					EndDisabled();
				EndIndent();
			}
			Draw("coord", "The UV channel this texture is mapped to.");

			Separator();

			Draw("group", "The group you want to associate this texture with. Only painting components with a matching group can paint this texture. This allows you to paint multiple textures at the same time with different settings (e.g. Albedo + Normal).");
			if (overlapGroup == true)
			{
				Error("This group is used by multiple P3dPaintableTexture components.");
			}
			Draw("undoRedo", "This allows you to set how this texture's state is stored, allowing you to perform undo and redo operations.\n\nFullTextureCopy = A full copy of your texture will be copied for each state. This allows you to quickly undo and redo, and works with animated skinned meshes, but it uses up a lot of texture memory.\n\nLocalCommandCopy = Each paint command will be stored in local space for each state. This allows you to perform unlimited undo and redo states with minimal memory usage, because the object will be repainted from scratch. However, performance will depend on how many states must be redrawn, and it may not work well with skinned meshes.");
			if (Any(tgts, t => t.UndoRedo == P3dPaintableTexture.UndoRedoType.FullTextureCopy))
			{
				BeginIndent();
					Draw("stateLimit", "The amount of times this texture can have its paint operations undone.");
				EndIndent();
			}
			DrawSaveName(tgts);

			Separator();

			DrawSize(tgts);
			DrawTexture(tgts);
			Draw("color", "When activated or cleared, this paintable texture will be given this color.\n\nNOTE: If Texture is set, then each pixel RGBA value will be multiplied/tinted by this color.");

			Separator();

			if (DrawFoldout("Advanced", "Show advanced settings?") == true)
			{
				BeginIndent();
					Draw("format", "The format of the created texture.");
					Draw("mipMaps", "The useMipMap mode of the created texture.\n\nAuto = Copied from the Texture.");
					Draw("filter", "The filterMode of the created texture.\n\nAuto = Copied from the Texture.");
					Draw("aniso", "The anisoLevel of the created texture.\n\nAuto = Copied from the Texture.");
					Draw("wrapU", "The wrapModeU of the created texture.\n\nAuto = Copied from the Texture.");
					Draw("wrapV", "The wrapModeV of the created texture.\n\nAuto = Copied from the Texture.");
					Draw("existing", "If this component's Texture setting is none/null but the Slot texture is not none/null when this component activates, what should happen?\n\nIgnore = Nothing will happen.\n\nUse = This paintable texture will activate with the Slot texture.\n\nUseAndKeep = The same as Use, but the Slot texture will also be stored in the Texture setting.");
					Draw("conversion", "If you're painting special textures then they may need to be converted before use.\n\nNormal = Convert texture to be a normal map.\n\nPremultiply = Premultiply the RGB values.");
					DrawLocalMask();
					Draw("shaderKeyword", "Some shaders require specific shader keywords to be enabled when adding new textures. If there is no texture in your selected slot then you may need to set this keyword.");
					Draw("hash", "The hash code for this model used for de/serialization of this instance.");
					Draw("isDummy", "If you want to paint a texture but don't want it to be applied to the specified material texture slot, then enable this.");
				EndIndent();
			}

			if (Any(tgts, t => t.Conversion != P3dPaintableTexture.ConversionType.Normal && (t.Slot.Name.Contains("Bump") == true || t.Slot.Name.Contains("Normal") == true)))
			{
				Warning("If you're painting a normal map, then you should enable the Normal setting in the advanced menu.");
			}
		}

		private void DrawLocalMask()
		{
			var propA    = serializedObject.FindProperty("localMaskTexture");
			var propB    = serializedObject.FindProperty("localMaskChannel");
			var contentA = new GUIContent("Local Mask", "This allows you to restrict painting to specific areas.\n\nNOTE: This mask texture should match the UV mapping of the texture itself.");

			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(propA, contentA);
				EditorGUILayout.PropertyField(propB, GUIContent.none, false, GUILayout.Width(70));
			EditorGUILayout.EndHorizontal();
		}

		private void DrawSaveName(TARGET[] tgts)
		{
			/*
			var rect  = Reserve();
			var rectL = rect; rectL.xMax -= 50;
			var rectR = rect; rectR.xMin = rectR.xMax - 48;
			var prop  = serializedObject.FindProperty("saveName");

			EditorGUI.PropertyField(rectL, prop, new GUIContent("Save Name", "When activated or cleared, this paintable texture will be given this texture, and then multiplied/tinted by the Color.\n\nNone = White."));

			BeginDisabled(All(CannotCopyTexture));
				if (GUI.Button(rectR, new GUIContent("+Wildcard", "Add a wildcard to the save name?"), EditorStyles.miniButton) == true)
				{
					var menu = new GenericMenu();

					menu.AddItem(new GUIContent(), false, );

					menu.ShowAsContext();
				}
			EndDisabled();
			*/
			Draw("saveLoad", "This allows you to control how this texture is loaded and saved.\n\nManual = You must manually call Save(SaveName) and Load(SaveName) from code.\n\nAutomatic = Save(SaveName) is called in Deactivate/OnDestroy, and Load(SaveName) is called in Activate.\n\nSemiManual = You can manually call the Save() and Load() methods from code or editor event, and the current SaveName will be used.");
			if (Any(tgts, t => t.SaveLoad != P3dPaintableTexture.SaveLoadType.Manual))
			{
				BeginIndent();
					Draw("saveName", "If you want this texture to automatically save & load, then you can set the unique save name for it here.\n\nNOTE: This name should be unique, so this setting won't work properly with prefab spawning since all clones will share the same SaveName.");
				EndIndent();
			}
		}

		private void DrawSize(TARGET[] tgts)
		{
			var rect  = Reserve();
			var rectL = rect; rectL.width = EditorGUIUtility.labelWidth;

			EditorGUI.LabelField(rectL, new GUIContent("Size", "This allows you to control the width and height of the texture when it gets activated."));

			rect.xMin += EditorGUIUtility.labelWidth;

			var rectR = rect; rectR.xMin = rectR.xMax - 48;
			var rectW = rect; rectW.xMax -= 50; rectW.xMax -= rectW.width / 2 + 1;
			var rectH = rect; rectH.xMax -= 50; rectH.xMin += rectH.width / 2 + 1;

			EditorGUI.PropertyField(rectW, serializedObject.FindProperty("width"), GUIContent.none);
			EditorGUI.PropertyField(rectH, serializedObject.FindProperty("height"), GUIContent.none);

			BeginDisabled(All(tgts, CannotScale));
				if (GUI.Button(rectR, new GUIContent("Copy", "Copy the width and height from the current slot?"), EditorStyles.miniButton) == true)
				{
					Undo.RecordObjects(targets, "Copy Sizes");

					Each(tgts, t => t.CopySize(), true);
				}
			EndDisabled();
		}

		private void DrawTexture(TARGET[] tgts)
		{
			var rect  = Reserve();
			var rectL = rect; rectL.xMax -= 50;
			var rectR = rect; rectR.xMin = rectR.xMax - 48;

			EditorGUI.PropertyField(rectL, serializedObject.FindProperty("texture"), new GUIContent("Texture", "When activated or cleared, this paintable texture will be given this texture, and then multiplied/tinted by the Color.\n\nNone = White."));

			BeginDisabled(All(tgts, CannotCopyTexture));
				if (GUI.Button(rectR, new GUIContent("Copy", "Copy the texture from the current slot?"), EditorStyles.miniButton) == true)
				{
					Undo.RecordObjects(targets, "Copy Textures");

					Each(tgts, t => t.CopyTexture(), true);
				}
			EndDisabled();
		}

		private bool CannotScale(P3dPaintableTexture paintableTexture)
		{
			var texture = paintableTexture.Slot.FindTexture(paintableTexture.gameObject);

			if (texture != null)
			{
				if (texture.width != paintableTexture.Width || texture.height != paintableTexture.Height)
				{
					return false;
				}
			}

			return true;
		}

		private bool CannotCopyTexture(P3dPaintableTexture paintableTexture)
		{
			var texture = paintableTexture.Slot.FindTexture(paintableTexture.gameObject);

			if (texture != null && texture != paintableTexture.Texture)
			{
				return false;
			}

			return true;
		}
	}
}
#endif