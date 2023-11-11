using Microsoft.AspNetCore.Mvc;

namespace FinalEcormer2023.Views.Shared.Components.PageDivide
{
    public class PageDivideViewComponent : ViewComponent
    {
        public PageDivideViewComponent()
        {

        }

        public IViewComponentResult Invoke(SPager PageDivide)
        {
            return View("PageDivide", PageDivide);
        }
    }
}
