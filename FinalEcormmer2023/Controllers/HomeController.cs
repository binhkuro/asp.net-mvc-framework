using FinalEcormer2023.Data;
using FinalEcormer2023.Interfaces;
using FinalEcormer2023.ViewModels;
using FinalEcormmer2023.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace FinalEcormmer2023.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
		private readonly ApplicationDbContext _context;

		public HomeController(ILogger<HomeController> logger , IProductService productService, ICategoryService categoryService,ApplicationDbContext context) {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _context = context;
        }

        public async Task<IActionResult> Index() {
            var products = await _productService.GetAll();
            var category = await _categoryService.GetAll();
            var homeViewModel = new HomeViewModel() {
                categories = category,
                products = products.ToList(),
            };
            return View(homeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Shop(int? page, int? categoryId = 0) {
			var products = await _context.Products.Include(X => X.category).ToListAsync();

            /*var colors = products.Select(p => p.Color).Distinct().ToList(); */
			int pageCurrent = page ?? 1;
			int totalRecords = products.Count();
			int pageSize = 3;
			int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            HomeViewModel homeViewModel = new HomeViewModel() {
                categories = await _categoryService.GetAll(),
                products = products.Skip((pageCurrent - 1) * pageSize).Take(pageSize),
                PageSize = pageSize,
                CurrentPage = pageCurrent,
                TotalPage = totalPages,
			};
            /*return Ok(color);*/
            return View("Main", homeViewModel);
        }

        [HttpGet]
        //keyword 
        public async Task<IActionResult> Search(string? keyword , int? page, string sortOrder ,  int? categoryId = 0 ) {
            ViewBag.keyword = keyword;  
            ViewBag.categoryId = categoryId;    
            ViewBag.url = "?keyword=" + keyword + "&categoryId=" + categoryId + "&sortOrder=" + sortOrder ;

			var products =  await _context.Products.Include(x => x.category).ToListAsync();
			var colors = products.Select(p => p.Color).Distinct().ToList();

			if (!String.IsNullOrEmpty(keyword)) {
				products = products
					.Where(p =>
						p.Name.ToLower().Contains(keyword.ToLower()) ||
						p.Color.ToLower().Contains(keyword.ToLower()) ||
						p.Description.ToLower().Contains(keyword.ToLower())
					)
					.ToList();
			}

			if (categoryId != 0) {
				products = products.Where(p => p.CategoryId.Equals(categoryId)).ToList();
			}
			
            switch (sortOrder) {
				case "price_asc": {
                        products = products.OrderBy(p => p.Price).ToList();
                        break;  
                }
                case "price_desc": {
                        products = products.OrderByDescending(p => p.Price).ToList();
                        break;  
                }
                default: {
                        products = products.OrderBy(p => p.Id).ToList();
                        break;
                }
            }

            int pageCurrent = page ?? 1;
			int totalRecords = products.Count();
			int pageSize = 3;
			int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            HomeViewModel homeViewModel = new HomeViewModel() {
                categories = await _categoryService.GetAll(),
                products = products.Skip((pageCurrent - 1) * pageSize).Take(pageSize),
                PageSize = pageSize,
                CurrentPage = pageCurrent,
                TotalPage = totalPages,
                colors = colors,
			};

            /*return Ok(homeViewModel);*/
            return View("Main", homeViewModel);
        }

        [HttpGet]
        //price 
        //color
        //category 
        public async Task<IActionResult> Filter(string? color,string sortOrder ,  double pMin = 0, double pMax = 0, int? page=1, int categoryId = 0 , int? pageSize =3 ) {
            try {
                ViewBag.Color = color;
                ViewBag.url = "?color=" + color + "&pMin=" + pMin + "&pMax=" + pMax + "&categoryId=" + categoryId  + "&sortOrder=" + sortOrder; 
				var products = await _productService.GetAll();
				var colors = products.Select(p => p.Color).Distinct().ToList();

				if (pMin >= 0 && pMax > pMin) {
					products = products.Where(p => p.Price >= new decimal(pMin) && p.Price <= new decimal(pMax));
				}
				if (categoryId != 0) {
					products = products.Where(p => p.category.Id == categoryId);
				}
				if (color != null) {
					products = products.Where(p => p.Color.ToLower().Contains(color.ToLower()));
				}

				switch (sortOrder) {
					case "price_asc": {
							products = products.OrderBy(p => p.Price).ToList();
							break;
						}
					case "price_desc": {
							products = products.OrderByDescending(p => p.Price).ToList();
							break;
						}
					default: {
							products = products.OrderBy(p => p.Id).ToList();
							break;
						}
				}

				int pageCurrent = page ?? 1;
				int pageS = pageSize ?? 3;
				int totalRecords = products.Count();
				int totalPages = (int)Math.Ceiling((double)totalRecords / pageS);

				HomeViewModel homeViewModel = new HomeViewModel() {
					categories = await _categoryService.GetAll(),
					products = products.Skip((pageCurrent - 1) * pageS).Take(pageS),
					PageSize = pageS,
					CurrentPage = pageCurrent,
					TotalPage = totalPages,
					colors = colors
				};
				return View("Main", homeViewModel);
			} catch (Exception ex)
            {
                return View("NotFound");
            }
          
        }

        public async Task<IActionResult> Detail(int? id) {
            if (id == null || _context.Products == null) {
				return NotFound();
			}
            var product = await _context.Products.Include(p => p.category).FirstOrDefaultAsync(m => m.Id == id);
			if (product == null) {
				return NotFound();
			}
			return View(product);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}