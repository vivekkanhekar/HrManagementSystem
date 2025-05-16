using HrManagementSystem.Data;
using HrManagementSystem.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static HrManagementSystem.Models.Timesheet;

namespace HrManagementSystem.Controllers
{
    [Authorize(Roles = "Employee")]

    public class EmployeeController : Controller
    {
        #region ~~variables
        UserManager<User> _userManager;

        SignInManager<User> _signInManager;

        RoleManager<Role> _roleManager;

        private readonly HrDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        #endregion

        #region ~~Constructor
        public EmployeeController(IWebHostEnvironment hostingEnvironment, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, HrDbContext dbContext)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._hostingEnvironment = hostingEnvironment;
            var options = new DbContextOptionsBuilder<HrDbContext>().UseSqlServer("RSDatabase").Options;
            this._context = dbContext;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
        #region Timesheet crud
        [HttpGet]
        public IActionResult CreateTimesheet()
        {
            var model = new WeeklyTimesheetViewModel
            {
                Clients = _context.Timesheets.Select(c => new SelectListItem { Value = c.ClientId.ToString(), Text = c.Client.UserName }).ToList(),
                Projects = _context.Project.Select(p => new SelectListItem { Value = p.ProjectId.ToString(), Text = p.ProjectName }).ToList(),
                ActivityTasks = _context.Activities.Select(a => new SelectListItem { Value = a.ActivityId.ToString(), Text = a.ActivityName }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateTimesheet(WeeklyTimesheetViewModel model)
        {
            foreach (var entry in model.DailyEntries)
            {
                var timesheet = new Timesheet
                {
                    EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    ClientId = entry.ClientId.ToString(),
                    ProjectId = entry.ProjectId.ToString(),
                    ActivityId = entry.ActivityId,
                    Date = entry.Date,
                    HoursWorked = entry.HoursWorked,
                    Description = entry.Description,
                    approval = entry.Equals(true),
                };
                _context.Timesheets.Add(timesheet);
            }

            _context.SaveChanges();
            return RedirectToAction("MyTimesheets");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var timesheet = await _context.Timesheets
                .FirstOrDefaultAsync(t => t.TimesheetId == id);

            if (timesheet == null)
                return NotFound();


            var user = await _userManager.GetUserAsync(User);
            var clientUsers = await GetUsersInRoleAsync("Client");

            var selectClientList = clientUsers.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            }).ToList(); // To bind client dropdown?
            ViewBag.Client = selectClientList;
            var ProjectList = _context.Project.Select(r => new SelectListItem
            {
                Value = r.ProjectId.ToString(),
                Text = r.ProjectName
            }).ToList();
            ViewBag.Project = ProjectList;

            var ActivityList = _context.Activities.Select(r => new SelectListItem
            {
                Value = r.ActivityId.ToString(),
                Text = r.ActivityName
            }).ToList();
            ViewBag.Activity = ActivityList;

            var viewModel = new TimesheetEntryViewModel
            {
                Date = timesheet.Date,
                ClientId = timesheet.ClientId,
                ProjectId = timesheet.ProjectId,
                ActivityId = timesheet.ActivityId,
                HoursWorked = timesheet.HoursWorked,
                Description = timesheet.Description,
                
            };

            ViewBag.TimesheetId = id;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TimesheetEntryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var timesheet = await _context.Timesheets.FindAsync(id);
            if (timesheet == null) return NotFound();

            timesheet.Date = model.Date;
            timesheet.ClientId = model.ClientId;
            timesheet.ProjectId = model.ProjectId;
            timesheet.ActivityId = model.ActivityId;
            timesheet.HoursWorked = model.HoursWorked;
            timesheet.Description = model.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction("MyTimesheets");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var timesheet = await _context.Timesheets
                .Include(t => t.Client)
                .Include(t => t.Project)
                .Include(t => t.Activity)
                .FirstOrDefaultAsync(t => t.TimesheetId == id);

            if (timesheet == null)
                return NotFound();

            return View(timesheet);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var timesheet = await _context.Timesheets.FindAsync(id);
            if (timesheet == null)
                return NotFound();

            _context.Timesheets.Remove(timesheet);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyTimesheets");
        }

        public async Task<IActionResult> MyTimesheets()
        {
            var user = await _userManager.GetUserAsync(User);
            var timesheets = _context.Timesheets
                .Include(t => t.Client)
                .Include(t => t.Project)
                .Include(t => t.Activity)
                .Where(t => t.EmployeeId == user.Id)
                .ToList();

            return View(timesheets);
        }
        #endregion
        public IActionResult SubmitTimesheet()
        {
            var empId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Clients = new SelectList(_context.EmployeeClientAssignments
                                              .Where(x => x.EmployeeId == empId)
                                              .Select(x => x.ClientId), "ClientId", "ClientId");
            return View();
        }
        //[HttpPost]
        //public IActionResult SubmitTimesheet(Timesheet timesheet)
        //{
        //    timesheet.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    timesheet.approval = "Pending";
        //    _context.Timesheets.Add(timesheet);
        //    _context.SaveChanges();
        //    return RedirectToAction("MyTimesheets");
        //}
        [HttpGet]
        public IActionResult Appraisals()
        {
            //var empId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var managerId = _context.EmployeeClientAssignments
            //                        .FirstOrDefault(x => x.EmployeeId == empId)?.ManagerId;
            //var templates = _context.appraisalTemplates.Where(t => t.CreatedByManagerId == managerId).ToList();

            var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(employeeId))
            {
                TempData["Error"] = "User is not authenticated.";
                return RedirectToAction("Login", "Account");
            }
            // Get templates where employee is assigned via Client -> EmployeeClientAssignment
            var assignedClientIds = _context.EmployeeClientAssignments
                .Where(x => x.EmployeeId == employeeId)
                .Select(x => x.ClientId)
                .ToList();

            if (!assignedClientIds.Any())
            {
                ViewBag.Message = "No clients assigned.";
                return View(new List<AppraisalTemplate>());
            }

            var templates = _context.appraisalTemplatesLatest
                .Where(t => assignedClientIds.Contains(t.ClientId))
                .Include(t => t.Client)
                .Include(t => t.Manager)
                .Include(t => t.Department)
                .Include(t => t.Activity)
                .ToList();

            if (!templates.Any())
            {
                ViewBag.Message = "No appraisal templates found.";
            }
            return View(templates);
        }

