using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using MvcNetCoreLinqToSqlInjection.Models;
using System.Data;

#region Stored procedures

//create procedure SP_DELETE_DOCTOR
//(@iddoctor int)
//as
//	delete from DOCTOR where DOCTOR_NO=@iddoctor
//go

#endregion

namespace MvcNetCoreLinqToSqlInjection.Repositories
{
    public class RepositoryDoctoresSQLServer : IRepositoryDoctores
    {
        private SqlConnection cn;
        private SqlCommand com;
        private DataTable tablaDoctor;
        public RepositoryDoctoresSQLServer()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;
            string sql = "select * from DOCTOR";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            this.tablaDoctor = new DataTable();
            ad.Fill(this.tablaDoctor);
        }

        public List<Doctor> GetDoctores()
        {
            var consulta = from datos in this.tablaDoctor.AsEnumerable() select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor
                {
                    IdDoctor = row.Field<int>("DOCTOR_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Especialidad = row.Field<string>("ESPECIALIDAD"),
                    Salario = row.Field<int>("SALARIO"),
                    IdHospital = row.Field<int>("HOSPITAL_COD"),
                };
                doctores.Add(doc);
            }
            return doctores;
        }

        public async Task CreateDoctorAsync(int idHospital, int idDoctor, string apellido, string especialidad, int salario)
        {
            string sql = "insert into DOCTOR values (@idHospital, @idDoctor, @apellido, @especialidad, @salario)";
            this.com.Parameters.AddWithValue("@idHospital", idHospital);
            this.com.Parameters.AddWithValue("@idDoctor", idDoctor);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task DeleteDoctorAsync(int idDoctor)
        {
            string sql = "SP_DELETE_DOCTOR";
            this.com.Parameters.AddWithValue("@idDoctor", idDoctor);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task EditDoctorAsync(int idHospital, int idDoctor, string apellido, string especialidad, int salario)
        {
            string sql = "SP_UPDATE_DOCTOR";
            this.com.Parameters.AddWithValue("@idHospital", idHospital);
            this.com.Parameters.AddWithValue("@idDoctor", idDoctor);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;

            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public Doctor FindDoctor(int idDoctor)
        {
            var consulta = from datos in this.tablaDoctor.AsEnumerable() where (int)datos["DOCTOR_NO"] == idDoctor select datos;

            Doctor doc = new Doctor
            {
                IdDoctor = (int)consulta.FirstOrDefault()["DOCTOR_NO"],
                Apellido = consulta.FirstOrDefault()["APELLIDO"].ToString(),
                Especialidad = consulta.FirstOrDefault()["ESPECIALIDAD"].ToString(),
                Salario = (int)consulta.FirstOrDefault()["SALARIO"],
                IdHospital = (int)consulta.FirstOrDefault()["HOSPITAL_COD"],
            };
            return doc;
        }
    }
}
