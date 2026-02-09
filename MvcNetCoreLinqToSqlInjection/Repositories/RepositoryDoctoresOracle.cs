using Microsoft.AspNetCore.Http.HttpResults;
using MvcNetCoreLinqToSqlInjection.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static Azure.Core.HttpHeader;

#region Stored procedures

//create or REPLACE PROCEDURE SP_DELETE_DOCTOR 
//(p_iddoctor DOCTOR.DOCTOR_NO%type)
//as
//begin
//    delete from DOCTOR where DOCTOR_NO=p_iddoctor;
//commit;
//end;

#endregion

namespace MvcNetCoreLinqToSqlInjection.Repositories
{
    public class RepositoryDoctoresOracle : IRepositoryDoctores
    {
        private DataTable tablaDoctor;
        private OracleConnection cn;
        private OracleCommand com;

        public RepositoryDoctoresOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/FREEPDB1;Persist Security Info=true;User Id=SYSTEM;Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = cn;
            this.tablaDoctor = new DataTable();
            string sql = "select * from DOCTOR";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
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
            //los parametros no son con "@", es con ":"
            string sql = "insert into DOCTOR values (:idHospital, :idDoctor, :apellido, :especialidad, :salario)";
            //AQUI VAN LOS PARAMETROS... tienen que ser en orden
            OracleParameter pamIdHospital = new OracleParameter(":idHospital", idHospital);
            OracleParameter pamIdDoctor = new OracleParameter(":idDoctor", idDoctor);
            OracleParameter pamApellido = new OracleParameter(":apellido", apellido);
            OracleParameter pamEspecialidad = new OracleParameter(":especialidad", especialidad);
            OracleParameter pamSalario = new OracleParameter(":salario", salario);
            this.com.Parameters.Add(pamIdHospital);
            this.com.Parameters.Add(pamIdDoctor);
            this.com.Parameters.Add(pamApellido);
            this.com.Parameters.Add(pamEspecialidad);
            this.com.Parameters.Add(pamSalario);

            //para oracle no funciona AddWithValue
            //this.com.Parameters.AddWithValue("@idHospital", idHospital);
            //this.com.Parameters.AddWithValue("@idDoctor", idDoctor);
            //this.com.Parameters.AddWithValue("@apellido", apellido);
            //this.com.Parameters.AddWithValue("@especialidad", especialidad);
            //this.com.Parameters.AddWithValue("@salario", salario);

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
            OracleParameter pamIdDoctor = new OracleParameter(":p_iddoctor", idDoctor);
            this.com.Parameters.Add(pamIdDoctor);
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

        public async Task EditDoctorAsync(int idHospital, int idDoctor, string apellido, string especialidad, int salario)
        {
            string sql = "SP_UPDATE_DOCTOR";

            OracleParameter pamIdHospital = new OracleParameter(":idHospital", idHospital);
            OracleParameter pamIdDoctor = new OracleParameter(":idDoctor", idDoctor);
            OracleParameter pamApellido = new OracleParameter(":apellido", apellido);
            OracleParameter pamEspecialidad = new OracleParameter(":especialidad", especialidad);
            OracleParameter pamSalario = new OracleParameter(":salario", salario);
            this.com.Parameters.Add(pamIdHospital);
            this.com.Parameters.Add(pamIdDoctor);
            this.com.Parameters.Add(pamApellido);
            this.com.Parameters.Add(pamEspecialidad);
            this.com.Parameters.Add(pamSalario);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;

            await this.cn.OpenAsync();

            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    }
}
