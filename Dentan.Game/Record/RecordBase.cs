using System.Data.SQLite;

namespace Moen.KanColle.Dentan.Record
{
    public abstract class RecordBase
    {
        public SQLiteConnection Connection { get; private set; }

        protected RecordBase(SQLiteConnection rpConnection)
        {
            Connection = rpConnection;
        }

        internal abstract void Load();
    }
}
