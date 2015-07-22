
namespace Moen.KanColle.Dentan.Data
{
    public class Experience
    {
        public int Level { get; private set; }
        public int Total { get; private set; }
        public int Next { get; private set; }

        internal Experience(int rpLevel, int rpTotal, int rpNext)
        {
            Level = rpLevel;
            Total = rpTotal;
            Next = rpNext;
        }
    }
}
