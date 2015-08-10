using Moen.KanColle.Dentan.Data;
using Moen.KanColle.Dentan.Data.Raw;
using Moen.KanColle.Dentan.Record;
using System.Text;

namespace Moen.KanColle.Dentan.Api.Parser.Factory
{
    [Api("api_req_kousyou/createitem")]
    class EquipmentDevelopmentParser : ApiParser<RawCreateEquipment>
    {
        public override void Process(RawCreateEquipment rpData)
        {
            var rFuel = int.Parse(Request["api_item1"]);
            var rBullet = int.Parse(Request["api_item2"]);
            var rSteel = int.Parse(Request["api_item3"]);
            var rBauxite = int.Parse(Request["api_item4"]);

            var rBuilder = new StringBuilder(32);
            if (!rpData.Success)
                rBuilder.Append("开发失败");
            else
            {
                Game.Equipments.Add(new Equipment(new RawEquipment() { ID = rpData.Result.ID, EquipmentID = rpData.Result.EquipmentID }));
                Game.UpdateEquipments();

                rBuilder.Append($"「{Game.Base.Equipments[rpData.Result.ID].Name}」开发成功");
            }

            rBuilder.Append($" 投入资材：{rFuel}/{rBullet}/{rSteel}/{rBauxite}");
            Game.SendMessageToStatusBar(rBuilder.ToString());

            Quest.Progresses[605].Current++;
            Quest.Progresses[607].Current++;

            RecordManager.Instance.Development.Update(rpData, rFuel, rBullet, rSteel, rBauxite);
        }
    }
}
