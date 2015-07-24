using Moen.Collections;
using Moen.KanColle.Dentan.Record;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moen.KanColle.Dentan.ViewModel.Record
{
    class ExpeditionHistoryViewModel : ModelBase
    {
        public ObservableRangeCollection<ExpeditionRecord.Item> Records { get; } = new ObservableRangeCollection<ExpeditionRecord.Item>();
        
        public async Task LoadData()
        {
            var rRecords = await Task<List<ExpeditionRecord.Item>>.Run(() => RecordManager.Instance.Expedition.GetRecords());

            Records.AddRange(rRecords);
        }
    }
}
