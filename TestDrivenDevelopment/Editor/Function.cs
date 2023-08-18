using UnityEngine;

namespace TestDrivenDevelopment.Editor
{
	public class Function
	{
		public float Value(float x)
		{
			return (Mathf.Pow(x,2) -(4f*x) +4f);
		}
	}
}
