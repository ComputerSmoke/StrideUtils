using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StrideUtils
{
    public class MissingComponentException(string msg) : Exception(msg);
    public static class EntityExtension
    {
        //Search children of entity for specified component
        public static T FindInChild<T>(this Entity entity) where T : EntityComponent
        {
            List<T> found = FindAllInChild<T>(entity);
            if(found.Count > 0)
                return found[0];
            throw new MissingComponentException("no child with component of this type");
        }
        public static List<T> FindAllInChild<T>(this Entity entity) where T : EntityComponent
        {
            List<T> res = [];
            foreach (Entity child in entity.GetChildren())
                res.AddRange(child.GetAll<T>());
            return res;
        }
        public static T GetNotNull<T>(this Entity entity) where T : EntityComponent
        {
            var component = entity.Get<T>();
            if(component != null) 
                return component;
            throw new MissingComponentException($"Entity does not contain component of type {typeof(T).FullName}.");
        }
    }
}
