using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace _Scripts.UI.LoadBoardUI
{
    public class CutOffMask : Image
    {
        public override Material materialForRendering
        {
            get
            {
                Material material = new Material(base.materialForRendering);
                material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
                return material;
            }
        }
    }
}