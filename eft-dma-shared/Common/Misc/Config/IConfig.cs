using eft_dma_shared.Common.Unity.LowLevel;

namespace eft_dma_shared.Common.Misc.Config
{
    public interface IConfig
    {
        LowLevelCache LowLevelCache { get; }
        ChamsConfig ChamsConfig { get; }
        bool MemWritesEnabled { get; }
        bool MonoPatches { get; }
        bool NativePatches { get; }
        int MonitorWidth { get; }
        int MonitorHeight { get; }

        void Save();
        Task SaveAsync();
    }
}
