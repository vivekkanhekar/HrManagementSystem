using HrManagementSystem.Data;
using HrManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ClosedXML.Excel;


namespace HRManagement.Controllers
{
    [Authorize(Roles = "Employee,Manager,Super Admin")]
    public class AttendanceController : Controller
    {
        #region ~~variables
        UserManager<User> _userManager;

        SignInManager<User> _signInManager;

        RoleManager<Role> _roleManager;

        private readonly HrDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        #endregion
        

        #region ~~Constructor
        public AttendanceController(IWebHostEnvironment hostingEnvironment, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, HrDbContext dbContext)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._hostingEnvironment = hostingEnvironment;
            var options = new DbContextOptionsBuilder<HrDbContext>().UseSqlServer("RSDatabase").Options;
            this._context = dbContext;
        }


        #endregion
        // Employee Dashboard -> My Attendance
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> MyAttendance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var records = await _context.EmployeeAttendances
                                        .Where(a => a.EmployeeId == userId)
                                        .OrderByDescending(a => a.AttendanceDate)
                                        .ToListAsync();
            return View(records);
        }

        // Punch-IN
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> PunchIn()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var today = DateTime.Now.Date;

            var record = await _context.EmployeeAttendances
                                       .FirstOrDefaultAsync(a => a.EmployeeId == userId && a.AttendanceDate == today);

            if (record == null)
            {
                record = new EmployeeAttendance
                {
                    EmployeeId = userId,
                    AttendanceDate = today,
                    PunchInTime = DateTime.Now,
                    Status = "Present"
                };
                _context.EmployeeAttendances.Add(record);
            }
            else if (record.PunchInTime == null)
            {
                record.PunchInTime = DateTime.Now;
                record.Status = "Present";
                _context.EmployeeAttendances.Update(record);
            }
            else
            {
                TempData["Error"] = "You have already punched in today.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("MyAttendance");
        }

        // Punch-OUT
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> PunchOut()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var today = DateTime.Now.Date;

            var record = await _context.EmployeeAttendances
                                       .FirstOrDefaultAsync(a => a.EmployeeId == userId && a.AttendanceDate == today);

            if (record != null && record.PunchInTime != null && record.PunchOutTime == null)
            {
                record.PunchOutTime = DateTime.Now;
                record.TotalHours = (decimal)(record.PunchOutTime.Value - record.PunchInTime.Value).TotalHours;
                record.Status = "Present";
                _context.EmployeeAttendances.Update(record);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["Error"] = "Punch-In not done or Punch-Out already marked.";
            }

            return RedirectToAction("MyAttendance");
        }

        // Manager/Admin View
        [Authorize(Roles = "Manager,Super Admin")]
        public async Task<IActionResult> Index()
        {
            var records = await _context.EmployeeAttendances
                                        .Include(a => a.Employee)
                                        .OrderByDescending(a => a.AttendanceDate)
                                        .ToListAsync();
            return View(records);
        }

       // [Authorize(Roles = "Manager,Super Admin")]
        //public async Task<IActionResult> AttendanceMonitoring(string filter = "daily")
        //{
        //    DateTime startDate = DateTime.Now.Date;
        //    DateTime endDate = DateTime.Now.Date;

        //    if (filter == "weekly")
        //    {
        //        startDate = DateTime.Now.Date.AddDays(-7);
        //    }
        //    else if (filter == "monthly")
        //    {
        //        startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //        endDate = startDate.AddMonths(1).AddDays(-1);
        //    }

        //    var records = await _context.EmployeeAttendances
        //        .Include(a => a.Employee)
        //        .Where(a => a.AttendanceDate >= startDate && a.AttendanceDate <= endDate)
        //        .OrderByDescending(a => a.AttendanceDate)
        //        .ToListAsync();

        //    ViewBag.Filter = filter;
        //    return View(records);
        //}

        //// Approve Correction
        //[HttpPost]
        //public async Task<IActionResult> ApproveCorrection(int id)
        //{
        //    var correction = await _context.AttendanceCorrections
        //        .Include(c => c.Attendance)
        //        .FirstOrDefaultAsync(c => c.CorrectionId == id);

        //    if (correction != null)
        //    {
        //        correction.Status = "Approved";
        //        correction.Attendance.PunchInTime = correction.RequestedPunchIn ?? correction.Attendance.PunchInTime;
        //        correction.Attendance.PunchOutTime = correction.RequestedPunchOut ?? correction.Attendance.PunchOutTime;

        //        if (correction.Attendance.PunchInTime != null && correction.Attendance.PunchOutTime != null)
        //        {
        //            correction.Attendance.TotalHours =
        //                (decimal)(correction.Attendance.PunchOutTime.Value - correction.Attendance.PunchInTime.Value).TotalHours;
        //        }

        //        _context.AttendanceCorrections.Update(correction);
        //        await _context.SaveChangesAsync();
        //    }

        //    return RedirectToAction("AttendanceMonitoring");
        //}

        //// Reject Correction
        //[HttpPost]
        //public async Task<IActionResult> RejectCorrection(int id)
        //{
        //    var correction = await _context.AttendanceCorrections.FindAsync(id);
        //    if (correction != null)
        //    {
        //        correction.Status = "Rejected";
        //        _context.AttendanceCorrections.Update(correction);
        //        await _context.SaveChangesAsync();
        //    }

        //    return RedirectToAction("AttendanceMonitoring");
        //}
        //public IActionResult ExportToExcel(string filter = "daily")
        //{
        //    // Fetch records using same logic as AttendanceMonitoring
        //    // Example using ClosedXML library
        //    var records = _context.EmployeeAttendances
        //        .Include(a => a.Employee)
        //        .OrderByDescending(a => a.AttendanceDate)
        //        .ToList();

        //    using (var workbook = new XLWorkbook())
        //    {
        //        var ws = workbook.Worksheets.Add("Attendance");
        //        ws.Cell(1, 1).Value = "Employee";
        //        ws.Cell(1, 2).Value = "Date";
        //        ws.Cell(1, 3).Value = "Punch In";
        //        ws.Cell(1, 4).Value = "Punch Out";
        //        ws.Cell(1, 5).Value = "Total Hours";
        //        ws.Cell(1, 6).Value = "Status";

        //        int row = 2;
        //        foreach (var rec in records)
        //        {
        //            ws.Cell(row, 1).Value = rec.Employee?.UserName;
        //            ws.Cell(row, 2).Value = rec.AttendanceDate;
        //            ws.Cell(row, 3).Value = rec.PunchInTime;
        //            ws.Cell(row, 4).Value = rec.PunchOutTime;
        //            ws.Cell(row, 5).Value = rec.TotalHours;
        //            ws.Cell(row, 6).Value = rec.Status;
        //            row++;
        //        }

        //        using (var stream = new System.IO.MemoryStream())
        //        {
        //            workbook.SaveAs(stream);
        //            var content = stream.ToArray();
        //            return File(content,
        //                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                "AttendanceReport.xlsx");
        //        }
        //    }
        //}

    }
}
