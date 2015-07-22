using Moen.KanColle.Dentan.Data.Raw;

namespace Moen.KanColle.Dentan.Data
{
    public class Material : ModelBase
    {
        internal bool IsDirty { get; set; }

        int r_Fuel;
        public int Fuel
        {
            get { return r_Fuel; }
            set
            {
                if (r_Fuel != value)
                {
                    r_Fuel = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }
        int r_Bullet;
        public int Bullet
        {
            get { return r_Bullet; }
            set
            {
                if (r_Bullet != value)
                {
                    r_Bullet = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }
        int r_Steel;
        public int Steel
        {
            get { return r_Steel; }
            set
            {
                if (r_Steel != value)
                {
                    r_Steel = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }
        int r_Bauxite;
        public int Bauxite
        {
            get { return r_Bauxite; }
            set
            {
                if (r_Bauxite != value)
                {
                    r_Bauxite = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }
        int r_DevelopmentMaterial;
        public int DevelopmentMaterial
        {
            get { return r_DevelopmentMaterial; }
            set
            {
                if (r_DevelopmentMaterial != value)
                {
                    r_DevelopmentMaterial = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }
        int r_Bucket;
        public int Bucket
        {
            get { return r_Bucket; }
            set
            {
                if (r_Bucket != value)
                {
                    r_Bucket = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }
        int r_InstantConstruction;
        public int InstantConstruction
        {
            get { return r_InstantConstruction; }
            set
            {
                if (r_InstantConstruction != value)
                {
                    r_InstantConstruction = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }
        int r_ImprovementMaterial;
        public int ImprovementMaterial
        {
            get { return r_ImprovementMaterial; }
            set
            {
                if (r_ImprovementMaterial != value)
                {
                    r_ImprovementMaterial = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        internal void Update(RawMaterial[] rpMaterials)
        {
            foreach (var rMaterial in rpMaterials)
            {
                switch (rMaterial.Type)
                {
                    case MaterialType.Fuel: Fuel = rMaterial.Value;
                        break;
                    case MaterialType.Bullet: Bullet = rMaterial.Value;
                        break;
                    case MaterialType.Steel: Steel = rMaterial.Value;
                        break;
                    case MaterialType.Bauxite: Bauxite = rMaterial.Value;
                        break;
                    case MaterialType.DevelopmentMaterial: DevelopmentMaterial = rMaterial.Value;
                        break;
                    case MaterialType.Bucket: Bucket = rMaterial.Value;
                        break;
                    case MaterialType.InstantConstruction: InstantConstruction = rMaterial.Value;
                        break;
                    case MaterialType.ImprovementMaterial: ImprovementMaterial = rMaterial.Value;
                        break;
                }
            }

        }
    }
}
