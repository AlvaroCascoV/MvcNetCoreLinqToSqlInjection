using Microsoft.AspNetCore.Mvc;
using MvcNetCoreLinqToSqlInjection.Models;
using MvcNetCoreLinqToSqlInjection.Repositories;
using System.Threading.Tasks;

namespace MvcNetCoreLinqToSqlInjection.Controllers
{
    public class DoctoresController : Controller
    {
        //private RepositoryDoctoresSQLServer repo;
        //private RepositoryDoctoresOracle repo;
        IRepositoryDoctores repo;

        //RECIBIMOS NUESTRO REPOSITORY
        public DoctoresController(/*RepositoryDoctoresSQLServer RepositoryDoctoresOracle*/ IRepositoryDoctores repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Doctor> doctores = this.repo.GetDoctores();
            return View(doctores);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Doctor doc)
        {
            await this.repo.CreateDoctorAsync(doc.IdHospital, doc.IdDoctor, doc.Apellido, doc.Especialidad, doc.Salario);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int idDoctor)
        {
            await this.repo.DeleteDoctorAsync(idDoctor);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int idDoctor)
        {
            Doctor doctor = this.repo.FindDoctor(idDoctor);
            return View(doctor);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Doctor doc)
        {
            await this.repo.EditDoctorAsync(doc.IdHospital, doc.IdDoctor, doc.Apellido, doc.Especialidad, doc.Salario);
            return RedirectToAction("Index");
        }
    }
}
