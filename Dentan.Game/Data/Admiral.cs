using Moen.KanColle.Dentan.Data.Raw;
using Moen.KanColle.Dentan.Record;
using System.Diagnostics;

namespace Moen.KanColle.Dentan.Data
{
    [DebuggerDisplay("Name={Name}, Level={Level}")]
    public class Admiral : RawDataWrapper<RawBasic>
    {
        public int ID { get { return RawData.ID; } }
        public string Name { get { return RawData.Nickname; } }
        public string Comment { get { return RawData.Comment; } }
        public int Level { get { return RawData.Level; } }
        public int MaxShipCount { get { return RawData.MaxShipCount; } }
        public int MaxEquipmentCount { get { return RawData.MaxEquipmentCount; } }

        int r_Experience;
        public int Experience
        {
            get { return r_Experience; }
            set
            {
                if (r_Experience != value)
                {
                    r_Experience = value;
                    OnPropertyChanged();
                }
            }
        }
        int r_NextExperience;
        public int NextExperience
        {
            get { return r_NextExperience; }
            set
            {
                if (r_NextExperience != value)
                {
                    r_NextExperience = value;
                    OnPropertyChanged();
                }
            }
        }

        bool r_Initialized;

        public Admiral(RawBasic rpRawData)
            : base(rpRawData) { }

        protected override void OnRawDataUpdated()
        {
            var rRawExperience = RawData.Experience;
            if (r_Initialized && rRawExperience > 0 && Experience != rRawExperience)
                RecordManager.Instance.Experience.AddAdmiralExpData(rRawExperience);
            Experience = rRawExperience;
            NextExperience = ExperienceTable.Admiral[Level + 1].Total - Experience;

            if (!r_Initialized)
                r_Initialized = true;
        }
    }
}
