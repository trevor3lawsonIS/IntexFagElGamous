namespace IntexFagElGamous.Models.ViewModels
{
    public class FilterViewModel
    {
        public bool Male { get; set; }
        public bool Female { get; set; }
        public bool West { get; set; }
        public bool East { get; set; }
        public decimal? MinDepth { get; set; }
        public decimal? MaxDepth { get; set; }
        public bool Adult { get; set; }
        public bool Child { get; set; }
        public bool Brown { get; set; }
        public bool Black { get; set; }
        public bool BrownRed { get; set; }
        public bool Red { get; set; }
        public bool Blond { get; set; }
        public bool Unknown { get; set; }
        public bool Full { get; set; }
        public bool Partial { get; set; }
        public bool Bones { get; set; }
    }
}
