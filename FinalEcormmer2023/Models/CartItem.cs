namespace FinalEcormer2023.Models {
	public class CartItem {
		public Product Product { get; set; }	
		public int Quantity {  get; set; }	
		private decimal _SubTotal {  get; set; }	
		public decimal SubTotal {
			get { return _SubTotal; }
			set { _SubTotal = Product.Price * Quantity; }	
		}
	}

}
