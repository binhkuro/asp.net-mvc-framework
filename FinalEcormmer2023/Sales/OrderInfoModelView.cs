using FinalEcormer2023.Models;

namespace FinalEcormer2023.Sales {
	public class OrderInfoModelView {
		public Order  order {  get; set; }	
		public List<OrderDetail> orderDetail { get; set; }
	}
}
