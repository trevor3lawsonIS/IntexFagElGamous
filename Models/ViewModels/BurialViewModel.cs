namespace IntexFagElGamous.Models.ViewModels
{
    public class BurialViewModel
    {
        public IQueryable<Burialmain> Burialmains { get; set; }
        
        public IQueryable<FilterViewModel> Filters { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
