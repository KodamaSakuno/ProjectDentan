using Moen.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    public class EquipmentsViewModel : ModelBase
    {
        int r_Count;
        public int Count
        {
            get { return r_Count; }
            set
            {
                if (r_Count != value)
                {
                    r_Count = value;
                    OnPropertyChanged();
                }
            }
        }

        EquipmentTypeViewModel[] r_Types;
        public EquipmentTypeViewModel[] Types
        {
            get { return r_Types; }
            set
            {
                if (r_Types != value)
                {
                    r_Types = value;
                    OnPropertyChanged();
                }
            }
        }

        bool r_IsSelectedAll;
        public bool IsSelectedAll
        {
            get { return r_IsSelectedAll; }
            set
            {
                if (r_IsSelectedAll != value)
                {
                    r_IsSelectedAll = value;

                    foreach (var rType in Types)
                        rType.IsSelected = value;

                    OnPropertyChanged();
                }
            }
        }

        EquipmentTypeGroupViewModel[] r_TypeGroups;
        public EquipmentTypeGroupViewModel[] TypeGroups
        {
            get { return r_TypeGroups; }
            internal set
            {
                if (r_TypeGroups != value)
                {
                    r_TypeGroups = value;
                    OnPropertyChanged();
                }
            }
        }

        public EquipmentsViewModel()
        {
            r_IsSelectedAll = true;

            KanColleGame.Current.BaseDataLoaded += UpdateType;

            var rShips = KanColleGame.Current.ObservablePropertyChanged.Where(r => r == "Ships").FirstAsync();

            KanColleGame.Current.ObservablePropertyChanged.Where(r => r == "Equipments").Subscribe(_ =>
            {
                Count = KanColleGame.Current.Equipments.Count;

                Task.Run(async () =>
                {
                    await rShips;

                    TypeGroups = KanColleGame.Current.Equipments.Values.GroupBy(r => r.Info).OrderBy(r => r.Key.ID)
                        .Select(r => new EquipmentTypeGroupViewModel(r.Key, Types.Single(rpType => rpType.Type == r.Key.IconType), r)).ToArray();
                });
            });
        }

        public void UpdateType()
        {
            Task.Run(() =>
            {
                Types = KanColleGame.Current.Base.Equipments.Values.GroupBy(r => r.IconType)
                    .Select(r => new EquipmentTypeViewModel(r.Key) { IsSelected = IsSelectedAll }).ToArray();
            });
        }
    }
}
