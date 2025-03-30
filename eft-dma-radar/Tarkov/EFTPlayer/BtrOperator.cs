using eft_dma_radar.UI.ESP;
using eft_dma_radar.UI.Misc;
using eft_dma_radar.Tarkov.EFTPlayer.Plugins;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Players;
using eft_dma_radar.Tarkov.Features.MemoryWrites;
using eft_dma_shared.Common.ESP;

namespace eft_dma_radar.Tarkov.EFTPlayer
{
    /// <summary>
    /// BTR Bot Operator.
    /// </summary>
    public sealed class BtrOperator : ObservedPlayer
    {
        private readonly ulong _btrView;
        private Vector3 _position;

        public override ref Vector3 Position
        {
            get => ref _position;
        }
        public override string Name
        {
            get => "BTR";
            set { }
        }
        public BtrOperator(ulong btrView, ulong playerBase) : base(playerBase)
        {
            _btrView = btrView;
            Type = PlayerType.AIRaider;
        }
        public override void DrawESP(SKCanvas canvas, LocalPlayer localPlayer)
        {
            if (this == localPlayer || !IsActive || !IsAlive)
                return;

            var dist = Vector3.Distance(localPlayer.Position, Position);
            if (dist > Program.Config.MaxDistance)
                return;

            if (!CameraManagerBase.WorldToScreen(ref Position, out var baseScrPos))
                return;

            var espPaints = GetESPPaints();

            if (IsHostile && (ESP.Config.HighAlertMode is HighAlertMode.AllPlayers ||
                              (ESP.Config.HighAlertMode is HighAlertMode.HumansOnly && IsHuman))) // Check High Alert
            {
                if (this.IsFacingTarget(localPlayer))
                {
                    if (!HighAlertSw.IsRunning)
                        HighAlertSw.Start();
                    else if (HighAlertSw.Elapsed.TotalMilliseconds >= 500f) // Don't draw twice or more
                        HighAlert.DrawHighAlertESP(canvas, this);
                }
                else
                {
                    HighAlertSw.Reset();
                }
            }

            var showDist = ESP.Config.BTRRendering.ShowDist;
            var showLabels = ESP.Config.BTRRendering.ShowLabels;
            var drawLabel = showLabels || showDist;

            var renderMode = ESP.Config.BTRRendering.RenderingMode;

            if (drawLabel)
            {
                var lines = new List<string>();

                if (showLabels)
                    lines.Add($"{Name}");

                if (showDist)
                {
                    if (lines.Count == 0)
                        lines.Add($"{(int)dist}m");
                    else
                        lines[0] += $" ({(int)dist}m)";
                }

                var textPt = new SKPoint(baseScrPos.X, baseScrPos.Y + espPaints.Item2.TextSize * ESP.Config.FontScale);

                textPt.DrawESPText(canvas, this, localPlayer, false, espPaints.Item2, lines.ToArray());
            }
        }

        /// <summary>
        /// Set the position of the BTR.
        /// Give this function it's own unique Index.
        /// </summary>
        /// <param name="index">Scatter read index to read off of.</param>
        public override void OnRealtimeLoop(ScatterReadIndex index)
        {
            index.AddEntry<Vector3>(0, _btrView + Offsets.BTRView._targetPosition);
            index.Callbacks += x1 =>
            {
                if (x1.TryGetResult<Vector3>(0, out var position))
                    _position = position;
            };
        }
    }
}
