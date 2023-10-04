// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace InverseMask.Scripts
{
    /// <summary>
    /// Ref : https://github.com/Behnamjef/Unity_Utils/blob/main/InverseMask/InverseMask.cs
    /// </summary>

    public class InverseMask : Image
    {
        private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");

        public override Material materialForRendering
        {
            get
            {
                var forRendering = new Material(base.materialForRendering);
                forRendering.SetInt(StencilComp, (int)CompareFunction.NotEqual);
                return forRendering;
            }
        }
    



    }
}
