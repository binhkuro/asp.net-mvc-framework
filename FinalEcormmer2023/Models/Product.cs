namespace FinalEcormer2023.Models {
	public class Product {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Color { get; set; }
		public string Description { get; set; }
		public string Url { get; set; }
		public decimal Price { get; set; }
		public int CategoryId { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public Category category { get; set; }
		public ICollection<OrderDetail> orderDetails { get; set;}
	}
}
