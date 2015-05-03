using DBGui.Models;
namespace DBGui
{
    public sealed partial class PersonWindow
    {
        private readonly int _id;
        private Person _person;

        public Person Person
        {
            get { return _person; }
            private set { Set( ref _person, value ); }
        }


        public PersonWindow( int id )
        {
            _id = id;
        }


        protected override void Load()
        {
            DoAsync( async () => { Person = await Person.GetAsync( _id ); } );
        }
    }
}