using AspNetCoreGeneratedDocument;
using HrManagementSystem.Data;
using HrManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using System.Data;
using System.Diagnostics;

namespace HrManagementSystem.Controllers
{
    [Authorize(Roles = "Super Admin")]


    public class SuperAdminController : Controller
    {
        #region ~~variables
        UserManager<User> _userManager;

        SignInManager<User> _signInManager;

        RoleManager<Role> _roleManager;

        private readonly HrDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        #endregion

        #region ~~Constructor
        public SuperAdminController(IWebHostEnvironment hostingEnvironment,UserManager<User> userManager,SignInManager<User> signInManager, RoleManager<Role> roleManager,HrDbContext dbContext)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._hostingEnvironment = hostingEnvironment;
            var options = new DbContextOptionsBuilder<HrDbContext>().UseSqlServer("RSDatabase").Options;
            this._context = dbContext;
        }


        #endregion
        
        #region Roles Details


        [HttpGet]
        public IActionResult RoleList()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();
                //var roles = _context.Roles.ToList();
                return View(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while listing the roles.",
                    error = ex.Message
                });
            }

        }

        [HttpGet]
        public IActionResult AddRole()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            try
            {
                if (!string.IsNullOrEmpty(roleName))
                {
                    Role role = new Role
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString() // Generate a unique concurrency stamp
                    };

                    if (!await _roleManager.RoleExistsAsync(role.Name))
                    {
                        var result = await _roleManager.CreateAsync((Role)role);

                        if (result.Succeeded)
                        {
                            TempData["Success"] = "Role added successfully.";
                        }
                        else
                        {
                            TempData["Error"] = "Error adding role.";
                        }

                        return RedirectToAction("RoleList");
                    }
                }

                TempData["Error"] = "Role name cannot be empty.";
                return RedirectToAction("RoleList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while adding the role.";
                return RedirectToAction("RoleList");
            }

        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id) // Change int? to string
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return NotFound();
                }

                //  var user = await _context.Roles.FindAsync(id); // check roles not user

                var role = _roleManager.Roles.ToList().Where(x => x.Id == id).First();
                if (role == null)
                {
                    return NotFound();
                }
                return View(role);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while retrieving the role.";
                return RedirectToAction("RoleList");
            }

        }
        [HttpPost]
        public async Task<IActionResult> EditRole(string id, string roleName)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    TempData["Error"] = "Role not found.";
                    return RedirectToAction("RoleList");
                }

                if (!string.IsNullOrEmpty(roleName))
                {
                    role.Name = roleName;
                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        TempData["Success"] = "Role updated successfully.";
                        return RedirectToAction("RoleList");
                    }
                    else
                    {
                        TempData["Error"] = "Error updating role.";
                    }
                }
                else
                {
                    TempData["Error"] = "Role name cannot be empty.";
                }

                return View(role);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating the role.";
                return RedirectToAction("RoleList");
            }

        }
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    TempData["Error"] = "Role not found.";
                    return RedirectToAction("RoleList");
                }

                return View(role);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while retrieving the role.";
                return RedirectToAction("RoleList");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoleConfirmed(string id)
        {

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["Error"] = "Role not found.";
                return RedirectToAction("RoleList");
            }

            try
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Role deleted successfully.";
                }
                else
                {
                    TempData["Error"] = "Error deleting the role.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the role.";
            }

            return RedirectToAction("RoleList"); // Redirect to Role List after deletion
        }

        #endregion


        #region Employee CRUD
        [HttpGet]
        public async Task<IActionResult> ViewEmployee()
        {
            try
            {
                var employees = await _userManager.GetUsersInRoleAsync("Employee");

                var model = employees.Select(emp => new EmployeeViewModel
                {
                    Id = emp.Id,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Email = emp.Email,
                    Address = emp.Address
                }).ToList();

                return View(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while listing the employees.",
                    error = ex.Message
                });
            }

        }


        [HttpGet]

        public IActionResult AddEmployee()

        {
            try
            {
                var model = new AddEmployeeViewModel();

                var emplist = _context.Roles.ToList().Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                }).ToList();


                ViewBag.Roles = emplist;
                var clientlist = _context.Timesheets.Select(r => new SelectListItem
                {
                    Value = r.ClientId.ToString(),
                    Text = r.Client.UserName,
                }).ToList();
                ViewBag.Clients = clientlist;
                // Get department list
                var deplist = _context.Departments.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                }).ToList();
                ViewBag.Departments = deplist;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the Add Employee page: " + ex.Message);
                return View(new AddEmployeeViewModel());
            }

            //ViewBag.Roles = _roleManager.Roles.ToList();
            //return View();

        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existingUser != null)
                    {
                        return StatusCode(409, "Error: This email is already in use.");
                    }
                    //var userName = new string(model.FirstName.Where(char.IsLetterOrDigit).ToArray());
                    var password = Guid.NewGuid().ToString("N").Substring(0, 8); // Auto-generate password
                    var user = new User
                    {
                        UserName = model.FirstName,
                        Email = model.Email,
                        PhoneNumber = model.Phone,
                        //Department model.Department
                        Password = password,
                        Address = model.Address,
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    };
                    var result = await _userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync("Employee"))
                            await _roleManager.CreateAsync(new Role { Name = "SuperAdmin" });

                        await _userManager.AddToRoleAsync(user, "Employee");
                        return RedirectToAction("ViewEmployee");
                    }
                    else

                    {
                        ModelState.AddModelError("", "Error creating user");
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while adding the employee: " + ex.Message);
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult EditEmployee(string id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                User user = _context.Users.Find(id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the Edit Employee page: " + ex.Message);
                return View(new AddEmployeeViewModel());
            }

        }
        [HttpPost]

        public IActionResult EditEmployee(String? id, AddEmployeeViewModel userModle )
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                User user = _context.Users.Find(id);

                //ClientUser clientUser = _context.ClientUser.Include(x => x.department).Include(x => x.client).Where(x => x.User.Id == id).FirstOrDefault();

                if (user != null)
                {
                    user.FirstName = userModle.FirstName;
                    user.LastName = userModle.LastName;
                    user.Email = userModle.Email;
                    user.PhoneNumber = userModle.Phone;
                    user.Address = userModle.Address;

                    _context.Users.Update(user);
                    _context.SaveChanges();
                    //userModle.FirstName = user.FirstName;
                    //userModle.LastName = user.LastName;

                    //userModle.Email = user.Email;
                    //userModle.PhoneNumber = user.PhoneNumber;
                    //userModle.Address = user.Address;

                    //_context.Update(user);

                    //_context.SaveChanges();

                    return RedirectToAction("ViewEmployee"); // Redirecting to the list after editing


                }
                //ViewBag.CLientUser = clientUser;

                return View(userModle);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating employee: " + ex.Message);
                return View(ex);
            }

        }


        [HttpGet]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the Delete Employee page: " + ex.Message);
                return View(new User());
            }
            // Pass user data to confirmation view
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployeeConfirmed(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }

                return RedirectToAction("ViewEmployee");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the employee: " + ex.Message);
                return View(new User());
            }
             // Redirect after deletion
        }
        #endregion

        #region Department
        [HttpGet]
        public async Task<IActionResult> ViewDepartment(int? pageNumber)
        {
            try
            {
                int pageSize = 5;
                List<Department> depList = _context.Departments.ToList();

                return View(depList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while listing the departments.",
                    error = ex.Message
                });
            }


        }
        [HttpGet]
        public IActionResult AddDepartment()
        {
            try
            {
                List<Department> depTypeList = _context.Departments.ToList();// bind the dropdown for DepartmentType
                var selectDepTypeList = depTypeList.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                }).ToList();
                ViewBag.DepartmentType = selectDepTypeList;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the Add Department page: " + ex.Message);
                return View(new Department());
            }   

        }

        [HttpPost]
        public IActionResult AddDepartment(Department depModule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var department = new Department
                    {
                        Name = depModule.Name,
                        Description = depModule.Description, 
                    };

                    _context.Departments.Add(department);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Department added successfully!";
                    return RedirectToAction("ViewDepartment"); // Redirect to the department list page
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while adding the department.";
            }

            return View(depModule); // Return to the form with validation errors
        }

        [HttpGet]
        public IActionResult EditDepartment(int? id)
        {
            try
            {
                Department department = _context.Departments.Find(id);


                if (department != null)
                {
                    return View(department);

                }
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the Edit Department page: " + ex.Message);
                return View(new Department());
            }
        }

        [HttpPost]
        public IActionResult EditDepartment(int id,Department depModel)
        {
            try
            {
                if (depModel == null || depModel.Id == 0)
                {
                    TempData["ErrorMessage"] = "Invalid department details.";
                    return RedirectToAction("Index"); // Redirect to department list
                }

                Department department = _context.Departments.Find(depModel.Id);
                if (department != null)
                {
                    department.Name = depModel.Name;
                    department.Description = depModel.Description;

                    _context.Update(department);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Department updated successfully!";
                    return RedirectToAction("ViewDepartment"); // Redirect after update
                }

                TempData["ErrorMessage"] = "Department not found.";
                return RedirectToAction("ViewDepartment");
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the department: " + ex.Message;
                return View(depModel);
            }

        }

        [HttpGet]
        public IActionResult DeleteDepartment(int? id, Department depModle)
        {
            try
            {
                Department dep = _context.Departments.Find(id);

                if (dep != null)
                {
                    depModle.Name = dep.Name;
                    depModle.Description = dep.Description;
                    // where is delete command?

                    _context.SaveChanges();
                }

                ModelState.AddModelError("Confirm Delete Department ! " + dep.Name, "");
                return View(depModle);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the Delete Department page: " + ex.Message);
                return View(new Department());
            }
            //var g = Convert.ToInt32(Request.Form["id"]);


        }
        [HttpPost]
