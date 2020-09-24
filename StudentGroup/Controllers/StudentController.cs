using System.Linq;
using System.Web.Mvc;
using StudentGroup.Models;
using System.Data.Entity;
using System.Net;

namespace StudentGroup.Controllers
{
    public class StudentController : Controller
    {
        StudentContext db = new StudentContext();

        public ActionResult Index()
        {
            var courses = db.Courses;
            return View(courses.ToList());
        }

        public ActionResult StudentList(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var students = db.Students.Where(s => s.GroupId == id).Include(s => s.Group);

            if (students == null || students.Count() <= 0)
            {
                return HttpNotFound();
            }

            return View(students);
        }

        public ActionResult GroupsList(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var groups = db.Groups.Where(c => c.Course.Id == id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        [HttpGet]
        public ActionResult CreateStudent()
        {
            SelectList groups = new SelectList(db.Groups, "Id", "Name");
            ViewBag.Groups = groups;
            return View();
        }

        [HttpPost]
        public ActionResult CreateStudent(Student student)
        {

            db.Students.Add(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateGroup()
        {
            SelectList courses = new SelectList(db.Courses, "Id", "Name");
            ViewBag.Courses = courses;
            return View();
        }

        [HttpPost]
        public ActionResult CreateGroup(Group group)
        {

            db.Groups.Add(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost]
        public ActionResult EditGroup([Bind(Include = "Id,CourseId,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        public ActionResult EditStudent(int? id)
        {
            SelectList groups = new SelectList(db.Groups, "Id", "Name");
            ViewBag.Groups = groups;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost]
        public ActionResult EditStudent([Bind(Include = "Id,FirstName,LastName,GroupId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public ActionResult DeleteStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Where(s => s.Id == id).Include(s => s.Group).FirstOrDefault();
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("DeleteStudent")]
        public ActionResult DeleteStudentConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups
                .Where(g => g.Id == id)
                .Include(g => g.Course)
                .Include(g => g.Students)
                .FirstOrDefault();
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost, ActionName("DeleteGroup")]
        public ActionResult DeleteGroupConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}