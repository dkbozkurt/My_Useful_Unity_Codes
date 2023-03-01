using System.Collections.Generic;
using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component shows you how to listen for and store paint commands added to any <b>P3dPaintableTexture</b> component in the scene.
	/// This component can then reset each paintable texture, and randomly apply one of the recorded paint commands.
	/// NOTE: For a paint command to be able to be de/serialized, all P3dModel, P3dPaintable, P3dPaintableTexture, and Texture instance associated with the paint command must be registered with a unique hash code that will be the same across all application runs and clients.
	/// NOTE: The hash codes for the P3d___ components can be set using the <b>Advanced/Hash</b> setting, but the <b>Texture</b> instances must be done separately using either the <b>P3dTextureHash</b> component, or manual calls to <b>P3dSerialization.TryRegister(texture)</b> before you attempt to de/serialize a paint command.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dCommandSerialization")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Command Serialization")]
	public class P3dCommandSerialization : MonoBehaviour
	{
		[System.Serializable]
		public struct CommandData
		{
			public P3dPaintableTexture PaintableTexture;

			[SerializeReference]
			public P3dCommand LocalCommand;
		}

		/// <summary>Should this component listen for added commands?</summary>
		public bool Listening { set { listening = value; } get { return listening; } } [SerializeField] private bool listening = true;

		/// <summary>All paintable textures and associated commands will be stored here.</summary>
		[SerializeField] private List<CommandData> commandDatas = new List<CommandData>();

		/// <summary>This method will pool and clear all commands.</summary>
		[ContextMenu("Clear")]
		public void Clear()
		{
			foreach (var commandData in commandDatas)
			{
				commandData.LocalCommand.Pool();
			}

			commandDatas.Clear();
		}

		/// <summary>This method will clear all paintable textures, and apply one random paint command that was recorded.</summary>
		[ContextMenu("Rebuild Random Command")]
		public void RebuildRandomCommand()
		{
			// Ignore added commands while this method is running
			var oldListening = listening;

			listening = false;

			// Loop through all paintable textures, and reset them to their original state
			foreach (var paintableTexture in P3dPaintableTexture.Instances)
			{
				paintableTexture.Clear();
			}

			// Randomly pick one command data
			if (commandDatas.Count > 0)
			{
				var index       = Random.Range(0, commandDatas.Count);
				var commandData = commandDatas[index];

				// Make sure it's still valid
				if (commandData.PaintableTexture != null)
				{
					// Convert the command to world space
					var command = commandData.LocalCommand.SpawnCopyWorld(commandData.PaintableTexture.transform);

					// Apply it to its paintable texture
					commandData.PaintableTexture.AddCommand(command);

					// Pool
					command.Pool();
				}
			}

			// Revert listening state
			listening = oldListening;
		}

		[ContextMenu("Serialize And Deserialize")]
		public void SerializeAndDeserialize()
		{
			// In this example the JSON data is merely created then loaded, but in your project you would probably want to save it to a file or send it over the network
			var json = JsonUtility.ToJson(this);

			JsonUtility.FromJsonOverwrite(json, this);
		}

		protected virtual void OnEnable()
		{
			P3dPaintableTexture.OnAddCommandGlobal += HandleAddCommandGlobal;
		}

		protected virtual void OnDisable()
		{
			P3dPaintableTexture.OnAddCommandGlobal -= HandleAddCommandGlobal;
		}

		private void HandleAddCommandGlobal(P3dPaintableTexture paintableTexture, P3dCommand command)
		{
			// Ignore commands if we're not listening (this is automatically enabled when rebuilding the paintable texture)
			if (listening == true)
			{
				// Ignore preview paint commands
				if (command.Preview == false)
				{
					// Convert the command from world space to local space
					var localCommand = command.SpawnCopyLocal(paintableTexture.transform);

					// Create a CommandData instance for this command and paintable texture pair
					var commandData = new CommandData();

					// Assign the paintable texture and local command
					commandData.PaintableTexture = paintableTexture;
					commandData.LocalCommand     = localCommand;

					// Store the local command
					commandDatas.Add(commandData);
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dCommandSerialization;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dCommandSerialization_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("listening", "Should this component listen for added commands?");
		}
	}
}
#endif