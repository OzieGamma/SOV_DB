namespace DB.Models
{
    public sealed class ProductionCast : IDatabaseModel
    {
        public int ProductionId;
        public int PersonId;
        public int? CharacterId;
        public CharacterRole Role;

        public void InsertIntoDb()
        {
            //throw new System.NotImplementedException();
        }
    }
}