namespace Moen.KanColle.Dentan.Data.QuestData
{
    abstract class CarrierProgress : DestroyTargetProgress
    {
        protected CarrierProgress()
            : base(new[] { 7, 11 }) { }
    }
    [Quest(211, QuestType.Daily, 3)]
    class Carrier3Progress : CarrierProgress
    {
    }
    [Quest(220, QuestType.Weekly, 20)]
    class CodeIProgress : CarrierProgress
    {
    }
}
