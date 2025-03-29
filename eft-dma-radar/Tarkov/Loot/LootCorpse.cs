using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.UI.Radar;
using eft_dma_radar.UI.ESP;
using eft_dma_radar.UI.Misc;
using eft_dma_shared.Common.Maps;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Unity;

namespace eft_dma_radar.Tarkov.Loot
{
    public sealed class LootCorpse : LootContainer
    {
        public override string Name => PlayerObject?.Name ?? "Body";
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of corpse (example: Killa).</param>
        public LootCorpse(IReadOnlyList<LootItem> loot) : base(loot)
        {
        }

        /// <summary>
        /// Corpse container's associated player object (if any).
        /// </summary>
        public Player PlayerObject { get; init; }

        public override void DrawESP(SKCanvas canvas, LocalPlayer localPlayer)
        {
            //if (!base.DrawESP(canvas, localPlayer))
            //    return false;
            if (!CameraManagerBase.WorldToScreen(ref Position, out var scrPos))
                return;
            var boxHalf = 2.75f * ESP.Config.FontScale; // was 3.5f
            var showDist = ESP.Config.ShowDistances;
            var boxPt = new SKRect(scrPos.X - boxHalf, scrPos.Y + boxHalf,
                scrPos.X + boxHalf, scrPos.Y - boxHalf);
            canvas.DrawRect(boxPt, SKPaints.PaintContainerLootESP);
            var textPt = new SKPoint(scrPos.X,
                scrPos.Y + 16f * ESP.Config.FontScale);
            textPt.DrawESPText(canvas, this, localPlayer, showDist, SKPaints.TextCorpseESP, this.Name);

            if (this.Loot.Count <= 0)
                return;

            List<string> lines = new List<string>();
            foreach (LootItem lootItem in this.FilteredLoot)
                lines.Add(lootItem.GetUILabel(MainForm.Config.QuestHelper.Enabled));

            var lootItemPt = new SKPoint(scrPos.X, scrPos.Y + SKPaints.TextContainerLootESP.TextSize + 16f * ESP.Config.FontScale);
            lootItemPt.DrawESPText(canvas, this, localPlayer, false, SKPaints.TextCorpseESP, lines.ToArray());
        }
    }
}