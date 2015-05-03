﻿namespace DBGui.Models
{
    public sealed class Character
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public PersonInfo[] PlayedBy { get; private set; }
        public ProductionInfo[] AppearsIn { get; private set; }
    }
}