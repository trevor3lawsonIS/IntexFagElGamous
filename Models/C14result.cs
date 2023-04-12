using System;
using System.Collections.Generic;

namespace IntexFagElGamous.Models
{
    public partial class C14result
    {
        public decimal Sample { get; set; }
        public decimal? SquareNorthSouth { get; set; }
        public char? NorthSouth { get; set; }
        public decimal? SquareEastWest { get; set; }
        public char? EastWest { get; set; }
        public string? Area { get; set; }
        public decimal? BurialNumber { get; set; }
        public string? Description { get; set; }
        public decimal? AgeBp { get; set; }
        public string? CalendarDate { get; set; }
    }
}