        [HttpPost]
        public IActionResult SubmitAppraisal(int templateId, string[] selfAssessment)
        {
            var empId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //_context.appraisalResponses.Add(new AppraisalResponse
            //{
            //    AppraisalTemplateId = templateId,
            //    EmployeeId = empId,
            //    KeyEntries = selfAssessment
            //});
            _context.SaveChanges();
            return RedirectToAction("Appraisals");
        }
        [HttpPost]
        public IActionResult Appraisals(int templateId, string[] keyNames, string[] scores)
        {
            var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var keyEntryList = new List<string>();
            for (int i = 0; i < keyNames.Length; i++)
            {
                keyEntryList.Add($"{keyNames[i]}:{scores[i]}");
            }

            var joinedKeyEntries = string.Join(", ", keyEntryList);

            var existing =_context.appraisalResponses
                .FirstOrDefault(x => x.Template.Id == templateId && x.EmployeeId == employeeId);

            //if (existing != null)
            //{
            //    // Prevent duplicate or update instead
            //    TempData["Message"] = "Self-assessment already submitted.";
            //    return RedirectToAction("Appraisals");
            //}

            var appraisalentry = new AppraisalResponse
            {
                AppraisalTemplateId = templateId,
                EmployeeId = employeeId,
                KeyEntries = keyEntryList,
                SubmittedDate = DateTime.Now
            };

            _context.appraisalResponses.Add(appraisalentry);
            _context.SaveChanges();

            TempData["Message"] = "Self-assessment submitted successfully.";
            return RedirectToAction("Appraisals");
        }



        private async Task<List<User>> GetUsersInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.Where(x => x.IsActive == true).ToList();
        }
       
    }
}
