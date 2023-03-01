using UnityEngine;
using System.Collections.Generic;

namespace PaintIn3D
{
	/// <summary>This is the base class for all components that repeat paint commands (e.g. mirroring).</summary>
	public abstract class P3dClone : MonoBehaviour, IClone
	{
		[System.NonSerialized]
		public static int MatrixCount;

		[System.NonSerialized]
		public static int ClonerCount;

		public abstract void Transform(ref Matrix4x4 posMatrix, ref Matrix4x4 rotMatrix);

		[System.NonSerialized]
		private static List<Matrix4x4> tempPosMatrices = new List<Matrix4x4>();

		[System.NonSerialized]
		private static List<Matrix4x4> tempRotMatrices = new List<Matrix4x4>();

		[System.NonSerialized]
		private static List<IClone> tempCloners = new List<IClone>();

		/// <summary>This stores all active and enabled instances in the open scenes.</summary>
		public static LinkedList<P3dClone> Instances { get { return instances; } } private static LinkedList<P3dClone> instances = new LinkedList<P3dClone>(); private LinkedListNode<P3dClone> instancesNode;

		public static void BuildCloners(List<IClone> cloners = null)
		{
			tempCloners.Clear();
			tempPosMatrices.Clear();
			tempRotMatrices.Clear();

			tempPosMatrices.Add(Matrix4x4.identity);
			tempRotMatrices.Add(Matrix4x4.identity);

			if (cloners != null)
			{
				for (var i = 0; i < cloners.Count; i++)
				{
					var cloner = cloners[i];

					if (cloner != null)
					{
						tempCloners.Add(cloner);
					}
				}
			}
			else
			{
				foreach (var instance in instances)
				{
					tempCloners.Add(instance);
				}
			}

			MatrixCount = 1;
			ClonerCount = tempCloners.Count;
		}

		public static void Clone(P3dCommand command, int clonerIndex, int matrixIndex)
		{
			if (matrixIndex == 0)
			{
				MatrixCount = tempPosMatrices.Count;
			}

			var posMatrix = tempPosMatrices[matrixIndex];
			var rotMatrix = tempRotMatrices[matrixIndex];

			tempCloners[clonerIndex].Transform(ref posMatrix, ref rotMatrix);

			tempPosMatrices.Add(posMatrix);
			tempRotMatrices.Add(rotMatrix);

			command.Transform(posMatrix, rotMatrix);
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode); instancesNode = null;
		}
	}
}