using System.Data.SQLite;
using System.IO;

namespace Moen.KanColle.Dentan.Record
{
    public class RecordManager
    {
        static RecordManager r_Instance;
        public static RecordManager Instance { get { return r_Instance ?? (r_Instance = new RecordManager()); } }

        public ResourceRecord Resource { get; private set; }
        public ExperienceRecord Experience { get; private set; }
        public ExpeditionRecord Expedition { get; private set; }
        public ConstructionRecord Construction { get; private set; }
        public DevelopmentRecord Development { get; private set; }
        public QuestRecord Quest { get; private set; }
        public SortieRecord Drop { get; private set; }

        public BattleRecord Battle { get; private set; }

        int r_ID;
        SQLiteConnection r_Connection, r_BattleRecordConnection;

        public bool IsLoaded { get; private set; }

        RecordManager()
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
        }

        public void Load(int rpID)
        {
            if (r_ID == rpID) return;

            if (r_Connection != null)
                r_Connection.Close();

            r_ID = rpID;
            r_Connection = new SQLiteConnection($"Data Source=Data\\{r_ID}.db").OpenAndReturn();
            r_BattleRecordConnection = new SQLiteConnection($"Data Source=Data\\{r_ID}_Battle.db").OpenAndReturn();

            Resource = new ResourceRecord(r_Connection);
            Experience = new ExperienceRecord(r_Connection);
            Expedition = new ExpeditionRecord(r_Connection);
            Construction = new ConstructionRecord(r_Connection);
            Development = new DevelopmentRecord(r_Connection);
            Quest = new QuestRecord(r_Connection);
            Drop = new SortieRecord(r_Connection);

            Battle = new BattleRecord(r_BattleRecordConnection);

            Resource.Load();
            Experience.Load();
            Expedition.Load();
            Construction.Load();
            Development.Load();
            Quest.Load();
            Drop.Load();

            Battle.Load();

            IsLoaded = true;
        }
    }
}
