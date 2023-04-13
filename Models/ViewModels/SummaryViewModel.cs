namespace IntexFagElGamous.Models.ViewModels
{
    public class SummaryViewModel
    {
        public IQueryable<Burialmain>? Burialmains { get; set; }
        public IQueryable<BurialmainTextile>? BurialmainTextiles { get; set; }
        public IQueryable<Textile>? Textiles { get; set; }
    }
}
