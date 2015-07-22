using Moen.Collections;
using Moen.KanColle.Dentan.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class EquipmentTypeGroupViewModel : ModelBase
    {
        public EquipmentInfo Info { get; private set; }
        public EquipmentTypeViewModel Type { get; private set; }
        public ObservableRangeCollection<EquipmentLevelGroupViewModel> Levels { get; private set; }

        internal EquipmentTypeGroupViewModel(EquipmentInfo rpInfo, EquipmentTypeViewModel rpType, IEnumerable<Equipment> rpEquipments)
        {
            Info = rpInfo;
            Type = rpType;
            Levels = new ObservableRangeCollection<EquipmentLevelGroupViewModel>();

            var rLevels = rpEquipments.GroupBy(r => r.Level).OrderBy(r => r.Key).Select(r => new EquipmentLevelGroupViewModel(r.Key, r, Info));
            DispatcherUtil.UIDispatcher.BeginInvoke(new Action<IEnumerable<EquipmentLevelGroupViewModel>>(Levels.AddRange), rLevels);
        }
    }
}
