using Moen.KanColle.Dentan.Data.Raw;
using System.Collections.Generic;
using System.Linq;

namespace Moen.KanColle.Dentan.Data
{
    public class BaseInfo
    {
        public Table<ShipInfo> Ships { get; private set; }
        public Table<ShipGraphInfo> ShipGraphs { get; private set; }
        public Table<EquipmentInfo> Equipments { get; private set; }
        public Table<EquipmentTypeInfo> EquipmentTypes { get; private set; }
        public Table<ExpeditionInfo> Expeditions { get; private set; }
        public Table<MapAreaInfo> MapAreas { get; private set; }
        public Table<BaseMapInfo> MapInfos { get; private set; }
        public Table<UseItemInfo> UseItems { get; private set; }

        public Dictionary<int, int[]> ShipIDsGroupByModel { get; private set; }

        internal BaseInfo(RawApiStart rpData)
        {
            Ships = new Table<ShipInfo>();
            ShipGraphs = new Table<ShipGraphInfo>();
            Equipments = new Table<EquipmentInfo>();
            EquipmentTypes = new Table<EquipmentTypeInfo>();
            Expeditions = new Table<ExpeditionInfo>();
            MapAreas = new Table<MapAreaInfo>();
            MapInfos = new Table<BaseMapInfo>();
            UseItems = new Table<UseItemInfo>();

            Update(rpData);
        }

        public void Update(RawApiStart rpBaseData)
        {
            Ships.UpdateRawData<RawShipInfo>(rpBaseData.Ships, r => new ShipInfo(r), (rpData, rpRawData) => rpData.Update(rpRawData));
            ShipGraphs.UpdateRawData<RawShipGraphInfo>(rpBaseData.ShipGraphes.Where(r => Ships.ContainsKey(r.ID)), r => new ShipGraphInfo(r, Ships[r.ID]), (rpData, rpRawData) => rpData.Update(rpRawData));

            Equipments.UpdateRawData<RawEquipmentInfo>(rpBaseData.Equipments, r => new EquipmentInfo(r), (rpData, rpRawData) => rpData.Update(rpRawData));
            EquipmentTypes.UpdateRawData<RawEquipmentTypeInfo>(rpBaseData.EquipmentTypes, r => new EquipmentTypeInfo(r), (rpData, rpRawData) => rpData.Update(rpRawData));

            Expeditions.UpdateRawData<RawExpeditionInfo>(rpBaseData.Expeditions, r => new ExpeditionInfo(r), (rpData, rpRawData) => rpData.Update(rpRawData));

            MapAreas.UpdateRawData<RawMapArea>(rpBaseData.MapAreas, r => new MapAreaInfo(r), (rpData, rpRawData) => rpData.Update(rpRawData));
            MapInfos.UpdateRawData<RawBaseMapInfo>(rpBaseData.MapInfos, r => new BaseMapInfo(r), (rpData, rpRawData) => rpData.Update(rpRawData));

            UseItems.UpdateRawData<RawUseItemInfo>(rpBaseData.UseItems, r => new UseItemInfo(r), (rpData, rpRawData) => rpData.Update(rpRawData));
        }

        public void UpdateShipModelIDs()
        {
            ShipIDsGroupByModel = new Dictionary<int, int[]>();
            foreach (var rShip in Ships.Values)
            {
                var rIDs = new List<int>();

                rIDs.Add(rShip.ID);
                for (var rAfterShip = rShip.RemodelAfterShipInfo; rAfterShip != ShipInfo.Default && !rIDs.Contains(rAfterShip.ID); rAfterShip = rAfterShip.RemodelAfterShipInfo)
                    rIDs.Add(rAfterShip.ID);

                ShipIDsGroupByModel.Add(rShip.ID, rIDs.ToArray());
            }
        }

        public ExpeditionInfo GetExpeditionFromName(string rpExpeditionName)
        {
            foreach (var rExpedition in Expeditions.Values)
                if (rExpedition.Name == rpExpeditionName)
                    return rExpedition;
            return null;
        }
    }
}
