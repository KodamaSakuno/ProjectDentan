using Moen.KanColle.Dentan.Record;

namespace Moen.KanColle.Dentan.ViewModel.Record
{
    class PracticeRecordViewModel
    {
        public BattleRecord.PracticeItem Data { get; private set; }

        public PracticeRecordViewModel(BattleRecord.PracticeItem rpData)
        {
            Data = rpData;
        }
    }
}
