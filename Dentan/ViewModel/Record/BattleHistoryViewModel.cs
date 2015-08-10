using Moen.Collections;
using Moen.KanColle.Dentan.Record;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moen.KanColle.Dentan.ViewModel.Record
{
    class BattleHistoryViewModel : ModelBase
    {
        public ObservableRangeCollection<SortieRecordViewModel> SortieRecords { get; } = new ObservableRangeCollection<SortieRecordViewModel>();
        public ObservableRangeCollection<PracticeRecordViewModel> PracticeRecords { get; } = new ObservableRangeCollection<PracticeRecordViewModel>();

        public async Task LoadData()
        {
            var rSortieRecords = await Task<IEnumerable<SortieRecordViewModel>>.Run(() => 
                RecordManager.Instance.Sortie.GetSortieRecords().Select(r => new SortieRecordViewModel(r)));
            SortieRecords.AddRange(rSortieRecords);

            var rPracticeRecords = await Task<IEnumerable<PracticeRecordViewModel>>.Run(() =>
                RecordManager.Instance.Battle.GetPracticeRecords().Select(r => new PracticeRecordViewModel(r)));
            PracticeRecords.AddRange(rPracticeRecords);
        }
    }
}
