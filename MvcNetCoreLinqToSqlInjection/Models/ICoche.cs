namespace MvcNetCoreLinqToSqlInjection.Models
{
    public interface ICoche
    {
        //LA INTERFACE NO TIENE AMBITO DE METODO
        //NI LLEVA PUBLIC, PRIVATE, NI NADA, SOLO LA DECLARACION
        string Marca { get; set; }
        string Modelo { get; set; }
        string Imagen { get; set; }
        int Velocidad { get; set; }
        int VelocidadMaxima { get; set; }
        void Acelerar();
        void Frenar();
    }
}
