using Stride.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;

namespace StrideUtils
{
    public class MissingBoneException(string msg) : Exception(msg);
    //Used for manual skeleton bone manipulation.
    public class ReverseBoneBinding
    {
        private readonly int boneIndex;
        private readonly SkeletonUpdater boner;
        public Vector3 BasePosition { get; }
        public Quaternion BaseRotation { get; }
        //Initialize bound to bone at certain index
        public ReverseBoneBinding(int boneIndex, SkeletonUpdater boner)
        {
            this.boneIndex = boneIndex;
            this.boner = boner;
            BasePosition = boner.NodeTransformations[boneIndex].Transform.Position;
            BaseRotation = boner.NodeTransformations[boneIndex].Transform.Rotation;
            boner.NodeTransformations[boneIndex].Flags = ModelNodeFlags.EnableRender | ModelNodeFlags.EnableTransform;
        }
        //Initialize bound to bone with name boneName (trimmed equivalent)
        public ReverseBoneBinding(string boneName, SkeletonUpdater boner)
        {
            this.boner = boner;
            string boneNameTrim = boneName.Trim();
            for(int i = 0; i < boner.Nodes.Length; i++)
            {
                string name = boner.Nodes[i].Name.Trim();
                if (name != boneNameTrim)
                    continue;
                boneIndex = i;
                BasePosition = boner.NodeTransformations[boneIndex].Transform.Position;
                BaseRotation = boner.NodeTransformations[boneIndex].Transform.Rotation;
                boner.NodeTransformations[boneIndex].Flags = ModelNodeFlags.EnableRender | ModelNodeFlags.EnableTransform;
                return;
            }
            throw new MissingBoneException($"Bone with name {boneName} not found in skeleton.");
        }
        public void Transform(Vector3 offset)
        {
            boner.NodeTransformations[boneIndex].Transform.Position = BasePosition + offset;
        }
        public void Rotate(Quaternion rotation)
        {
            boner.NodeTransformations[boneIndex].Transform.Rotation = rotation * BaseRotation;
        }
        public Vector3 GetTransform()
        {
            return boner.NodeTransformations[boneIndex].Transform.Position - BasePosition;
        }
        public Quaternion GetRotation()
        {
            Quaternion inv = BaseRotation;
            inv.Invert();
            return boner.NodeTransformations[boneIndex].Transform.Rotation * inv;
        }
    }
}
