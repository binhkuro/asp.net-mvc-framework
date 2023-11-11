using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalEcormer2023.Data;
using FinalEcormmer2023.Models;
using FinalEcormer2023.Service;
using FinalEcormer2023.Interfaces;
using System.Runtime.InteropServices;
using Microsoft.Identity.Client;

namespace FinalEcormer2023.Controllers {
	public class ProductController : Controller {
		private readonly IProductService _productService;
		public ProductController(IProductService productService, ApplicationDbContext context) {
			_productService = productService;
		}

		public IActionResult Index(string? keyword) {
			var products = _productService.GetAll();
			return View(products);
		}

		public IActionResult Detail(int id) {
			var product = _productService.GetById(id);
			return View(product);
		}
	}
}
