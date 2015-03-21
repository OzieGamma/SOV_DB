namespace DB.Models
{
    public sealed class ProductionCast : IDbModel
    {
        public long ProductionId;
        public long PersonId;
        public long CharacterId;
        public CharacterRole Role;

        public void InsertIntoDb()
        {
            throw new System.NotImplementedException();
        }
    }
}