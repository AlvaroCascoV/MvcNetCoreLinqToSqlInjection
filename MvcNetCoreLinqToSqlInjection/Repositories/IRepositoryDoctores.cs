using MvcNetCoreLinqToSqlInjection.Models;

namespace MvcNetCoreLinqToSqlInjection.Repositories
{
    public interface IRepositoryDoctores
    {
        List<Doctor> GetDoctores();
        //tiene que coincidir nombre de metodo y nombe de variables
        Task CreateDoctorAsync(int idHospital, int idDoctor, string apellido, string especialidad, int salario);
        Task DeleteDoctorAsync(int idDoctor);
        Doctor FindDoctor(int idDoctor);
        Task EditDoctorAsync(int idHospital, int idDoctor, string apellido, string especialidad, int salario);
    }
}
