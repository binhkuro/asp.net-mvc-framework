using FinalEcormer2023.Models;
using FinalEcormmer2023.Models;

namespace FinalEcormer2023.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<Product> products { get; set; }

        public int PageSize {  get; set; }   
        public int CurrentPage {  get; set; }   
        public int TotalPage { get; set; } 
        public List<String> colors { get; set; }
	}
}
