using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Engine;

namespace StrideUtils.WorldStatics
{
    public class OriginEntity : SyncScript
    {
        public override void Start()
        {
            base.Start();
            WorldStatics.Origin = Entity;
        }
        public override void Update() { }
    }
}
