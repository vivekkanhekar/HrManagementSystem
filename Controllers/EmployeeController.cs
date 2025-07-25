using HrManagementSystem.Data;
using HrManagementSystem.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
//using static HrManagementSystem.Models.Timesheet;

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
        //public IActionResult Index()
        //{
        //    return View();
        //}
        //public IActionResult Index()
        //{
        //    try
        //    {
        //        var userId = _userManager.GetUserId(User); // Get logged-in employee's ID

        //        var approvedTimesheets = _context.Timesheets
        //            .Where(t => /*t.EmployeeId == userId &&*/ t.approval == true)
        //            .Select(t => new TimesheetEntryViewModel
        //            {
        //                Date = t.Date,
        //                ClientId = t.Client.UserName,
        //                ProjectId = t.Project.ProjectName,
        //                ManagerId = t.Manager.UserName,
        //                HoursWorked = t.HoursWorked,
        //                Description = t.Description
        //            })
        //            .ToList();

        //        return View(approvedTimesheets);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = "An error occurred while loading the timesheets.";
        //        return View("Error");
        //    }

        //}

        public IActionResult Index(DateTime? startDate, DateTime? endDate, string? clientName, string? searchTerm)
        {
            try
            {
                var userId = _userManager.GetUserId(User);

                var query = _context.Timesheets
                    .Where(t => t.approval == true)
                    .AsQueryable();

                if (startDate.HasValue)
                    query = query.Where(t => t.Date >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(t => t.Date <= endDate.Value);

                if (!string.IsNullOrEmpty(clientName))
                    query = query.Where(t => t.Client.UserName.Contains(clientName));

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(t =>
                        t.Project.ProjectName.Contains(searchTerm) ||
                        t.Manager.UserName.Contains(searchTerm) ||
                        t.Description.Contains(searchTerm)
                    );
                }

                var approvedTimesheets = query
                    .Select(t => new TimesheetEntryViewModel
                    {
                        Date = t.Date,
                        ClientId = t.Client.UserName,
                        ProjectId = t.Project.ProjectName,
                        ManagerId = t.Manager.UserName,
                        HoursWorked = t.HoursWorked,
                        Description = t.Description
                    }).ToList();

                return View(approvedTimesheets);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading the timesheets.";
                return View("Error");
            }
        }

        #region  LeavePolicy
        public IActionResult ViewLeavePolicy()
        {
            var empId = _userManager.GetUserId(User);

            // Step 1: Get the client(s) assigned to this employee
            var assignedClientIds = _context.EmployeeClientAssignments
                .Where(e => e.EmployeeId == empId)
                .Select(e => e.ClientId)
                .ToList();

            // Step 2: Get leave policies ONLY for those clients
            var policies = _context.ClientLeavePolicies
                .Where(lp => assignedClientIds.Contains(lp.ClientID))
                .Include(lp => lp.Client)
                .ToList();

            return View(policies);
        }
        [HttpGet]
        public async Task<IActionResult> ApplyLeave()
        {
            var vm = new ApplyLeaveViewModel
            {
                LeaveTypes = _context.LeaveTypes.Where(x => x.IsActive).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyLeave(ApplyLeaveViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    model.LeaveTypes = _context.LeaveTypes.Where(x => x.IsActive).ToList();
            //    return View(model);
            //}

            var leaveRequest = new LeaveApplication
            {
                EmployeeID = User.FindFirstValue(ClaimTypes.NameIdentifier),
                LeaveTypeId = model.LeaveTypeId,
                IsHalfDay = model.DurationType == "HalfDay",
                FromDate = model.DurationType == "FullDay" ? model.FromDate.GetValueOrDefault() : model.SingleDate.GetValueOrDefault(),
                ToDate = model.DurationType == "FullDay" ? model.ToDate.GetValueOrDefault() : model.SingleDate.GetValueOrDefault(),
                Reason = model.Reason,
                SubmittedOn = DateTime.Now
            };

            if (model.Attachment != null)
            {
                // Save file to server and store path (simplified)
                var fileName = Path.GetFileName(model.Attachment.FileName);
                var path = Path.Combine("wwwroot/uploads", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await model.Attachment.CopyToAsync(stream);
                leaveRequest.AttachmentPath = "/uploads/" + fileName;
            }

            _context.LeaveApplications.Add(leaveRequest);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Leave request submitted successfully.";
            return RedirectToAction("Index", "Employee");
        }

        #endregion

        #region Timesheet crud
        [HttpGet]
        public async Task<IActionResult> CreateTimesheet()
        {
            try
            {
                //var model = new WeeklyTimesheetViewModel
                //{
                //    Clients = _context.Timesheets.Select(c => new SelectListItem { Value = c.ClientId.ToString(), Text = c.Client.UserName }).ToList(),
                //    Projects = _context.Project.Select(p => new SelectListItem { Value = p.ProjectId.ToString(), Text = p.ProjectName }).ToList(),
                //    ManagerID = _context.Timesheets.Select(m => new SelectListItem { Value = m.ManagerID.ToString(), Text = m.Manager.UserName }).ToList(),

                //    ActivityTasks = _context.Activities.Select(a => new SelectListItem { Value = a.ActivityId.ToString(), Text = a.ActivityName }).ToList()
                //};
                //return View(model);
                var clientList = await _userManager.GetUsersInRoleAsync("Client");
                var managerList = await _userManager.GetUsersInRoleAsync("Manager");
                var model = new WeeklyTimesheetViewModel
                {
                    Clients = clientList.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FirstName }).ToList(), // _context.Timesheets.Select(c => new SelectListItem { Value = c.ClientId.ToString(), Text = c.Client.UserName }).ToList(),
                    Projects = _context.Project.Select(p => new SelectListItem { Value = p.ProjectId.ToString(), Text = p.ProjectName }).ToList(),
                    ManagerID = managerList.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FirstName }).ToList(),


                    ActivityTasks = _context.Activities.Select(a => new SelectListItem { Value = a.ActivityId.ToString(), Text = a.ActivityName }).ToList()
                };
                return View(model);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading the timesheet form.";
                return View("Error");
            }


        }

        [HttpPost]
        public IActionResult CreateTimesheet(WeeklyTimesheetViewModel model)
        {
            try
            {
                foreach (var entry in model.DailyEntries)
                {
                    var timesheet = new Timesheet
                    {
                        EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        ClientId = entry.ClientId.ToString(),
                        ProjectId = entry.ProjectId.ToString(),
                        ManagerID = entry.ManagerID.ToString(),
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
            
            catch (Exception ex)
            {
                
                ViewBag.ErrorMessage = "An error occurred while submitting the timesheet.";
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var timesheet = await _context.Timesheets
               .FirstOrDefaultAsync(t => t.TimesheetId == id);

                if (timesheet == null)
                    return NotFound();


                var user = await _userManager.GetUserAsync(User);
                var clientUsers = await GetUsersInRoleAsync("Client");
                var managerList = await _userManager.GetUsersInRoleAsync("Manager");

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

                var ManagerList = managerList.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.UserName
                }).ToList();
                ViewBag.Manager = ManagerList;

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
                    ManagerId=timesheet.ManagerID,
                    ActivityId = timesheet.ActivityId,
                    HoursWorked = timesheet.HoursWorked,
                    Description = timesheet.Description,

                };

                ViewBag.TimesheetId = id;
                return View(viewModel);
            }

            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading the timesheet form.";
                return View("Error");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TimesheetEntryViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                var timesheet = await _context.Timesheets.FindAsync(id);
                if (timesheet == null) return NotFound();

                timesheet.Date = model.Date;
                timesheet.ClientId = model.ClientId;
                timesheet.ProjectId = model.ProjectId;
                timesheet.ManagerID = model.ManagerId;
                timesheet.ActivityId = model.ActivityId;
                timesheet.HoursWorked = model.HoursWorked;
                timesheet.Description = model.Description;

                await _context.SaveChangesAsync();
                return RedirectToAction("MyTimesheets");
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMessage = "An error occurred while editting the timesheet.";
                return View("Error");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var timesheet = await _context.Timesheets
                .Include(t => t.Client)
                .Include(t => t.Project)
                .Include(t => t.Activity)
                .FirstOrDefaultAsync(t => t.TimesheetId == id);

                if (timesheet == null)
                    return NotFound();
                //return View();  
                return View(timesheet);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMessage = "An error occurred while deleting the timesheet.";
                return View("Error");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var timesheet = await _context.Timesheets.FindAsync(id);
                if (timesheet == null)
                    return NotFound();

                _context.Timesheets.Remove(timesheet);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyTimesheets");
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMessage = "An error occurred while deleting the timesheet.";
                return View("Error");
            }

        }

        public async Task<IActionResult> MyTimesheets()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var timesheets = _context.Timesheets
                    .Include(t => t.Client)
                    .Include(t => t.Project)
                    .Include(t => t.Manager)
                    .Include(t => t.Activity)
                    .Where(t => t.EmployeeId == user.Id)
                    .ToList();
                //return View(user);
                return View(timesheets);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading the timesheets.";
                return View("Error");
            }

        }
        #endregion
        public IActionResult SubmitTimesheet()
        {
            try
            {
                var empId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.Clients = new SelectList(_context.EmployeeClientAssignments
                                                  .Where(x => x.EmployeeId == empId)
                                                  .Select(x => x.ClientId), "ClientId", "ClientId");
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading the timesheet form.";
                return View("Error");
            }

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
            try
            {
                var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    TempData["Error"] = "User is not authenticated.";
                    return RedirectToAction("Login", "Account");
                }
                var assignedClientIds = _context.EmployeeClientAssignments
                .Where(x => x.EmployeeId == employeeId)
                .Select(x => x.ClientId)
                .ToList();

                if (!assignedClientIds.Any())
                {
                    ViewBag.Message = "No clients assigned.";
                    return View(new List<AppTemplateLatest>());
                }

                var templates = _context.appraisalTemplatesLatest
                    .Where(t => assignedClientIds.Contains(t.ClientId))
                    .Include(t => t.Client)
                    .Include(t => t.Manager)
                    .Include(t => t.Department)
                    .Include(t => t.Activity)
                    //.Include(t => t.Remarks)
                    //.Include(t => t.Amount)
                    .ToList();

                if (!templates.Any())
                {
                    ViewBag.Message = "No appraisal templates found.";
                }
                return View(templates);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading the appraisal templates.";
                return View("Error");
            }
            // Get templates where employee is assigned via Client -> EmployeeClientAssignment

        }

        [HttpPost]
        public IActionResult SubmitAppraisal(int templateId, string[] selfAssessment,string[] remark)
        {
            try
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
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while submitting the appraisal.";
                return View("Error");
            }

        }

       

        [HttpPost]
        public IActionResult Appraisals(int templateId, string[] keyNames, string[] scores, string remark,string amount)
        {
            try
            {
                var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var remarkList=new List<string>();
                var amountList = new List<string>();
                var keyEntryList = new List<string>();

                var ratingPercents = new List<string>();

                for (int i = 0; i < keyNames.Length; i++)
                {
                    double score = double.Parse(scores[i]);
                    double percent = (score / 10.0) * 100;
                    keyEntryList.Add($"{keyNames[i]}:{scores[i]}:{remark[i]}:{amount[i]}:{percent}%");
                    ratingPercents.Add($"{percent}%");
                }

                for (int i = 0; i < keyNames.Length; i++)
                {
                    keyEntryList.Add($"{keyNames[i]}:{scores[i]}:{remark[i]}:{amount[i]}");
                }

                for (int i = 0; i < remark.Length; i++)
                {
                    remarkList.Add($"{remark[i]}");
                }
                for(int i = 0; i < amount.Length; i++)
                {
                    amountList.Add($"{amount[i]}");
                }
                var joinedKeyEntries = string.Join(", ", keyEntryList,remarkList,amountList);

                var existing = _context.appraisalResponses
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
                    Remarks = remarkList,
                    Amount = amountList,
                    SubmittedDate = DateTime.Now,


                    //Remarks = remark,
                };

                _context.appraisalResponses.Add(appraisalentry);
                _context.SaveChanges();

                TempData["Message"] = "Self-assessment submitted successfully.";
                return RedirectToAction("Appraisals");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while submitting the appraisal.";
                return View("Error");
            }

        }
        [HttpGet]
        public IActionResult MyAppraisals()
        {
            var empId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var responses = _context.appraisalResponses
                                    .Where(r => r.EmployeeId == empId)
                                    .ToList();
            return View(responses);
        }
        [HttpGet]
       
        public IActionResult EditAppraisal(int id)
        {
            var empId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = _context.appraisalResponses.FirstOrDefault(r => r.Id == id && r.EmployeeId == empId);
            if (response == null)
                return NotFound();

            return View(response);
        }
        [HttpPost]
        public IActionResult EditAppraisal(int Id, string[] KeyLabels, string[] ScoreInputs)
        {
            var existing = _context.appraisalResponses.FirstOrDefault(r => r.Id == Id);
            if (existing != null)
            {
                var updatedEntries = new List<string>();
                for (int i = 0; i < KeyLabels.Length; i++)
                {
                    updatedEntries.Add($"{KeyLabels[i]}:{ScoreInputs[i]}");
                }

                existing.KeyEntries = updatedEntries; 
                existing.SubmittedDate = DateTime.Now;
                _context.SaveChanges();
            }

            return RedirectToAction("MyAppraisals");
        }


        //[HttpPost]
        //public IActionResult EditAppraisal(AppraisalResponse updatedResponse)
        //{
        //    var existing = _context.appraisalResponses.FirstOrDefault(r => r.Id == updatedResponse.Id);
        //    if (existing != null)
        //    {
        //        existing.AppraisalTemplateId = updatedResponse.AppraisalTemplateId;
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("MyAppraisals");
        //}

        [HttpGet]
        public IActionResult DeleteAppraisal(int id)
        {
            var empId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = _context.appraisalResponses.FirstOrDefault(r => r.Id == id && r.EmployeeId == empId);
            if (response == null)
                return NotFound();

            return View(response);
        }

       
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _context.appraisalResponses.FirstOrDefault(r => r.Id == id);
            if (response != null)
            {
                _context.appraisalResponses.Remove(response);
                _context.SaveChanges();
            }
            return RedirectToAction("MyAppraisals");
        }



        private async Task<List<User>> GetUsersInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.Where(x => x.IsActive == true).ToList();
        }
       
    }
}
