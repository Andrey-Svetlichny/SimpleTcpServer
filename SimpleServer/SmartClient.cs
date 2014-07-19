using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using SimpleServerProto;

namespace SimpleServer
{
    /// <summary>
    /// Сообразительный клиент, ститающий свои деньги и печеньки.
    /// </summary>
    public class SmartClient
    {
        /// <summary>
        /// Печеньки одного типа
        /// </summary>
        [DebuggerDisplay("CookName = {CookName}; Quantity = {Quantity}")]
        class CookStore : IEquatable<CookStore>
        {
            public string CookName { get; set; }
            public uint Quantity { get; set; }
            public bool Equals(CookStore other)
            {
                throw new NotImplementedException();
            }
        }


        private readonly string _userId;
        private readonly string _password;
        private readonly int _dealDelay;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public IServer Server { get; set; }

        public SmartClient(string userId, string password, int dealDelay)
        {
            _userId = userId;
            _password = password;
            _dealDelay = dealDelay;
        }


        /// <summary>
        /// Покупка печенек в случайном порядке на все деньги.
        /// </summary>
        public void Execute()
        {
            Log.InfoFormat("'{0}' Execute", _userId);
            int dealCount = 0;
            var random = new Random();

            var requestAuthorization = new RequestAuthorization { UserId = _userId, Password = _password };
            var responseAuthorization = Server.Authorization(requestAuthorization);
            var responseCookieList = Server.CookieList();

            // наличные деньги
            var money = responseAuthorization.Money;
            // купленные печеньки
            var cookies = GetCookStores(responseAuthorization);              
            // печеньки на сервере
            var cookiesOnServer = responseCookieList.Cookies;

            // покупаем все, что можем, по одной печеньке
            while (true)
            {
                // печеньки, которые можно купить
                var availableCookies = cookiesOnServer.Where(o => o.Price <= money).ToList();
                
                if (availableCookies.Count == 0)
                {
                    // ничего нельзя купить
                    break;
                }

                // покупаем случайную печеньку
                var cookieToBuy = availableCookies[random.Next(availableCookies.Count)];
                uint quantity = 1;
                var requestBuyCookie = new RequestBuyCookie { Cookie = cookieToBuy, Quantity = quantity };
                var responseBuyCookie = Server.BuyCookie(requestBuyCookie);
                Log.DebugFormat("{0}: {1}", _userId, requestBuyCookie.Dump());
                Log.Debug(responseBuyCookie.Dump());

                // обновляем список купленных печенек и деньги
                money -= cookieToBuy.Price;
                if (cookies.All(o => o.CookName != cookieToBuy.Name))
                {
                    cookies.Add(new CookStore{CookName = cookieToBuy.Name, Quantity = 0});
                }
                cookies.Single(o => o.CookName == cookieToBuy.Name).Quantity += quantity;
                dealCount++;
                Thread.Sleep(_dealDelay);
            }

            // проверяем оставшиеся деньги и купленные печеньки
            var requestAuthorization2 = new RequestAuthorization { UserId = _userId, Password = _password };
            var responseAuthorization2 = Server.Authorization(requestAuthorization2);
            bool ok = true;
            if (money != responseAuthorization2.Money)
            {
                ok = false;
                Log.Error("Wrong money");
            }

            // сравниваем список купленных печенек, полученный с сервера с локальным
            var cookiesResp = GetCookStores(responseAuthorization2).OrderBy(o => o.CookName).ToList();
            cookies = cookies.OrderBy(o => o.CookName).ToList();
            if (cookies.Count != cookiesResp.Count)
            {
                ok = false;
                Log.Error("Wrong cookies list");
            }
            else
            {
                for (int i = 0; i < cookies.Count; i++)
                {
                    var c1 = cookiesResp[i];
                    var c2 = cookies[i];
                    if (c1.CookName != c2.CookName || c1.Quantity != c2.Quantity)
                    {
                        ok = false;
                        Log.Error("Wrong cookies list");
                        break;
                    }
                }
            }


            if (ok)
            {
                Log.InfoFormat("'{0}' - DealCount = {1}, OK", _userId, dealCount);
            }
        }


        /// <summary>
        /// Получает список купленных печенек.
        /// </summary>
        List<CookStore> GetCookStores(ResponseAuthorization responseAuthorization)
        {
            if (responseAuthorization.Cookies == null)
            {
                return new List<CookStore>();
            }
            return responseAuthorization.Cookies.Select(o => new CookStore { CookName = o.Key.Name, Quantity = o.Value }).ToList();
        }

    }
}
