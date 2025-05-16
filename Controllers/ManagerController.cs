using HrManagementSystem.Data;
using HrManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Drawing;

namespace HrManagementSystem.Controllers
{
    [Authorize(Roles = "Manager")]

    public class ManagerController : Controller
    {
        #region ~~variables
        UserManager<User> _userManager;

        SignInManager<User> _signInManager;

        RoleManager<Role> _roleManager;
        private readonly ICompositeViewEngine _viewEngine;

        private readonly HrDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly object workbook;
        #endregion
        public ManagerController(ICompositeViewEngine viewEngine, IWebHostEnvironment hostingEnvironment, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, HrDbContext dbContext)
        {
            _viewEngine = viewEngine;

            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._hostingEnvironment = hostingEnvironment;
            var options = new DbContextOptionsBuilder<HrDbContext>().UseSqlServer("RSDatabase").Options;
            this._context = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ViewTimesheets()
        {

            var user = await _userManager.GetUserAsync(User);
            var timesheets = _context.Timesheets
                .Include(t => t.Client)
                .Include(t => t.Project)
                .Include(t => t.Activity)
                .Where(t => t.EmployeeId == user.Id)
                .ToList();
            //        var timesheets = await _context.Timesheets
            //            .Include(t => t.Employee)
            //            .Include(t => t.Project)
            //            .Include(t => t.Client)
            //            .Where(t => t.approval == null) //  logic
            //            .ToListAsync();
            //        var data = _context.Timesheets
            //.Include(t => t.Employee) // assuming navigation property is Employee
            //.Select(t => new DailyTimesheetEntry
            //{
            //    ClientId = t.ClientId,
            //    ProjectId = t.ProjectId.ToString(),
            //    ActivityId=t.ActivityId,
            //    HoursWorked=t.HoursWorked,
            //    Description = t.Description,
            //    //ApprovalStatus=ApprovalStatus.Approved,

            //    //UserName = t.Employee.FirstName + " " + t.Employee.LastName,
            //    //Date = t.Date,
            //    //HoursWorked = t.HoursWorked,
            //    //Approval = t.Approval
            //}).ToList();

            return View(timesheets);
        }
        [HttpGet]
        public async Task<IActionResult> ApproveTimesheet()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ApproveTimesheet(int id)
        {
            var timesheet = await _context.Timesheets.FindAsync(id);
            if (timesheet == null) return NotFound();

            timesheet.approval = true;
            timesheet.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            timesheet.Date = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction("ViewTimesheets");
        }
        [HttpGet]
        public async Task<IActionResult> DisapproveTimesheet()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DisapproveTimesheet(int id, string? reason)
        {
            var timesheet = await _context.Timesheets.FindAsync(id);
            if (timesheet == null) return NotFound();

            timesheet.approval = false;
            timesheet.DisapprovalReason = reason;
            timesheet.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            timesheet.Date = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction("ViewTimesheets");
        }
        [HttpGet]
        public IActionResult AssignEmployee()
        {

            var employees = _userManager.GetUsersInRoleAsync("Employee").Result
        .Select(e => new SelectListItem
        {
            Value = e.Id,
            Text = e.UserName // or FullName
        }).ToList();

            var clients = _userManager.GetUsersInRoleAsync("Client").Result
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.UserName
                }).ToList();

            ViewBag.EmployeeList = employees;
            ViewBag.ClientList = clients;

            return View();
        }

