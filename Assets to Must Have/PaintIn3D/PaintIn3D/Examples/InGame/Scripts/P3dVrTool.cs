using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component attached the current GameObject to a tracked hand.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dVrTool")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "VR Tool")]
	public class P3dVrTool : MonoBehaviour
	{
		/// <summary>The XR node this GameObject will follow.</summary>
		public XRNode Node { set { node = value; } get { return node; } } [SerializeField] private XRNode node = XRNode.RightHand;

		/// <summary>Should painting triggered from this component be eligible for being undone?</summary>
		public bool StoreStates { set { storeStates = value; } get { return storeStates; } } [SerializeField] private bool storeStates = true;

		/// <summary>This tool will be offset by this vector in local space.</summary>
		public Vector3 LocalOffset { set { localOffset = value; } get { return localOffset; } } [SerializeField] private Vector3 localOffset;

		/// <summary>When simulating a VR tool, it will be offset by this local position.</summary>
		public Vector3 SimulatedOffset { set { simulatedOffset = value; } get { return simulatedOffset; } } [SerializeField] private Vector3 simulatedOffset = new Vector3(0.0f, 0.0f, 0.0f);

		/// <summary>The <b>SimulatedOffset</b> value will be offset by this when the simulated key is held.</summary>
		public Vector3 SimulatedKeyOffset { set { simulatedKeyOffset = value; } get { return simulatedKeyOffset; } } [SerializeField] private Vector3 simulatedKeyOffset;

		/// <summary>This allows you to control the speed of the simulated transform changes.</summary>
		public float SimulatedDampening { set { simulatedDampening = value; } get { return simulatedDampening; } } [SerializeField] private float simulatedDampening = 5.0f;

		public UnityEvent OnGrabbed { get { if (onGrabbed == null) onGrabbed = new UnityEvent(); return onGrabbed; } } [SerializeField] private UnityEvent onGrabbed;

		public UnityEvent OnDropped { get { if (onDropped == null) onDropped = new UnityEvent(); return onDropped; } } [SerializeField] private UnityEvent onDropped;

		public UnityEvent OnTriggerPress { get { if (onTriggerPress == null) onTriggerPress = new UnityEvent(); return onTriggerPress; } } [SerializeField] private UnityEvent onTriggerPress;

		public UnityEvent OnTriggerRelease { get { if (onTriggerRelease == null) onTriggerRelease = new UnityEvent(); return onTriggerRelease; } } [SerializeField] private UnityEvent onTriggerRelease;

		public UnityEvent OnGripPress { get { if (onGripPress == null) onGripPress = new UnityEvent(); return onGripPress; } } [SerializeField] private UnityEvent onGripPress;

		public UnityEvent OnGripRelease { get { if (onGripRelease == null) onGripRelease = new UnityEvent(); return onGripRelease; } } [SerializeField] private UnityEvent onGripRelease;

		private static LinkedList<P3dVrTool> instances = new LinkedList<P3dVrTool>(); private LinkedListNode<P3dVrTool> instancesNode;

		private static List<P3dVrTool> tempTools = new List<P3dVrTool>();

		public void Grab(XRNode newNode)
		{
			if (node != newNode)
			{
				Drop();

				node = newNode;

				if (onGrabbed != null)
				{
					onGrabbed.Invoke();
				}
			}
		}

		/// <summary>This will drop the current tool.</summary>
		[ContextMenu("Drop")]
		public void Drop()
		{
			if (node >= 0)
			{
				node = (XRNode)(-1);

				if (onDropped != null)
				{
					onDropped.Invoke();
				}
			}
		}

		/// <summary>This will drop the current tool and grab the next in the scene.</summary>
		[ContextMenu("Drop And Grab Next Tool")]
		public void DropAndGrabNextTool()
		{
			if (node >= 0)
			{
				var previousNode = node;

				Drop();

				var tools = GetTools((XRNode)(-1));
				var index = tools.IndexOf(this);

				tools[(index + 1) % tools.Count].Grab(previousNode);
			}
		}

		/// <summary>This method allows you to find the tool currently on the specified node.</summary>
		public static List<P3dVrTool> GetTools(XRNode node)
		{
			GetTools(node, ref tempTools);

			return tempTools;
		}

		public static void GetTools(XRNode node, ref List<P3dVrTool> tools)
		{
			if (tools == null)
			{
				tools = new List<P3dVrTool>();
			}
			else
			{
				tools.Clear();
			}

			foreach (var instance in instances)
			{
				if (instance.node == node)
				{
					tools.Add(instance);
				}
			}
		}

		/// <summary>This method allows you to drop all tools on the specified node.</summary>
		public static void DropAllTools(XRNode node)
		{
			foreach (var instance in instances)
			{
				if (instance.node == node)
				{
					instance.Drop();
				}
			}
		}

		public void UpdateGripped(P3dVrManager vrManager)
		{
			// Position?
			var position    = default(Vector3);
			var positionSet = false;

			if (vrManager.TryGetPosition(node, ref position) == true)
			{
				positionSet = true;

				if (vrManager.IsSimulation == true)
				{
					position += transform.rotation * localOffset;

					if (vrManager.GetTrigger(node) == true)
					{
						position += transform.rotation * simulatedKeyOffset;
					}
				}

				position += transform.rotation * simulatedOffset;
			}

			// Rotation?
			var rotation    = default(Quaternion);
			var rotationSet = false;

			if (vrManager.TryGetRotation(node, ref rotation) == true)
			{
				rotationSet = true;
			}

			// Transition?
			var dampening = 1.0f;

			if (vrManager.IsSimulation == true)
			{
				dampening = CwHelper.DampenFactor(simulatedDampening, Time.deltaTime);
			}

			if (positionSet == true)
			{
				transform.position = Vector3.Lerp(transform.position, position, dampening);
			}

			if (rotationSet == true)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, dampening);
			}

			// Events?
			if (vrManager.GetTriggerPressed(node) == true)
			{
				if (storeStates == true)
				{
					P3dStateManager.StoreAllStates();
				}

				if (onTriggerPress != null)
				{
					onTriggerPress.Invoke();
				}
			}

			if (vrManager.GetTriggerReleased(node) == true)
			{
				if (onTriggerRelease != null)
				{
					onTriggerRelease.Invoke();
				}
			}

			if (vrManager.GetGripPressed(node) == true)
			{
				if (onGripPress != null)
				{
					onGripPress.Invoke();
				}
			}

			if (vrManager.GetGripReleased(node) == true)
			{
				if (onGripRelease != null)
				{
					onGripRelease.Invoke();
				}
			}
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
			if (node >= 0)
			{
				if (onGrabbed != null)
				{
					onGrabbed.Invoke();
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dVrTool;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dVrTool_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("node", "The XR node this GameObject will follow.");
			Draw("storeStates", "Should painting triggered from this component be eligible for being undone?");
			Draw("localOffset", "This tool will be offset by this vector in local space.");

			Separator();

			Draw("simulatedDampening", "This allows you to control the speed of the simulated transform changes.");
			Draw("simulatedOffset", "When simulating a VR tool, it will be offset by this local position.");
			Draw("simulatedKeyOffset", "The SimulatedOffset value will be offset by this when the simulated key is held.");

			Separator();

			Draw("onGrabbed", "");
			Draw("onDropped", "");
			Draw("onTriggerPress", "");
			Draw("onTriggerRelease", "");
			Draw("onGripPress", "");
			Draw("onGripRelease", "");
		}
	}
}
#endif