using System;

namespace Moen.KanColle.Dentan.Data.QuestData
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class QuestAttribute : Attribute
    {
        public int ID { get; private set; }
        public QuestType Type { get; private set; }
        public int Total { get; private set; }

        public int StartFrom { get; set; }

        public QuestAttribute(int rpID)
            : this(rpID, QuestType.Daily, 1) { }
        public QuestAttribute(int rpID, QuestType rpType, int rpTotal)
        {
            ID = rpID;
            Type = rpType;
            Total = rpTotal;
        }
    }
}
