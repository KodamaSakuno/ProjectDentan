using Moen.Collections;
using Moen.KanColle.Dentan.Model.Game;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace Moen.KanColle.Dentan.ViewModel.Game
{
    class ImprovementArsenalViewModel : ModelBase
    {
        static Table<ImprovementArsenalModel> r_Models;
        ImprovementArsenalEquipmentViewModel[] r_Equipments;
        public ObservableRangeCollection<ImprovementArsenalEquipmentViewModel> Equipments { get; } = new ObservableRangeCollection<ImprovementArsenalEquipmentViewModel>();

        static ImprovementArsenalViewModel()
        {
            using (var rReader = File.OpenText(@"Data\AkashiImprovementArsenal.json"))
            {
                var rData = JArray.Load(new JsonTextReader(rReader));
                r_Models = new Table<ImprovementArsenalModel>(rData.ToObject<ImprovementArsenalModel[]>());
            }
        }
        public ImprovementArsenalViewModel()
        {
            r_Equipments = r_Models.Values.Select(r => new ImprovementArsenalEquipmentViewModel(r)).ToArray();

            Update();
            KanColleGame.Current.ShipsUpdated += Update;
            KanColleGame.Current.EquipmentsUpdated += Update;
        }

        void Update()
        {
            foreach (var rEquipment in r_Equipments)
                rEquipment.Update();

            var rEquipments = r_Equipments.ToLookup(r => r.IsAvailable);

            DispatcherUtil.UIDispatcher.BeginInvoke(new Action(() =>
            {
                Equipments.Clear();
                Equipments.AddRange(rEquipments[true]);
                Equipments.AddRange(rEquipments[false]);
            }));
        }
    }
}
