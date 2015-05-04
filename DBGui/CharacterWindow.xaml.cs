using DBGui.Models;

namespace DBGui
{
    public sealed partial class CharacterWindow
    {
        private readonly int _id;
        private Character _character;

        public Character Character
        {
            get { return _character; }
            private set { Set( ref _character, value ); }
        }

        public CharacterWindow( int id )
        {
            _id = id;
        }

        protected override void Load()
        {
            DoAsync( async () => Character = await Character.GetAsync( _id ) );
        }
    }
}