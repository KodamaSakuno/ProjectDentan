using Moen.Collections;
using Moen.KanColle.Dentan.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class EquipmentLevelGroupViewModel : ModelBase
    {
        public int Level { get; private set; }
        public int Count { get; private set; }
        public EquipmentInfo Info { get; private set; }

        public ObservableRangeCollection<EquipedShipViewModel> EquipedShips { get; private set; }

        public EquipmentLevelGroupViewModel(int rpLevel, IEnumerable<Equipment> rpEquipments, EquipmentInfo rpInfo)
        {
            Level = rpLevel;
            Count = rpEquipments.Count();
            Info = rpInfo;
            EquipedShips = new ObservableRangeCollection<EquipedShipViewModel>();

            Task.Run(() =>
            {
                var rData = App.Root.Game.Ships.Ships.Select(r => new EquipedShipViewModel(r, r.Model.Slots.Count(rpSlot => rpSlot.Equipment.Info == Info && rpSlot.Equipment.Level == Level))).Where(r => r.Count > 0);
                DispatcherUtil.UIDispatcher.BeginInvoke(new Action<IEnumerable<EquipedShipViewModel>>(EquipedShips.AddRange), rData);
            });
        }
    }
}
