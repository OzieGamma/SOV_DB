using System;
using DBGui.Models;

namespace DBGui
{
    public partial class ProductionWindow
    {
        private readonly int _productionId;
        private Production _production;

        public Production Production
        {
            get { return _production; }
            private set { Set( ref _production, value ); }
        }

        public ProductionWindow( int productionId )
        {
            _productionId = productionId;
        }

        protected override void Load()
        {
            throw new NotImplementedException();
        }
    }
}