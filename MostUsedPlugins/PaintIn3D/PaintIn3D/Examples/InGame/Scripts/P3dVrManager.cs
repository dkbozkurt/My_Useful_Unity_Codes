using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component attached the current GameObject to a tracked hand.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dVrManager")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "VR Manager")]
	public class P3dVrManager : MonoBehaviour
	{
		class SimulatedState
		{
			public XRNode     Node;
			public bool       Set;
			public Vector3    Position;
			public Quaternion Rotation = Quaternion.identity;

			public SimulatedState(XRNode newNode)
			{
				Node = newNode;
			}
		}

		/// <summary>This key allows you to reset the VR orientation.</summary>
		//public KeyCode RecenterKey { set { recenterKey = value; } get { return recenterKey; } } [SerializeField] private KeyCode recenterKey = KeyCode.Space;

		/// <summary>The default distance in world space a hand must be to grab a tool.</summary>
		public float GrabDistance { set { grabDistance = value; } get { return grabDistance; } } [SerializeField] private float grabDistance = 0.3f;

		/// <summary>This key allows you to simulate a left hand VR trigger.</summary>
		public KeyCode SimulatedLeftTrigger { set { simulatedLeftTrigger = value; } get { return simulatedLeftTrigger; } } [SerializeField] private KeyCode simulatedLeftTrigger = KeyCode.Mouse0;

		/// <summary>This key allows you to simulate a left hand VR grip.</summary>
		public KeyCode SimulatedLeftGrip { set { simulatedLeftGrip = value; } get { return simulatedLeftGrip; } } [SerializeField] private KeyCode simulatedLeftGrip = KeyCode.LeftControl;

		/// <summary>This key allows you to simulate a right hand VR trigger.</summary>
		public KeyCode SimulatedRightTrigger { set { simulatedRightTrigger = value; } get { return simulatedRightTrigger; } } [SerializeField] private KeyCode simulatedRightTrigger = KeyCode.Mouse1;

		/// <summary>This key allows you to simulate a right hand VR grip.</summary>
		public KeyCode SimulatedRightGrip { set { simulatedRightGrip = value; } get { return simulatedRightGrip; } } [SerializeField] private KeyCode simulatedRightGrip = KeyCode.RightControl;

		/// <summary>When simulating a VR tool, it will be offset by this Euler rotation.</summary>
		public Vector3 SimulatedTilt { set { simulatedTilt = value; } get { return simulatedTilt; } } [SerializeField] private Vector3 simulatedTilt = new Vector3(0.0f, -15.0f, 0.0f);

		/// <summary>When simulating a VR tool, it will be offset by this local position.</summary>
		public Vector3 SimulatedOffset { set { simulatedOffset = value; } get { return simulatedOffset; } } [SerializeField] private Vector3 simulatedOffset = new Vector3(0.0f, 0.0f, -0.2f);

		/// <summary>When simulating a VR tool, it will be moved away from the hit surface by this.</summary>
		public float SimulatedDistanceMax { set { simulatedReach = value; } get { return simulatedReach; } } [SerializeField] private float simulatedReach = 1.0f;

		/// <summary>The simulated left VR eye will be offset this much.</summary>
		public Vector3 SimulatedEyeOffset { set { simulatedEyeOffset = value; } get { return simulatedEyeOffset; } } [SerializeField] private Vector3 simulatedEyeOffset = new Vector3(-0.0325f, 0.0f, 0.0f);

		/// <summary>When simulating a VR tool, this will control how much the hit surface normal influences the tool rotation.</summary>
		public float SimulatedNormalInfluence { set { simulatedNormalInfluence = value; } get { return simulatedNormalInfluence; } } [Range(0.0f, 1.0f)] [SerializeField] private float simulatedNormalInfluence = 0.25f;
		
		private SimulatedState[] simulatedStates = new SimulatedState[]
			{
				new SimulatedState(XRNode.LeftEye),
				new SimulatedState(XRNode.RightEye),
				new SimulatedState(XRNode.CenterEye),
				new SimulatedState(XRNode.Head),
				new SimulatedState(XRNode.LeftHand),
				new SimulatedState(XRNode.RightHand)
			};

		private float hitDistance;

		private Quaternion hitRotation = Quaternion.identity;
		
		public bool LeftTrigger;
		public bool RightTrigger;
		public bool LeftGrip;
		public bool RightGrip;

		public bool PrevLeftTrigger;
		public bool PrevRightTrigger;
		public bool PrevLeftGrip;
		public bool PrevRightGrip;

		private static List<XRNodeState> states = new List<XRNodeState>();

		private static List<P3dVrTool> tempTools = new List<P3dVrTool>();

		/// <summary>This stores all active and enabled instances in the open scenes.</summary>
		public static LinkedList<P3dVrManager> Instances { get { return instances; } } private static LinkedList<P3dVrManager> instances = new LinkedList<P3dVrManager>(); private LinkedListNode<P3dVrManager> instancesNode;

		public bool IsSimulation
		{
			get
			{
				return XRSettings.enabled == false;
			}
		}

#if ENABLE_INPUT_SYSTEM
		public void SetRightTrigger(UnityEngine.InputSystem.InputAction.CallbackContext context)
		{
			RightTrigger = context.ReadValueAsButton();
		}

		public void SetLeftTrigger(UnityEngine.InputSystem.InputAction.CallbackContext context)
		{
			LeftTrigger = context.ReadValueAsButton();
		}

		public void SetLeftGrip(UnityEngine.InputSystem.InputAction.CallbackContext context)
		{
			LeftGrip = context.ReadValueAsButton();
		}

		public void SetRightGrip(UnityEngine.InputSystem.InputAction.CallbackContext context)
		{
			RightGrip = context.ReadValueAsButton();
		}
#endif

		public void SetRightTrigger(bool value)
		{
			RightTrigger = value;
		}

		public void SetLeftTrigger(bool value)
		{
			LeftTrigger = value;
		}

		public void SetLeftGrip(bool value)
		{
			LeftGrip = value;
		}

		public void SetRightGrip(bool value)
		{
			RightGrip = value;
		}

		public bool GetTrigger(XRNode node)
		{
			switch (node)
			{
				case XRNode.LeftHand: return LeftTrigger == true;
				case XRNode.RightHand: return RightTrigger == true;
			}

			return false;
		}

		public bool GetTriggerPressed(XRNode node)
		{
			switch (node)
			{
				case XRNode.LeftHand: return LeftTrigger == true && PrevLeftTrigger == false;
				case XRNode.RightHand: return RightTrigger == true && PrevRightTrigger == false;
			}

			return false;
		}

		public bool GetTriggerReleased(XRNode node)
		{
			switch (node)
			{
				case XRNode.LeftHand: return LeftTrigger == false && PrevLeftTrigger == true;
				case XRNode.RightHand: return RightTrigger == false && PrevRightTrigger == true;
			}

			return false;
		}
		
		public bool GetGrip(XRNode node)
		{
			switch (node)
			{
				case XRNode.LeftHand: return LeftGrip == true;
				case XRNode.RightHand: return RightGrip == true;
			}

			return false;
		}

		public bool GetGripPressed(XRNode node)
		{
			switch (node)
			{
				case XRNode.LeftHand: return LeftGrip == true && PrevLeftGrip == false;
				case XRNode.RightHand: return RightGrip == true && PrevRightGrip == false;
			}

			return false;
		}

		public bool GetGripReleased(XRNode node)
		{
			switch (node)
			{
				case XRNode.LeftHand: return LeftGrip == false && PrevLeftGrip == true;
				case XRNode.RightHand: return RightGrip == false && PrevRightGrip == true;
			}

			return false;
		}

		public XRNode GetClosestNode(Vector3 point, float maximumDistance)
		{
			var bestNode     = (XRNode)(-1);
			var bestDistance = maximumDistance;
			var position     = default(Vector3);

			if (TryGetPosition(XRNode.LeftHand, ref position) == true)
			{
				var distance = Vector3.Distance(point, position);

				if (distance < bestDistance)
				{
					bestDistance = distance;
					bestNode     = XRNode.LeftHand;
				}
			}

			if (TryGetPosition(XRNode.RightHand, ref position) == true)
			{
				var distance = Vector3.Distance(point, position);

				if (distance < bestDistance)
				{
					bestDistance = distance;
					bestNode     = XRNode.RightHand;
				}
			}

			return bestNode;
		}

		private void SetSimulatedState(XRNode node, Vector3 position, Quaternion rotation)
		{
			foreach (var simulatedState in simulatedStates)
			{
				if (simulatedState.Node == node)
				{
					simulatedState.Set      = true;
					simulatedState.Position = position;
					simulatedState.Rotation = rotation;

					return;
				}
			}
		}

		public bool TryGetPosition(XRNode node, ref Vector3 position)
		{
			if (IsSimulation == true)
			{
				foreach (var simulatedState in simulatedStates)
				{
					if (simulatedState.Node == node)
					{
						if (simulatedState.Set == false)
						{
							return false;
						}

						position = simulatedState.Position;

						return true;
					}
				}
			}
			else
			{
				InputTracking.GetNodeStates(states);

				foreach (var state in states)
				{
					if (state.nodeType == node)
					{
						return state.TryGetPosition(out position);
					}
				}
			}

			return false;
		}

		public bool TryGetRotation(XRNode node, ref Quaternion rotation)
		{
			if (IsSimulation == true)
			{
				foreach (var simulatedState in simulatedStates)
				{
					if (simulatedState.Node == node)
					{
						if (simulatedState.Set == false)
						{
							return false;
						}

						rotation = simulatedState.Rotation;

						return true;
					}
				}
			}
			else
			{
				InputTracking.GetNodeStates(states);

				foreach (var state in states)
				{
					if (state.nodeType == node)
					{
						return state.TryGetRotation(out rotation);
					}
				}
			}

			return false;
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode); instancesNode = null;
		}

		protected virtual void Start()
		{
			//Recenter();

			hitDistance = simulatedReach * 0.25f;
		}

		protected virtual void Update()
		{
			if (IsSimulation == true)
			{
				LeftTrigger  = CwInput.GetKeyIsHeld(simulatedLeftTrigger);
				LeftGrip     = CwInput.GetKeyIsHeld(simulatedLeftGrip);
				RightTrigger = CwInput.GetKeyIsHeld(simulatedRightTrigger);
				RightGrip    = CwInput.GetKeyIsHeld(simulatedRightGrip);

				if (CwInputManager.PointOverGui(CwInput.GetMousePosition()) == true)
				{
					LeftTrigger  = false;
					LeftGrip     = false;
					RightTrigger = false;
					RightGrip    = false;
				}
			}

			//if (CwInputManager.IsDown(recenterKey) == true)
			//{
			//	Recenter();
			//}
		}

		protected virtual void LateUpdate()
		{
			var camera = CwHelper.GetCamera(null);

			if (camera != null)
			{
				var ray = camera.ScreenPointToRay(CwInput.GetMousePosition());
				var hit = default(RaycastHit);
				var cam = camera.transform.rotation;

				if (Physics.Raycast(ray, out hit, simulatedReach) == true)
				{
					hitDistance = hit.distance;
					hitRotation = Quaternion.Inverse(cam) * Quaternion.LookRotation(-hit.normal);
				}

				var leftHandRot = Quaternion.Slerp(cam, cam * hitRotation, simulatedNormalInfluence) * Quaternion.Euler(simulatedTilt.x, -simulatedTilt.y, simulatedTilt.z);
				var leftHandPos = ray.GetPoint(hitDistance) + leftHandRot * new Vector3(simulatedOffset.x, simulatedOffset.y, simulatedOffset.z);

				SetSimulatedState(XRNode.LeftHand, leftHandPos, leftHandRot);

				var rightHandRot = Quaternion.Slerp(cam, cam * hitRotation, simulatedNormalInfluence) * Quaternion.Euler(simulatedTilt.x, simulatedTilt.y, simulatedTilt.z);
				var rightHandPos = ray.GetPoint(hitDistance) + rightHandRot * new Vector3(simulatedOffset.x, -simulatedOffset.y, simulatedOffset.z);

				SetSimulatedState(XRNode.RightHand, rightHandPos, rightHandRot);

				SetSimulatedState(XRNode.Head, camera.transform.position, camera.transform.rotation);

				SetSimulatedState(XRNode.CenterEye, camera.transform.position, camera.transform.rotation);

				SetSimulatedState(XRNode.LeftEye, camera.transform.TransformPoint(simulatedEyeOffset.x, simulatedEyeOffset.y, simulatedEyeOffset.z), camera.transform.rotation);

				SetSimulatedState(XRNode.RightEye, camera.transform.TransformPoint(-simulatedEyeOffset.x, simulatedEyeOffset.y, simulatedEyeOffset.z), camera.transform.rotation);
			}

			for (var i = 0; i <= 8; i++)
			{
				UpdateTools((XRNode)i);
			}

			PrevLeftTrigger  = LeftTrigger;
			PrevLeftGrip     = LeftGrip;
			PrevRightTrigger = RightTrigger;
			PrevRightGrip    = RightGrip;
		}

		private void UpdateTools(XRNode node)
		{
			P3dVrTool.GetTools(node, ref tempTools);

			foreach (var tool in tempTools)
			{
				if (tool != null && tool.Node == node)
				{
					tool.UpdateGripped(this);
				}
			}
		}

		//[ContextMenu("Recenter")]
		//public void Recenter()
		//{
		//	if (XRSettings.enabled == true)
		//	{
		//		InputTracking.Recenter();
		//	}
		//}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dVrManager;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dVrManager_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			if (Application.isPlaying == true && XRSettings.enabled == false)
			{
				Warning("VR is disabled in your project settings, so simulated fingers will be used. If you have a VR device then you can enable it.");
			}

			//Draw("recenterKey", "This key allows you to reset the VR orientation.");
			Draw("grabDistance", "The default distance in world space a hand must be to grab a tool.");

			Separator();

			Draw("simulatedLeftTrigger", "This key allows you to simulate a left hand VR trigger.");
			Draw("simulatedLeftGrip", "This key allows you to simulate a left hand VR grip.");
			Draw("simulatedRightTrigger", "This key allows you to simulate a right hand VR trigger.");
			Draw("simulatedRightGrip", "This key allows you to simulate a right hand VR grip.");

			Separator();

			Draw("simulatedTilt", "When simulating a VR tool, it will be offset by this Euler rotation.");
			Draw("simulatedOffset", "When simulating a VR tool, it will be offset by this local position.");
			Draw("simulatedReach", "When simulating a VR tool, it will be moved away from the hit surface by this.");
			Draw("simulatedEyeOffset", "The simulated left VR eye will be offset this much.");
			Draw("simulatedNormalInfluence", "When simulating a VR tool, this will control how much the hit surface normal influences the tool rotation.");

			Separator();

			BeginDisabled();
				EditorGUILayout.Toggle("Left Trigger", tgt.LeftTrigger);
				EditorGUILayout.Toggle("Left Grip", tgt.LeftGrip);
				EditorGUILayout.Toggle("Right Trigger", tgt.RightTrigger);
				EditorGUILayout.Toggle("Right Grip", tgt.RightGrip);
			EndDisabled();
		}
	}
}
#endif