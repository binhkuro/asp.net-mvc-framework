using FinalEcormer2023.Models;

namespace FinalEcormer2023.Sales {
    public class CheckoutRequest {
        public string Name {  get; set; }   
        public string Email { get; set; }   
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
          
        public List<CartItem> orderDetails { get; set; }    
    }
}
