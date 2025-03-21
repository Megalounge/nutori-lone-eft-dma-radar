using eft_dma_radar.Tarkov.Features;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Unity;


namespace LonesEFTRadar.Tarkov.Features.MemoryWrites
{
    class ToggleWeaponCollision : MemWriteFeature<ToggleWeaponCollision>
    {
        private static uint original_WEAPON_OCCLUSION_LAYERS = 1082136832;
        private static uint new_WEAPON_OCCLUSION_LAYERS = 0;
        private static ulong hardSettingsStaticFieldData = 0;
        private bool isApplied = false;

        public override bool Enabled
        {
            get => MemWrites.Config.ToggleWeaponCollision;
            set => MemWrites.Config.ToggleWeaponCollision = value;
        }

        public override bool CanRun
        {
            get
            {
                return base.CanRun && Utils.IsValidVirtualAddress(hardSettingsStaticFieldData);
            }
        }

        public override void TryApply(ScatterWriteHandle writes)
        {
            try
            {
                if (this.Enabled && !this.isApplied)
                {
                    writes.AddValueEntry(hardSettingsStaticFieldData + Offsets.EFTHardSettings.WEAPON_OCCLUSION_LAYERS, ToggleWeaponCollision.new_WEAPON_OCCLUSION_LAYERS);
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
                    writes.AddValueEntry(hardSettingsStaticFieldData + Offsets.EFTHardSettings.WEAPON_OCCLUSION_LAYERS, ToggleWeaponCollision.original_WEAPON_OCCLUSION_LAYERS);
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

            if (ToggleWeaponCollision.hardSettingsStaticFieldData == 0)
                ToggleWeaponCollision.hardSettingsStaticFieldData = Memory.ReadPtr(MonoLib.MonoClass.Find("Assembly-CSharp", "EFTHardSettings", out var hardSettingsClassAddress).GetStaticFieldDataPtr());
        }
    }
}