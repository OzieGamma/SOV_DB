namespace DB.Models
{
    public sealed class ProductionCast
    {
        public long ProductionId;
        public long PersonId;
        public long? CharacterId;
        public CharacterRole Role;
    }
}