public IActionResult DeleteDepartment(int id)
{
            try
            {
                Department department = _context.Departments.Find(id);
                if (department != null)
                {
                    _context.Departments.Remove(department);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = $"Department {department.Name} deleted successfully!";
                    return RedirectToAction("ViewDepartment");
                }

                TempData["ErrorMessage"] = "Department not found.";
                return RedirectToAction("ViewDepartment");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the department: " + ex.Message;
                return RedirectToAction("ViewDepartment");
            }

        }

        #endregion
        #region Client CRUD
        [HttpGet]
        public async Task<IActionResult> ClientList(int? pageNumber)
        {

            try
            {
                int pageSize = 9;
                var usersInManagerRole = await _userManager.GetUsersInRoleAsync("Client");
                List<User> userList = _context.Users.ToList();
                List<User> adminList = new List<User>();

                foreach (User user in userList)
                {
                    var isInUserRole = await _userManager.IsInRoleAsync(user, "Client");
                    if (isInUserRole && user.IsActive)
                    {
                        adminList.Add(user);
                    }
                }

                // Implement pagination if needed
                var paginatedList = usersInManagerRole.ToList();
                return View(PaginationList<User>.Create(paginatedList, pageNumber ?? 1, 9));

                //return View(paginatedList);
            }
            
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while listing the clients.",
                    error = ex.Message
                });
            }

        }

        [HttpGet]
        public async Task<IActionResult> AddClient()
        {
            try
            {
                var userUsers = await getUserByRoles("Manager");

                var selectUserList = userUsers.Select(u => new SelectListItem
                {
                    Value = u.Value,
                    Text = u.Text
                }).ToList();

                ViewBag.Admin = selectUserList;
                return View();
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the Add Client page: " + ex.Message);
                return View(new User());
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddClient(User model)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return StatusCode(409, "Error: This email is already in use.");
                }


                var password = Guid.NewGuid().ToString("N").Substring(0, 8); // Auto-generate password
                var user = new User
                {
                    //clientId = model.clientId,
                    //Email = model.Email,
                    UserName = model.FirstName,
                    FirstName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,

                    Password = password,
                    Address = model.Address,
                    //Access = false,
                    //Name = userModule.Client.Name            //FirstName = model.Admin
                    //LastName = model.LastName

                    LastName = model.LastName


                };
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Client"))
                        await _roleManager.CreateAsync(new Role { Name = "Client" });

                    await _userManager.AddToRoleAsync(user, "Client");
                    return RedirectToAction("ClientList");
                }
                else

                {
                    ModelState.AddModelError("", "Error creating Manager");
                }
            

                

               
                    //return Ok("Success: Client Added.");
                    return View();

                
            }
            catch (Exception ex)
            {
                // Check if the exception message or inner exception contains information about a duplicate entry
                if (ex.Message.Contains("duplicate") || ex.InnerException?.Message.Contains("duplicate") == true)
                {
                    // Return 409 Conflict with a specific duplicate email message
                    return StatusCode(409, "Error: This email is already in use.");
                }

                // For other errors, return a 500 Internal Server Error
                return StatusCode(500, "Error: Client Not Added.!");
            }


        }

        [HttpGet]
        public async Task<IActionResult> EditClient(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound("Error: Client not found.");
                }

                // Create a model and fill it with user data
                var model = new User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address
                };

                return View(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: Unable to load client data.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditClient(string id, User model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound("Error: Client not found.");
                }

                // Check if email is being updated and already exists
                if (user.Email != model.Email)
                {
                    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existingUser != null)
                    {
                        return StatusCode(409, "Error: This email is already in use.");
                    }
                }

                // Update client details

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.UserName = model.Email; // Ensure username consistency

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ClientList");
                }

                return BadRequest(result.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: Unable to update client.");
            }
        }
        #endregion

        #region Manager CRUD 
        [HttpGet]
        public async Task<IActionResult> ViewManager(int? pageNumber)
        {
            try
            {
                int pageSize = 9;
                var usersInManagerRole = await _userManager.GetUsersInRoleAsync("Manager"); // use this direct way of fetching users in roles 
                List<User> userList = _context.Users.ToList();
                List<User> adminList = new List<User>();

                foreach (User user in userList)
                {
                    var isInUserRole = await _userManager.IsInRoleAsync(user, "Manager");
                    if (isInUserRole && user.IsActive)
                    {
                        adminList.Add(user);
                    }
                }

                // Implement pagination if needed
                var paginatedList = usersInManagerRole.ToList();
                return View(PaginationList<User>.Create(paginatedList, pageNumber ?? 1, 9));

                //return View(paginatedList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while listing the manager.",
                    error = ex.Message
                });
            }

        }


        [HttpGet]
        public IActionResult AddManager()
        {
            //var emplist = _context.Roles.ToList().Select(r => new SelectListItem
            //{
            //    Value = r.Id.ToString(),
            //    Text = r.Name
            //}).ToList();


            //ViewBag.Roles = emplist;
            //// Get department list
            //var deplist = _context.Departments.Select(r => new SelectListItem
            //{
            //    Value = r.Id.ToString(),
            //    Text = r.Name
            //}).ToList();
            //ViewBag.Departments = deplist;
            return View();
            
        }

       

        [HttpPost] // This was the problem
        public async Task<IActionResult> AddManager(String? id,AddEmployeeViewModel model)
        {
            var password = Guid.NewGuid().ToString("N").Substring(0, 8);
            User user = _context.Users.Find(id);

            if (user == null)
            {
                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = password, 

                    PhoneNumber = model.Phone,
                    Address = model.Address,
                    UserName = model.FirstName
                };

                var result = await _userManager.CreateAsync(user, password); // Provide password here

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Manager"))
                        await _roleManager.CreateAsync(new Role { Name = "Manager" });

                    await _userManager.AddToRoleAsync(user, "Manager");

                    // Optionally: Send password to email or display once (for demo purposes)
                    TempData["SuccessMessage"] = $"Manager added successfully. Temporary Password: {password}";

                    return RedirectToAction("ViewManager");
                }
                else
                {
                    ModelState.AddModelError("", "Error creating Manager: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                ModelState.AddModelError("", "User '" + model.FirstName + "' already exists!");
            }

            //try
            //{
            //    //if (ModelState.IsValid)
            //    //{
            //    //var existingUser = await _userManager.FindByEmailAsync(model.Email);
            //    //if (existingUser != null)
            //    //{
            //    //    return StatusCode(409, "Error: This email is already in use.");
            //    //}
            //    //var userName = new string(model.FirstName.Where(char.IsLetterOrDigit).ToArray());
            //    // var password = Guid.NewGuid().ToString("N").Substring(0, 8); // Auto-generate password
            //    User user = _context.Users.Find(id);
            //    if (user == null) //if user exist no need to add records
            //    {
            //        user=new User();
            //        user.FirstName = model.FirstName;
            //        user.LastName = model.LastName;
            //        user.Email = model.Email;
            //        user.PhoneNumber = model.Phone;
            //        user.Password= Guid.NewGuid().ToString("N").Substring(0, 8);
            //        user.Address = model.Address;
            //        user.UserName = model.FirstName;
            //        //var user = new User
            //        //    {
            //        //        UserName = model.FirstName,
            //        //        Email = model.Email,
            //        //        PhoneNumber = model.Phone,
            //        //        //Department model.Department
            //        //        Address = model.Address,
            //        //        FirstName = model.FirstName,
            //        //        LastName = model.LastName
            //        //    };

            //        var result = await _userManager.CreateAsync(user);

            //        if (result.Succeeded)
            //        {
            //            if (!await _roleManager.RoleExistsAsync("Manager"))
            //                await _roleManager.CreateAsync(new Role { Name = "Manager" });

            //            await _userManager.AddToRoleAsync(user, "Manager");
            //            return RedirectToAction("ViewManager");
            //        }
            //        else

            //        {
            //            ModelState.AddModelError("", "Error creating Manager");
            //        }
            //    }

            //    if (user != null)
            //    {
            //        ModelState.AddModelError("", "User '"+model.FirstName+"' already exist.! ");
            //    }

            //   //}
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("", "An error occurred while adding the manager: " + ex.Message);
            //}
            return View(model);

        }


        [HttpGet]
        public async Task<IActionResult> EditManager(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var model = new AddEmployeeViewModel
            {
                //Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Address = user.Address
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditManager(string id,AddEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (id == null)
            {
                return NotFound();
            }
            User user = _context.Users.Find(id);
            //var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ModelState.AddModelError("", "Manager not found.");
                return View(model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.Address = model.Address;
            user.UserName = model.FirstName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Manager updated successfully.";
                return RedirectToAction("ViewManager");
            }

            ModelState.AddModelError("", "Error updating manager: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteManager(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the Delete Employee page: " + ex.Message);
                return View(new User());
            }
            // Pass user data to confirmation view
        }
        [HttpPost]
        public async Task<IActionResult> DeleteManagerConfirmed(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
                TempData["SuccessMessage"] = "Manager deleted successfully.";
                return RedirectToAction("ViewManager");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the employee: " + ex.Message);
                return View(new User());
            }
            // Redirect after deletion
        }
        
        #endregion
        #region Projects

        [HttpGet]
        public IActionResult ViewProjects()
        {
            try
            {
                var projects = _context.Project.ToList();
                return View(projects);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while listing the projects.",
                    error = ex.Message
                });
            }

        }
        [HttpGet]

        public IActionResult AddProject() => View();

        // POST: SuperAdmin/AddProject
        [HttpPost]
        public IActionResult AddProject(Projects project)
        {
            try
            {
                Projects projectToAdd = new Projects();
                projectToAdd.ProjectId = Guid.NewGuid().ToString();
                projectToAdd.ProjectName = project.ProjectName;
                projectToAdd.Description = project.Description;
                _context.Project.Add(projectToAdd);
                _context.SaveChanges();
                return RedirectToAction("ViewProjects");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while adding the project: " + ex.Message);
                return View(project);
            }
                       // return View(project);
        }

        [HttpGet]

        public IActionResult EditProject(string id)
        {
            try
            {
                var project = _context.Project.Find(id);
                return View(project);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the Edit Project page: " + ex.Message);
                return View(new Projects());
            }
        }

        // POST: SuperAdmin/EditProject/5
        [HttpPost]
        public IActionResult EditProject(Projects project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Project.Update(project);
                    _context.SaveChanges();
                    return RedirectToAction("ViewProjects");
                }
                return View(project);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the project: " + ex.Message);
                return View(project);
            }
        }

        [HttpGet]
        public IActionResult DeleteProject(string id)
        {
            try
            {
                var project = _context.Project.Find(id);
                _context.Project.Remove(project);
                _context.SaveChanges();
                return RedirectToAction("ViewProjects");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the project: " + ex.Message);
                return RedirectToAction("ViewProjects");
            }
        }
        #endregion
        #region Activities
        [HttpGet]
        public IActionResult ViewActivities()
        {
            try
            {
                var activities = _context.Activities.Include(a => a.Project).ToList();
                return View(activities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while listing the activities.",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult AddActivity()
        {
            try
            {
                var ProjectList = _context.Project.Select(r => new SelectListItem
                {
                    Value = r.ProjectId.ToString(),
                    Text = r.ProjectName
                }).ToList();
                ViewBag.Project = ProjectList;
                return View();
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the Add Activity page: " + ex.Message);
                return View(new ActivityTasks());
            }
            //ViewBag.Project = new SelectList(_context.Project, "ProjectId", "ProjectName");
            //return View();
        }

        [HttpPost]
        public IActionResult AddActivity(ActivityTasks activity)
        {
            try
            {
                _context.Activities.Add(activity);
                _context.SaveChanges();
                return RedirectToAction("ViewActivities");
            }
               catch(Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while adding the activity: " + ex.Message);
                return View(activity);
            }

            //ViewBag.Project = new SelectList(_context.Project, "ProjectId", "ProjectName");
            //return View(activity);
        }

        [HttpGet]
        public IActionResult EditActivity(int id)
        {
            try
            {
                if (id != null && id != 0)
                {
                    ActivityTasks activityToBind = _context.Activities.Find(id);
                    if (activityToBind != null)
                    {

                        var ProjectList = _context.Project.Select(r => new SelectListItem
                        {
                            Value = r.ProjectId.ToString(),
                            Text = r.ProjectName
                        }).ToList();
                        ViewBag.Project = ProjectList;
                        return View(activityToBind);
                    }
                }
                return View();
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the Edit Activity page: " + ex.Message);
                return View(new ActivityTasks());
            }
            //var activity = _context.Activities.Find(id);
            //ViewBag.Project = new SelectList(_context.Project, "ProjectId", "ProjectName", activity.Project);

        }

        [HttpPost]
        public IActionResult EditActivity(int id, ActivityTasks activity)
        {
            try
            {
                if (activity == null)
                {
                    TempData["ErrorMessage"] = "Invalid Activity details.";
                    return RedirectToAction("Index"); // Redirect to department list
                }
                ActivityTasks activityToUpdate = _context.Activities.Find(id);
                if (activityToUpdate != null)
                {
                    activityToUpdate.ActivityName = activity.ActivityName;
                    activityToUpdate.Description = activity.Description;
                    activityToUpdate.Project = activity.Project;

                    _context.Update(activityToUpdate);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Activities updated successfully!";
                    return RedirectToAction("ViewActivities"); // Redirect after update
                }

                TempData["ErrorMessage"] = "Activities not found.";
                return RedirectToAction("ViewActivities");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the activity: " + ex.Message);
                return View(activity);
            }

            //_context.Activities.Update(activity);
            //    _context.SaveChanges();
            // return RedirectToAction("Activities");

            //ViewBag.Project = new SelectList(_context.Project, "ProjectId", "ProjectName", activity.Project);
        }

        [HttpGet]
        public IActionResult DeleteActivity(int id)
        {
            try
            {
                var activity = _context.Activities.Find(id);
                _context.Activities.Remove(activity);
                _context.SaveChanges();
                return RedirectToAction("ViewActivities");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the activity: " + ex.Message);
                return RedirectToAction("ViewActivities");
            }
        }

        #endregion

        #region ClientLeavePolicy
        [HttpGet]
        public async Task<IActionResult> ManageLeavePolicy()
        {
            try
            {
                var clients = await _userManager.GetUsersInRoleAsync("Client");

                //var clients = _context.Users.Where(u => u.UserName == "Client").ToList();
                ViewBag.ClientList = new SelectList(clients, "Id", "UserName");
                return View();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the ManageLeavePolicy page: " + ex.Message);
                return View();
            }

        }

        [HttpPost]
        public IActionResult ManageLeavePolicy(ClientLeavePolicy policy)
        {
            //if (ModelState.IsValid)
            //{
            try
            {
                //var clients = _context.Users.Where(u => u.UserName == "Client").ToList();
                //ViewBag.ClientList = new SelectList(clients, "Id", "UserName");

                _context.ClientLeavePolicies.Add(policy);
                _context.SaveChanges();
                TempData["Message"] = "Leave policy added successfully.";
                return RedirectToAction("Index");


               
                //return View(policy);
            }

            //}
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the ManageLeavePolicy page: " + ex.Message);
                return View();
            }
            
        }
        #endregion


        #region Payroll Process
       
        [HttpGet]
        public async Task<IActionResult> EnterOfficialDetails(string employeeId)
        
        {
            try
            {
                var model = new EmployeeOffDetailsViewModel
                {
                    Details = new EmployeeOffDetails { EmployeeId = employeeId },
                    Employees = (List<User>)await _userManager.GetUsersInRoleAsync("Employee"),
                    JobTypes = _context.JobTypes.ToList(),
                    Designations = _context.Designations.ToList(),
                    LeaveTypes = _context.LeaveTypes.ToList(),
                    AssetTypes = _context.AssetTypes.ToList(),
                    SalaryComponents = _context.SalaryComponents.ToList()
                };

                return View(model);
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the  Employee page: " + ex.Message);
                return View(new User());
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnterOfficialDetails(EmployeeOffDetails details)
        {

            try
            {
                //if (ModelState.IsValid)
                //{
                //    //var vm = new EmployeeOffDetailsViewModel
                //    //{
                //    //    Details = details,
                //    //    Employees = (List<User>)await _userManager.GetUsersInRoleAsync("Employee"),
                //    //    JobTypes = _context.JobTypes.ToList(),
                //    //    Designations = _context.Designations.ToList()
                //    //};

                //    //_context.EmployeeOffDetails.Add(details);
                //    //await _context.SaveChangesAsync();
                //    //return RedirectToAction("Index", "Home");
                //}
                
               // return View(vm);



                _context.EmployeeOffDetails.Add(details);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Employee official details saved.";

                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the EnterOfficialDetails page: " + ex.Message);
                return View();
            }
            // Reload dropdowns on error

        }

        #endregion

        public async Task<List<SelectListItem>> getUserByRoles(string roleName)
        {
            try
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

            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while retrieving users by role: " + ex.Message);
                return new List<SelectListItem>();
            }
        }

        private async Task<List<User>> GetUsersInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.Where(x => x.EmailConfirmed == true).ToList();
        }
        public IActionResult Index()
        {
            return View();
        }
      
    }
}
