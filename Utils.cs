// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Physics;
using StrideUtils.WorldStatics;

namespace StrideUtils
{
    public static class Utils
    {
        public static Vector3 LogicDirectionToWorldDirection(Vector2 logicDirection, CameraComponent camera, Vector3 upVector)
        {
            camera.Update();
            var inverseView = Matrix.Invert(camera.ViewMatrix);

            var forward = Vector3.Cross(upVector, inverseView.Right);
            forward.Normalize();

            var right = Vector3.Cross(forward, upVector);
            var worldDirection = forward * logicDirection.Y + right * logicDirection.X;
            worldDirection.Normalize();
            return worldDirection;
        }
        public static Vector3 WorldPos(Entity entity)
        {
            entity.Transform.GetWorldTransformation(out Vector3 pos, out _, out _);
            return pos;
        }
        public static Vector3 WorldScale(Entity entity)
        {
            entity.Transform.GetWorldTransformation(out _, out _, out Vector3 scale);
            return scale;
        }
        public static Quaternion WorldRot(Entity entity)
        {
            entity.Transform.GetWorldTransformation(out _, out Quaternion rot, out _);
            return rot;
        }
        public static void Disown(Entity entity)
        {
            Disown(entity, Vector3.Zero, Vector3.Zero);
        }
        public static void Disown(Entity entity, Vector3 linearVelocity, Vector3 angularImpulse)
        {
            entity.Transform.GetWorldTransformation(
                 out entity.Transform.Position,
                 out entity.Transform.Rotation,
                 out entity.Transform.Scale
             );
            entity.SetParent(WorldStatics.WorldStatics.Origin);

            var rigidBody = entity.Get<RigidbodyComponent>();
            if (rigidBody != null)
            {
                rigidBody.IsKinematic = false;
                rigidBody.CanSleep = true;
                rigidBody.IsTrigger = false;
                rigidBody.LinearVelocity = linearVelocity;
                //rigidBody.ApplyImpulse(linearVelocity);
                rigidBody.AngularVelocity = angularImpulse;
                //rigidBody.ApplyTorqueImpulse(angularImpulse);
            }
        }
        public static Vector3 Proj(Vector3 u, Vector3 v)
        {
            return (Vector3.Dot(v, u) / Vector3.Dot(u, u)) * u;
        }
    }
}
