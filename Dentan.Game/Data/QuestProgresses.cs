using Moen.KanColle.Dentan.Data.QuestData;
using Moen.KanColle.Dentan.Data.Raw;
using System;
using System.Linq;
using System.Reflection;

namespace Moen.KanColle.Dentan.Data
{
    partial class Quest
    {
        public static Table<QuestProgressData> Progresses { get; private set; }

        static Quest()
        {
            var rCurrentAssembly = Assembly.GetExecutingAssembly();
            var rParserTypes = rCurrentAssembly.GetTypes().Where(r => !r.IsAbstract && r.IsSubclassOf(typeof(QuestProgressData)));

            Progresses = new Table<QuestProgressData>(rParserTypes.Select(rType =>
            {
                var rAttribute = rType.GetCustomAttributes<QuestAttribute>().SingleOrDefault();

                var rProgress = (QuestProgressData)Activator.CreateInstance(rType);
                rProgress.ID = rAttribute.ID;
                rProgress.Type = rAttribute.Type;
                rProgress.Total = rAttribute.Total;
                rProgress.StartFrom = rAttribute.StartFrom;

                return rProgress;
            }));

            r_Default = new Quest(new RawQuest() { ID = -1, Title = "?????", State = QuestState.Progress });
        }
    }
}
