using eft_dma_radar.UI.Misc;
using eft_dma_shared.Common.Misc.Data;

namespace eft_dma_radar.Tarkov.Loot
{
    /// <summary>
    /// Defines a static/dynamic spawn Quest Item (not normal loot).
    /// </summary>
    public sealed class QuestItem : LootItem
    {
        public QuestItem(TarkovMarketItem entry) : base(entry)
        {
        }

        public QuestItem(string id, string name) : base(id, name)
        {
        }

        public override ValueTuple<SKPaint, SKPaint> GetPaints() => new(SKPaints.QuestHelperPaint, SKPaints.QuestHelperText);
        public override ValueTuple<SKPaint, SKPaint> GetESPPaints() => new(SKPaints.PaintQuestHelperESP, SKPaints.TextQuestHelperESP);
    }
}
