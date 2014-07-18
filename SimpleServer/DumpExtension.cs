using System.Linq;
using SimpleServerProto;

namespace SimpleServer
{
    /// <summary>
    /// Расширения для логирования протокола SimpleServerProto.
    /// </summary>
    public static class DumpExtension
    {

        public static string Dump(this RequestAuthorization o)
        {
            return string.Format("RequestAuthorization: UserId = {0}; Password = {1}", o.UserId, o.Password);
        }
        public static string Dump(this ResponseAuthorization o)
        {
            var res = "ResponseAuthorization: ";
            if (o.Error)
            {
                res += string.Format("Error: {0}", o.ErrorMessage);
            }
            else
            {
                res += string.Format("OK\nMoney = " + o.Money);
                res += string.Format("\nCookies:");
                o.Cookies.Select(c => string.Format("\n\t{0}; Quantity = {1}", c.Key.Dump(), c.Value))
                    .ToList().ForEach(s => res += s);
            }
            return res;
        }
        public static string Dump(this ResponseCookieList o)
        {
            var res = "ResponseCookieList:";
            o.Cookies.Select(c => string.Format("\n\t{0}", c.Dump()))
                .ToList().ForEach(s => res += s);
            return res;
        }
        public static string Dump(this RequestBuyCookie o)
        {
            return string.Format("RequestBuyCookie: Cookie = {0}; Quantity = {1}", o.Cookie.Dump(), o.Quantity);
        }
        public static string Dump(this ResponseBuyCookie o)
        {
            var res = "ResponseBuyCookie: ";
            if (o.Error)
            {
                res += string.Format("Error: {0}", o.ErrorMessage);
            }
            else
            {
                res += string.Format("Cookie = {0}; Quantity = {1}", o.Cookie.Dump(), o.Quantity);
            }
            return res;
        }
        public static string Dump(this Cookie o)
        {
            return string.Format("Cookie: Name = {0}; Price = {1}", o.Name, o.Price);
        }
    }
}