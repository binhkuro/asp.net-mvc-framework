using FinalEcormmer2023.Models;

namespace FinalEcormer2023.Models {
	public class Category {
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<Product> products{ get; set;}
	}
}
