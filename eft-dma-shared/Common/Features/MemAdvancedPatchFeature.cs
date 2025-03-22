using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eft_dma_shared.Common.Features
{
    public abstract class MemAdvancedPatchFeature<T> : MemPatchFeature<T>
        where T : IMemPatchFeature
    {
        public override bool CanRun => Memory.Ready && Enabled && DelayElapsed && SharedProgram.Config.AdvancedPatches;
    }
}
