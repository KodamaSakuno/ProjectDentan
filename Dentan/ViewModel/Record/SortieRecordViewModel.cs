using Moen.KanColle.Dentan.Record;

namespace Moen.KanColle.Dentan.ViewModel.Record
{
    class SortieRecordViewModel : ModelBase
    {
        public SortieRecord.Item Data { get; private set; }

        public string Map { get; private set; }

        public SortieRecordViewModel(SortieRecord.Item rpData)
        {
            Data = rpData;

            Map = $"{rpData.Map / 10}-{rpData.Map % 10}";
        }
    }
}
