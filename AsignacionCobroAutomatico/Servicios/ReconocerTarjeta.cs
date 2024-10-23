using System.Text.RegularExpressions;

namespace AsignacionCobroAutomatico.Servicios
{
    public class ReconocerTarjeta
    {
        public static string BuscarEmpresa(string NoTarjeta) 
        {
            string empresa = "";
            var Amex = new Regex(@"^3[47][0-9]{13}$");
            var Visa = new Regex(@"^4[0-9]{14}");
            var Masterd = new Regex(@"^5[15][0-9]{13}$");
            var Discover = new Regex(@"^(6011|644|65)[0-9]{12})");
            if (Amex.IsMatch(NoTarjeta))
            {
                empresa = "AMEX";
            } else if (Visa.IsMatch(NoTarjeta))
            {
                empresa = "VISA";
            } else if (Masterd.IsMatch(NoTarjeta))
            {
                empresa = "MASTERCARD";
            } else if (Discover.IsMatch(NoTarjeta)) 
            {
                empresa = "DISCOVER";
            }

            return empresa;
        }
    }
}
