using eft_dma_radar.Tarkov.Features;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonesEFTRadar.Tarkov.Features.MemoryWrites
{
    class InstantPoseChange : MemWriteFeature<InstantPoseChange>
    {
        private static float original_POSE_CHANGING_SPEED = 3f;
        private static float new_POSE_CHANGING_SPEED = float.MaxValue;
        private static ulong hardSettingsStaticFieldData = 0;
        private bool isApplied = false;

        public override bool Enabled
        {
            get => MemWrites.Config.InstantPoseChange;
            set => MemWrites.Config.InstantPoseChange = value;
        }

        public override bool CanRun => Memory.InRaid && Memory.RaidHasStarted && DelayElapsed && Utils.IsValidVirtualAddress(hardSettingsStaticFieldData);

        protected override TimeSpan Delay => TimeSpan.FromMilliseconds(100);

        public override void TryApply(ScatterWriteHandle writes)
        {
            try
            {
                if (this.Enabled && !this.isApplied)
                {
                    writes.AddValueEntry(hardSettingsStaticFieldData + Offsets.EFTHardSettings.POSE_CHANGING_SPEED, InstantPoseChange.new_POSE_CHANGING_SPEED);
                    writes.Callbacks += () =>
                    {
                        if (!this.isApplied)
                        {
                            this.isApplied = true;
                            LoneLogging.WriteLine($"{this.GetType().Name} [ON]");
                        }
                    };
                }
                else if (!this.Enabled && this.isApplied)
                {
                    writes.AddValueEntry(hardSettingsStaticFieldData + Offsets.EFTHardSettings.POSE_CHANGING_SPEED, InstantPoseChange.original_POSE_CHANGING_SPEED);
                    writes.Callbacks += () =>
                    {
                        if (this.isApplied)
                        {
                            this.isApplied = false;
                            LoneLogging.WriteLine($"{this.GetType().Name} [OFF]");
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR configuring {this.GetType().Name}: {ex}");
            }
        }

        public override void OnRaidStart()
        {
            base.OnRaidStart();

            if (InstantPoseChange.hardSettingsStaticFieldData == 0)
                InstantPoseChange.hardSettingsStaticFieldData = Memory.ReadPtr(MonoLib.MonoClass.Find("Assembly-CSharp", "EFTHardSettings", out var hardSettingsClassAddress).GetStaticFieldDataPtr());
        }
    }
}