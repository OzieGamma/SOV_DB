﻿using DBGui.Models;

namespace DBGui
{
    public partial class ProductionWindow
    {
        private readonly int _id;
        private Production _production;

        public Production Production
        {
            get { return _production; }
            private set { Set( ref _production, value ); }
        }

        public ProductionWindow( int id )
        {
            _id = id;

            // TEMP
            Title = "Production TODO";
        }
    }
}