using Microsoft.AspNetCore.Mvc;

namespace Budget.Web
{
    [Controller]
    public class HomeController : Controller
    {
        public ViewResult Index() => View("Index.cshtml"); 
    }
}
