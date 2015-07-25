using Moen.KanColle.Dentan.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class ExpeditionInfoViewModel : ViewModel<ExpeditionInfo>
    {
        public ExpeditionData Data { get; private set; }

        public TextBlock[] GetItems { get; private set; }
        public TextBlock[] Requirement { get; private set; }

        public string Time { get; private set; }

        public ExpeditionInfoViewModel(ExpeditionInfo rpModel)
            : base(rpModel)
        {
            Time = TimeSpan.FromMinutes(rpModel.Time).ToString(@"h\:mm");

            ExpeditionData rData;
            if (ExpeditionDataManager.Data.TryGetValue(rpModel.ID, out rData))
            {
                Data = rData;

                Func<string, TextBlock> rCreateTextBlock = r => DispatcherUtil.UIDispatcher.Invoke(() => new TextBlock() { Text = r });

                GetItems = new[] { rpModel.GetItem1, rpModel.GetItem2 }.Where(r => r[0] != 0)
                    .Select(r => rCreateTextBlock($"获得 {KanColleGame.Current.Base.UseItems[r[0]].Name} 0~{r[1]} 个")).ToArray();

                List <TextBlock> rRequirement = new List<TextBlock>();
                rRequirement.Add(rCreateTextBlock($"需要 {Data.ShipCount} 艘船"));

                if (Data.FlagshipType.HasValue)
                    rRequirement.Add(rCreateTextBlock($"旗舰为 {GetTypeName(Data.FlagshipType.Value)}"));

                if (Data.RequiredShipTypes != null)
                    rRequirement.AddRange(Data.RequiredShipTypes.Select(r => rCreateTextBlock($"需要 {r.Count} 艘 {r.Types.Select(rpType=>GetTypeName(rpType)).Join(" 或 ")}")));

                if (Data.Drum != null)
                {
                    if (Data.Drum.Count > 0)
                        rRequirement.Add(rCreateTextBlock($"需要 {Data.Drum.Count} 只桶"));
                    if (Data.Drum.ShipCount > 0)
                        rRequirement.Add(rCreateTextBlock($"需要有 {Data.Drum.ShipCount} 艘船装备了桶"));
                }

                Requirement = rRequirement.ToArray();
            }
        }

        static string GetTypeName(int rpID)
        {
            switch (rpID)
            {
                case 1: return "海防艦";
                case 2: return "駆逐艦";
                case 3: return "軽巡洋艦";
                case 4: return "重雷装巡洋艦";
                case 5: return "重巡洋艦";
                case 6: return "航空巡洋艦";
                case 7: return "軽空母";
                case 8: return "巡洋戦艦";
                case 9: return "戦艦";
                case 10: return "航空戦艦";
                case 11: return "正規空母";
                case 12: return "超弩級戦艦";
                case 13: return "潜水艦";
                case 14: return "潜水空母";
                case 15: return "補給艦";
                case 16: return "水上機母艦";
                case 17: return "揚陸艦";
                case 18: return "装甲空母";
                case 19: return "工作艦";
                case 20: return "潜水母艦";
                case 21: return "練習巡洋艦";
                default: throw new ArgumentException(nameof(rpID));
            }
        }
    }
}
