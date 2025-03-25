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
        private static ulong EFTHardSettingsInstance = 0;
        private bool isApplied = false;

        public override bool Enabled
        {
            get => MemWrites.Config.ToggleWeaponCollision;
            set => MemWrites.Config.ToggleWeaponCollision = value;
        }

        public override bool CanRun => base.CanRun && Utils.IsValidVirtualAddress(ToggleWeaponCollision.EFTHardSettingsInstance);

        public override void TryApply(ScatterWriteHandle writes)
        {
            try
            {
                if (this.Enabled && !this.isApplied)
                {
                    writes.AddValueEntry(ToggleWeaponCollision.EFTHardSettingsInstance + Offsets.EFTHardSettings.WEAPON_OCCLUSION_LAYERS, ToggleWeaponCollision.new_WEAPON_OCCLUSION_LAYERS);
                    writes.Callbacks += () =>
                    {
                        if (!this.isApplied)
                        {
                            this.isApplied = true;
                            LoneLogging.WriteLine($"{nameof(ToggleWeaponCollision)} [ON]");
                        }
                    };
                }
                else if (!this.Enabled && this.isApplied)
                {
                    writes.AddValueEntry(ToggleWeaponCollision.EFTHardSettingsInstance + Offsets.EFTHardSettings.WEAPON_OCCLUSION_LAYERS, ToggleWeaponCollision.original_WEAPON_OCCLUSION_LAYERS);
                    writes.Callbacks += () =>
                    {
                        if (this.isApplied)
                        {
                            this.isApplied = false;
                            LoneLogging.WriteLine($"{nameof(ToggleWeaponCollision)} [OFF]");
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR configuring {nameof(ToggleWeaponCollision)}: {ex}");
            }
        }

        public override void OnRaidStart()
        {
            base.OnRaidStart();

            if (ToggleWeaponCollision.EFTHardSettingsInstance == 0)
                ToggleWeaponCollision.EFTHardSettingsInstance = Memory.ReadPtr(MonoLib.MonoClass.Find("Assembly-CSharp", "EFTHardSettings", out var hardSettingsClassAddress).GetStaticFieldDataPtr());
        }

        public override void OnGameStop()
        {
            base.OnGameStop();
            ToggleWeaponCollision.EFTHardSettingsInstance = default;
        }
    }
}