using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skyline.DbContexts;
using Skyline.Models;

namespace Skyline.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _IWebHostEnvironment;
        public EmployeesController(ApplicationDbContext context, IWebHostEnvironment iWebHostEnvironment)
        {
            _context = context;
            _IWebHostEnvironment = iWebHostEnvironment;
        }

        //List<Employee> employees = new List<Employee>()
        //{
        //    new Employee(){Id = 1001,FullName = "Ayman Mostafa Ali",NationalId = "11022033044",Position = "Tester", Salary = 9000m },
        //    new Employee(){Id = 1002,FullName = "Wael Mahmoud Omar",NationalId = "10020030055",Position = "Developer", Salary =14500m },
        //    new Employee(){Id = 1003,FullName = "Bahaa Ahmed Osama",NationalId = "10102020303066",Position = "Tech Lead", Salary = 19500m }
        //};
        public IActionResult GetIndexView(string? search)
        {
            ViewBag.EmpCountries = new List<string>() { "Egypt", "Sudan", "Kuwait", "Oman" };

            ViewBag.SearchText = search;

            IQueryable<Employee> queryableEmps = _context.Employees.AsQueryable();

            if (string.IsNullOrEmpty(search) == false)
            {
                queryableEmps = queryableEmps.Where(emp => emp.FullName.Contains(search) || emp.Position.Contains(search));
            }
            return View("Index", queryableEmps.ToList());
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}


        public IActionResult GetDetailsView(int id)
        {
            Employee employee = _context.Employees.Include(emp => emp.Departments).FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View("Details", employee);
        }

        public string GreetVisitor()
        {
            return "Welcome to Skyline";
        }
        public string CalculateAge(string name, int birthYear)
        {
            return $"Hi, {name}. You are {DateTime.Now.Year - birthYear} years old";
        }


        //http verbs:  HttpGet,HttpPost 
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddNew(Employee emp)
        {
            if (((emp.HiringDateAndTime - emp.BirthDate).Days / 365) < 18)
            {
                ModelState.AddModelError(string.Empty, "Not Allowed hiring age (Under 18 years old");
            }
            if (ModelState.IsValid)
            {
                if (emp.ImageFile == null)
                {
                    emp.ImagePath = "\\images\\No_Image.png";
                }
                else
                {
                    //GUID global unique identifier 
                    Guid imageGuid = Guid.NewGuid();
                    string imageExtension = Path.GetExtension(emp.ImageFile.FileName);

                    emp.ImagePath = "\\images\\" + imageGuid + imageExtension;
                    string imageUploadPath = _IWebHostEnvironment.WebRootPath + emp.ImagePath;

                    FileStream imageStream = new FileStream(imageUploadPath, FileMode.Create);
                    emp.ImageFile.CopyTo(imageStream);
                    imageStream.Dispose();

                }

                _context.Employees.Add(emp);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");

            }
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Create");
            }

        }

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Employee employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Edit", employee);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditCurrent(Employee emp)
        {
            if (((emp.HiringDateAndTime - emp.BirthDate).Days / 365) < 18)
            {
                ModelState.AddModelError(string.Empty, "Not Allowed hiring age (Under 18 years old");
            }
            if (ModelState.IsValid)
            {

                if (emp.ImageFile != null)
                {

                    if (emp.ImagePath != "\\images\\No_Image.png")
                    {
                        System.IO.File.Delete(_IWebHostEnvironment.WebRootPath + emp.ImagePath);
                    }

                    //GUID global unique identifier 
                    Guid imageGuid = Guid.NewGuid();
                    string imageExtension = Path.GetExtension(emp.ImageFile.FileName);

                    emp.ImagePath = "\\images\\" + imageGuid + imageExtension;
                    string imageUploadPath = _IWebHostEnvironment.WebRootPath + emp.ImagePath;

                    FileStream imageStream = new FileStream(imageUploadPath, FileMode.Create);
                    emp.ImageFile.CopyTo(imageStream);
                    imageStream.Dispose();

                }


                _context.Employees.Update(emp);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Edit");
            }

        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Employee employee = _context.Employees.Include(emp => emp.Departments).FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                return View("Delete", employee);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]

        public IActionResult DeleteCurrent(int id)
        {
            Employee employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                if (employee.ImagePath != "\\images\\No_Image.png")
                {
                    System.IO.File.Delete(_IWebHostEnvironment.WebRootPath + employee.ImagePath);
                }

                _context.Remove(employee);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
        }

        public IActionResult GetCreateView()
        {
            ViewBag.AllDepartments = _context.Departments.ToList();
            return View("Create");
        }


    }
}
