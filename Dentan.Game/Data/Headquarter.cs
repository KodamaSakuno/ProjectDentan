﻿using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Data
{
    public class Headquarter : ModelBase
    {
        public Admiral Admiral { get; private set; }

        public Material Material { get; private set; }

        public Headquarter()
        {
            Material = new Material();
        }

        public void UpdateAdmiral(RawBasic rpData)
        {
            if (Admiral == null)
                Admiral = new Admiral(rpData);
            else
                Admiral.Update(rpData);

            OnPropertyChanged(() => Admiral);
        }
    }
}