        [HttpPost]
        public IActionResult AssignEmployee(string EmployeeId, List<string> ClientIds)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var clientId in ClientIds)
            {
                _context.EmployeeClientAssignments.Add(new EmployeeClientAssignment
                {
                    EmployeeId = EmployeeId,
                    ClientId = clientId,
                    ManagerId = managerId
                });
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]

        public IActionResult CreateAppraisalTemplate()
        {

            var deptList = _context.Departments.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();
            ViewBag.Department = deptList;

            var ClientList = _context.EmployeeClientAssignments.Select(r => new SelectListItem
            {
                Value = r.ClientId.ToString(),
                Text = r.Client.UserName
            }).ToList();
            ViewBag.Client = ClientList;

            var ActivityList = _context.Activities.Select(r => new SelectListItem
            {
                Value = r.ActivityId.ToString(),
                Text = r.ActivityName
            }).ToList();
            ViewBag.Activity = ActivityList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAppraisalTemplate([FromForm] IFormFile file, AppTemplateLatest template)
        {
            try
            {
                if (template == null || template.MeasuringKeys == null || template.MeasuringKeys.Count < 5)
                {
                    TempData["Error"] = "Incomplete input data.";
                    return View(template);
                }

                // Create folder and path
                string fileName = $"Appraisal_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "AppraisalTemplate");
                Directory.CreateDirectory(folderPath);
                string filePath = Path.Combine(folderPath, fileName);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // License

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("AppraisalTemplate");

                    // Header
                    worksheet.Cells[1, 1].Value = "Department Name";
                    worksheet.Cells[1, 2].Value = "Activity Name";
                    worksheet.Cells[1, 3].Value = "Client Name";
                    worksheet.Cells[1, 4].Value = "Measuring Key 1";
                    worksheet.Cells[1, 5].Value = "Measuring Key 2";
                    worksheet.Cells[1, 6].Value = "Measuring Key 3";
                    worksheet.Cells[1, 7].Value = "Measuring Key 4";
                    worksheet.Cells[1, 8].Value = "Measuring Key 5";

                    // Header style
                    using (var range = worksheet.Cells[1, 1, 1, 8])
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                    }

                    // Data
                    var dept = _context.Departments.Find(template.DepartmentId);
                    var activity = _context.Activities.Find(template.ActivityId);
                    var client = _context.Users.Find(template.ClientId);

                    worksheet.Cells[2, 1].Value = dept?.Name ?? "N/A";
                    worksheet.Cells[2, 2].Value = activity?.ActivityName ?? "N/A";
                    worksheet.Cells[2, 3].Value = client?.FirstName ?? "N/A";

                    for (int i = 0; i < 5; i++)
                    {
                        worksheet.Cells[2, 4 + i].Value = template.MeasuringKeys[i];
                    }

                    package.SaveAs(new FileInfo(filePath));
                }

                // Save to database
                var appraisal = new AppTemplateLatest
                {
                    ClientId = template.ClientId,
                    ActivityId = template.ActivityId,
                    MeasuringKeys = template.MeasuringKeys,
                    ManagerId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    CreatedDate = DateTime.Now,
                    DepartmentId = template.DepartmentId,
                    FileName = fileName,
                    FilePath = Path.Combine("AppraisalTemplate", fileName)
                };

                _context.appraisalTemplatesLatest.Add(appraisal);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Appraisal template saved successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error saving appraisal template: " + ex.Message;
            }

            // reload dropdowns if error
            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name");
            ViewBag.Client = new SelectList(_context.EmployeeClientAssignments.Select(e => new { e.ClientId, Name = e.Client.UserName }).Distinct().ToList(), "ClientId", "Name");
            ViewBag.Activities = new SelectList(_context.Activities.ToList(), "ActivityId", "ActivityName");

            return View(template);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateAppraisalTemplate([FromForm] IFormFile file, AppraisalTemplate template)
        //{
        //    try
        //    {
        //        // AT first convert the HTMl to excel and save in folder, write the logic for that (Research)

        //        //if (ModelState.IsValid)
        //        //{
        //        // 1. Render View to string (HTML)
        //        //  string htmlContent = await RenderViewToStringAsync("AppraisalTemplate", template); // a partial or clean version of the view

        //        // 2. Save as Excel using EPPlus
        //        string fileName = $"Appraisal_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        //        string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "AppraisalTemplate");
        //        Directory.CreateDirectory(folderPath);
        //        string filePath = Path.Combine(folderPath, fileName);

        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set license context here 

        //        using (var package = new ExcelPackage())
        //        {
        //            //  ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //            var worksheet = package.Workbook.Worksheets.Add("CreateAppraisalTemplate");


        //            worksheet.Cells[1, 1].Value = "Department Name";
        //            worksheet.Cells[1, 2].Value = "Activity Name";
        //            worksheet.Cells[1, 3].Value = "Client Name";

        //            worksheet.Cells[1, 4].Value = "Measuring Keys";
        //            using (var range = worksheet.Cells[1, 1, 1, 3]) // From cell 1,1 to 1,3
        //            {
        //                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                range.Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue); // or any Color
        //            }


        //            worksheet.Cells[2, 1].Value =_context.Departments.Find(template.DepartmentId).Name; // Handle null exceptions
        //            worksheet.Cells[2, 2].Value = _context.Activities.Find(template.ActivityId).ActivityName; // Null checking


        //            worksheet.Cells[2, 3].Value =_context.Users.Find(template.ClientId).FirstName; //Null checking must be done
        //            //  worksheet.Cells[2, 4].Value = template.CreatedByManagerId;  
        //            //  worksheet.Cells[2, 5].Value = template.CreatedDate;
        //            worksheet.Cells[2, 4].Value = template.MeasuringKeys[0];
        //            worksheet.Cells[2, 5].Value = template.MeasuringKeys[1];
        //            worksheet.Cells[2, 6].Value = template.MeasuringKeys[2];
        //            worksheet.Cells[2, 7].Value = template.MeasuringKeys[3];
        //            worksheet.Cells[2, 8].Value = template.MeasuringKeys[4];



        //            //worksheet.Cells[2, 7].Value = template.FileName;
        //            //worksheet.Cells[2, 8].Value = template.FilePath;

        //            /*                    workbook.SaveAs(filePath)*/
        //            ;


        //            package.SaveAs(new FileInfo(filePath));


        //            if (file == null || template == null || template.MeasuringKeys == null )
        //            {
        //                TempData["Error"] = "Invalid input data.";
        //                return View(template);
        //            }
        //            AppraisalTemplate temp = new AppraisalTemplate
        //            {
        //                Id = template.Id,
        //                ClientId = template.ClientId,
        //                Activity = template.Activity,
        //                MeasuringKeys = template.MeasuringKeys,
        //                CreatedByManagerId = User.FindFirstValue(ClaimTypes.NameIdentifier),
        //                CreatedDate = DateTime.Now,
        //                Department = template.Department,
        //                FileName = file.FileName,
        //                FilePath = folderPath,
        //            };
        //            _context.appraisalTemplates.Add(temp);
        //            _context.SaveChanges();
        //            ViewBag.Message = "Appraisal template saved successfully!";
        //            return RedirectToAction("Index");
        //            //TempData["Success"] = "Appraisal HTML saved to Excel.";
        //            //return RedirectToAction("Index");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = "Error saving Appraisal HTML to Excel: " + ex.Message;
        //    }

        //    // reload dropdowns
        //    ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name");
        //    ViewBag.Client = new SelectList(_context.EmployeeClientAssignments.ToList(), "Id", "Name");

        //    ViewBag.Activities = new SelectList(_context.Activities.ToList(), "Id", "Name");
        //    return View(template);
        //}
        public async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor);

            using var sw = new StringWriter();
            var viewResult = _viewEngine.FindView(actionContext, viewName, false);

            if (viewResult.View == null)
                throw new ArgumentNullException($"{viewName} does not match any available view");

            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDictionary,
                new TempDataDictionary(HttpContext, (ITempDataProvider)TempData),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }

        //if (file != null)
        //    {
        //        template.CreatedDate = DateTime.Now;

        //        // Check the file extension  
        //        if (Path.GetExtension(file.FileName).ToLower() == ".xlsx" || Path.GetExtension(file.FileName).ToLower() == ".xls")
        //        {
        //            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Templates", file.FileName);

        //            using (var stream = new FileStream(folderPath, FileMode.Create))
        //            {
        //                file.CopyToAsync(stream).Wait();
        //            }

        //            AppraisalTemplate temp = new AppraisalTemplate
        //            {
        //                Id = template.Id,
        //                ClientId = template.ClientId,
        //                Activity = template.Activity,
        //                MeasuringKeys = template.MeasuringKeys,
        //                CreatedByManagerId = User.FindFirstValue(ClaimTypes.NameIdentifier),
        //                CreatedDate = DateTime.Now,
        //                Department = template.Department,
        //                FileName = file.FileName,
        //                FilePath = folderPath,
        //            };

        //            _context.appraisalTemplates.Add(temp);
        //            _context.SaveChanges();
        //            ViewBag.Message = "Appraisal template saved successfully!";
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            return BadRequest("Invalid file type. Please upload an Excel file.");
        //        }
        //    }


        //    return View(template);
        //}
        public async Task<List<SelectListItem>> getUserByRoles(string roleName)
        {
            var usersInRole = new List<User>();
            usersInRole = await GetUsersInRoleAsync(roleName);


            var selectUserList = usersInRole.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            }).ToList();



            return selectUserList;
        }
        private async Task<List<User>> GetUsersInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.Where(x => x.EmailConfirmed == true).ToList();
        }
    }
}


