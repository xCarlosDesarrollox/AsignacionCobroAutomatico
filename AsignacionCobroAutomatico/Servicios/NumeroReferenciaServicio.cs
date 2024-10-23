namespace AsignacionCobroAutomatico.Servicios
{
    public class NumeroReferenciaServicio
    {
        private static readonly Dictionary<int, string> RepertorioServicio = new Dictionary<int, string>
        {
            { 1, "001"}, //Electricidad
            { 2, "002"}, //Telefono
            { 3, "003"}, //Agua
        };
        public static string AsignarNumeroReferencia(int servicioId) 
        {
            if (RepertorioServicio.TryGetValue(servicioId,out string numeroRef)) { return numeroRef; }
            return "000";
        }
    }
}
