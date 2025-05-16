using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrManagementSystem.Controllers
{
    [Authorize(Roles = "Client")]

    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewAssignedTimesheets()
        {
            return View();
        }
    }
}
