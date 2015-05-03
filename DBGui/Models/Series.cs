namespace DBGui.Models
{
    public sealed class Series : Production
    {
        public int? BeginningYear { get; private set; }
        public int? EndYear { get; private set; }
        public Episode[] Episodes { get; private set; }
    }
}