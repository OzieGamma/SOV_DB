namespace DB.Models
{
    public sealed class ProductionCast
    {
        public int ProductionId;
        public int PersonId;
        public int? CharacterId;
        public CharacterRole Role;

        public override string ToString()
        {
            return string.Join( "\t", ProductionId, PersonId, CharacterId, Role );
        }
    }
}