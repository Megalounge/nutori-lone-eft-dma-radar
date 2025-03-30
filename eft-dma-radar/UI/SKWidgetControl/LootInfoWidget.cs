using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.Tarkov.Features;
using eft_dma_radar.Tarkov.Loot;
using eft_dma_radar.UI.Misc;
using eft_dma_radar.UI.Radar;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Data;
using System.Windows.Forms;

namespace eft_dma_radar.UI.SKWidgetControl
{
    public sealed class LootInfoWidget : SKWidget
    {

        /// <summary>
        /// Constructs a Player Info Overlay.
        /// </summary>
        public LootInfoWidget(SKGLControl parent, SKRect location, bool minimized, float scale)
            : base(parent, "Loot Info", new SKPoint(location.Left, location.Top),
                new SKSize(location.Width, location.Height), scale, false)
        {
            Minimized = minimized;
            SetScaleFactor(scale);
        }

        /// <summary>
        /// All Filtered Loot on the map (Grouped by Position).
        /// </summary>
        /// <summary>
        private static IEnumerable<(LootItem, int)> Loot =>
            Memory.Loot?.FilteredLoot
                ?.Where(item => item is not LootCorpse && item is not QuestItem) // Remove corpses and quest items
                .GroupBy(item => item.Position) // Group items by their position
                .Select(group => (group.First(), group.Count())) // Take the first item in each group and count duplicates
                .OrderByDescending(entry => entry.Item1.Price) // ✅ Use Item1 to access LootItem
                .Take(15)
                ?? Enumerable.Empty<(LootItem, int)>(); // Ensure it's never null

        public void Draw(SKCanvas canvas, Player localPlayer)
        {
            if (Minimized)
            {
                base.Draw(canvas);
                return;
            }

            var lootItems = Loot.ToList();
            var lootCount = lootItems.Count;
            var sb = new StringBuilder();

            // Table headers
            sb.AppendFormat("{0,-18}", "Name")
                .AppendFormat("{0,-8}", "Price")
                .AppendFormat("{0,-8}", "Dist")
                .AppendLine();

            var drawPt = new SKPoint(ClientRectangle.Left + 5, ClientRectangle.Top + 20);

            float textHeight = SKPaints.LootInfoText.TextSize * 1.2f;

            foreach (var (item, count) in lootItems)
            {
                string name = item.ShortName;
                if (count > 1)
                    name += $" ({count})";
                string price = TarkovMarketItem.FormatPrice(item.Price);
                float dist = Vector3.Distance(item.Position, localPlayer.Position);

                sb.AppendFormat("{0,-18}", name)
                    .AppendFormat("{0,-8}", price)
                    .AppendFormat("{0,-8:F0}", dist)
                    .AppendLine();

                drawPt.Y += textHeight;
            }

            var data = sb.ToString().Split(Environment.NewLine);
            var lineSpacing = SKPaints.LootInfoText.FontSpacing;
            var maxLength = data.Max(x => SKPaints.LootInfoText.MeasureText(x));
            var pad = 2.5f * ScaleFactor;

            Size = new SKSize(maxLength + pad, data.Length * lineSpacing + pad);
            Location = Location;
            Draw(canvas);

            drawPt = new SKPoint(ClientRectangle.Left + pad, ClientRectangle.Top + lineSpacing / 2 + pad);
            canvas.DrawText($"Total Loot: {lootCount}", drawPt, SKPaints.LootInfoText);
            drawPt.Y += lineSpacing;

            foreach (var line in data)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    canvas.DrawText(line, drawPt, SKPaints.LootInfoText);
                    drawPt.Y += textHeight;
                }
            }
        }
        public override void SetScaleFactor(float newScale)
        {
            base.SetScaleFactor(newScale);
            SKPaints.LootInfoText.TextSize = 12 * newScale;
        }
    }
}