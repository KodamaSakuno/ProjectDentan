using System.Collections.Generic;

namespace Moen.KanColle.Dentan.Data
{
    public class CompassData : ModelBase
    {
        static Dictionary<int, string> r_MapNames = new Dictionary<int, string>();

        public int MapAreaID { get; private set; }
        public int MapInfoNo { get; private set; }
        public int MapID { get; private set; }
        public string MapName { get; private set; }
        public MapHP? MapHP { get; internal set; }

        int r_Cell;
        public int Cell
        {
            get { return r_Cell; }
            set
            {
                if (r_Cell != value)
                {
                    r_Cell = value;
                    OnPropertyChanged();
                }
            }
        }
        MapCellEvent r_CellEvent;
        public MapCellEvent CellEvent
        {
            get { return r_CellEvent; }
            set
            {
                if (r_CellEvent != value)
                {
                    r_CellEvent = value;
                    OnPropertyChanged();
                }
            }
        }
        int r_CellEventSubID;
        public int CellEventSubID
        {
            get { return r_CellEventSubID; }
            set
            {
                if (r_CellEventSubID != value)
                {
                    r_CellEventSubID = value;
                    OnPropertyChanged();
                }
            }
        }

        bool r_IsManualSelection;
        public bool IsManualSelection
        {
            get { return r_IsManualSelection; }
            set
            {
                if (r_IsManualSelection != value)
                {
                    r_IsManualSelection = value;
                    OnPropertyChanged();
                }
            }
        }

        bool r_IsBattleCell;
        public bool IsBattleCell
        {
            get { return r_IsBattleCell; }
            set
            {
                if (r_IsBattleCell != value)
                {
                    r_IsBattleCell = value;
                    OnPropertyChanged();
                }
            }
        }

        EnemyFleet r_EnemyData;
        public EnemyFleet EnemyFleet
        {
            get { return r_EnemyData; }
            internal set
            {
                if (r_EnemyData != value)
                {
                    r_EnemyData = value;
                    OnPropertyChanged();
                }
            }
        }
        BattleData r_Battle;
        public BattleData Battle
        {
            get { return r_Battle; }
            internal set
            {
                if (r_Battle != value)
                {
                    r_Battle = value;
                    OnPropertyChanged();
                }
            }
        }

        int r_GetItemID;
        public int GetItemID
        {
            get { return r_GetItemID; }
            set
            {
                if (r_GetItemID != value)
                {
                    r_GetItemID = value;
                    OnPropertyChanged();
                }
            }
        }
        int r_GetItemCount;
        public int GetItemCount
        {
            get { return r_GetItemCount; }
            set
            {
                if (r_GetItemCount != value)
                {
                    r_GetItemCount = value;
                    OnPropertyChanged();
                }
            }
        }

        AviationReconnaissancePlaneType r_AviationReconnaissancePlaneType;
        public AviationReconnaissancePlaneType AviationReconnaissancePlaneType
        {
            get { return r_AviationReconnaissancePlaneType; }
            set
            {
                if (r_AviationReconnaissancePlaneType != value)
                {
                    r_AviationReconnaissancePlaneType = value;
                    OnPropertyChanged();
                }
            }
        }
        AviationReconnaissanceResult r_AviationReconnaissanceResult;
        public AviationReconnaissanceResult AviationReconnaissanceResult
        {
            get { return r_AviationReconnaissanceResult; }
            set
            {
                if (r_AviationReconnaissanceResult != value)
                {
                    r_AviationReconnaissanceResult = value;
                    OnPropertyChanged();
                }
            }
        }

        internal CompassData(int rpMapAreaID, int rpMapInfoNo)
        {
                MapAreaID = rpMapAreaID;
                MapInfoNo = rpMapInfoNo;
                MapID = MapAreaID * 10 + MapInfoNo;

                if (MapID != -11)
                    MapName = GetMapName();
                else
                    MapName = "演習";
        }

        string GetMapName()
        {
            string rResult;
            if (!r_MapNames.TryGetValue(MapID, out rResult))
            {
                if (!KanColleGame.Current.Base.MapAreas[MapAreaID].IsEventMap)
                    rResult = MapAreaID + "-" + MapInfoNo;
                else
                {
                    rResult = "E" + MapInfoNo;
                    switch (KanColleGame.Current.EventRank[MapID])
                    {
                        case 1: rResult += "丙"; break;
                        case 2: rResult += "乙"; break;
                        case 3: rResult += "甲"; break;
                    }
                }

                r_MapNames.Add(MapID, rResult);
            }
            return rResult;
        }
    }
}
