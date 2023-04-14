namespace IntexFagElGamous.Models.ViewModels
{
    public class FilterViewModel
    {
        public bool Male { get; set; }
        public bool Female { get; set; }
        public bool HeadWest { get; set; }
        public bool HeadEast { get; set; }
        public decimal? MinDepth { get; set; }
        public decimal? MaxDepth { get; set; }
        public bool Adult { get; set; }
        public bool Child { get; set; }
        public bool Infant { get; set; }
        public bool Newborn { get; set; }
        public bool Brown { get; set; }
        public bool Black { get; set; }
        public bool BrownRed { get; set; }
        public bool Red { get; set; }
        public bool Blond { get; set; }
        public bool Unknown { get; set; }
        public bool Full { get; set; }
        public bool Partial { get; set; }
        public bool Bones { get; set; }
        public string? TextileColor { get; set; }
        public bool North { get; set; }
        public bool South { get; set; }
        public bool East { get; set; }
        public bool West { get; set; }
        public int? SquareNorthSouth { get; set; }

        public int? SquareEastWest { get; set; }
        public bool NorthEastArea { get; set; }
        public bool NorthWestArea { get; set; }
        public bool SouthEastArea { get; set; }
        public bool SouthWestArea { get; set; }
        public int? BurialNumber { get; set; }
        public string? TextileFunction { get; set; }
        public string? FaceBundle { get; set; }

    }
}
