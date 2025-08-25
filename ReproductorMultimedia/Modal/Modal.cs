namespace ReproductorMultimedia.Modal
{
    public class Modal
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string Tipo { get; set; } // El tipo puede ser "info", "warning", "error", etc.
        public string BotonTexto { get; set; } = "Aceptar";
        public bool MostrarCancelar { get; set; } = false;
    }
}
