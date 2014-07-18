using System.Reflection;
using log4net;
using SimpleServer;
using SimpleServerProto;

namespace SimpleServer
{
    /// <summary>
    /// Тестовый клиент. Для наглядного тестирования работы сервера.
    /// Выполняет операции:
    ///  - попытка авторизации с неправильным паролем
    ///  - авторизация существующим пользователем
    ///  - получение списка печенек
    ///  - несколько неправильных попыток купить печеньки
    ///  - правильная покупка
    ///  - еще одна авторизация для проверки того, что печеньки куплены.
    /// 
    /// Для тестирования бизнес-логики лучше испольовать юнит-тест. Но мы же пишем прототип
    /// </summary>
    public class Client
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public IServer Server { get; set; }
        public void Start()
        {
            // неправильный пароль
            var responseAuthorization = Server.Authorization(new RequestAuthorization { UserId = "user1", Password = "WRONG" });
            Log.Debug(responseAuthorization.Dump());

            // существующий user1
            var requestAuthorization = new RequestAuthorization { UserId = "user1", Password = "123" };
            responseAuthorization = Server.Authorization(requestAuthorization);
            Log.Debug(requestAuthorization.Dump());
            Log.Debug(responseAuthorization.Dump());

            // список печенек на сервере
            var responseCookieList = Server.CookieList();
            Log.Debug(responseCookieList.Dump());

            // покупка несуществущий печенек
            var requestBuyCookie = new RequestBuyCookie { Cookie = new Cookie { Name = "Пиво", Price = 10 }, Quantity = 20 };
            var responseBuyCookie = Server.BuyCookie(requestBuyCookie);
            Log.Debug(requestBuyCookie.Dump());
            Log.Debug(responseBuyCookie.Dump());

            // покупка печенек с неправильной ценой
            requestBuyCookie = new RequestBuyCookie { Cookie = new Cookie { Name = "Курабье", Price = 10 }, Quantity = 20 };
            responseBuyCookie = Server.BuyCookie(requestBuyCookie);
            Log.Debug(requestBuyCookie.Dump());
            Log.Debug(responseBuyCookie.Dump());

            // покупка печенек на слишком большую сумму
            requestBuyCookie = new RequestBuyCookie { Cookie = new Cookie { Name = "Курабье", Price = 35 }, Quantity = 20 };
            responseBuyCookie = Server.BuyCookie(requestBuyCookie);
            Log.Debug(requestBuyCookie.Dump());
            Log.Debug(responseBuyCookie.Dump());

            // покупка 3-х печенек
            requestBuyCookie = new RequestBuyCookie { Cookie = new Cookie { Name = "Курабье", Price = 35 }, Quantity = 3 };
            responseBuyCookie = Server.BuyCookie(requestBuyCookie);
            Log.Debug(requestBuyCookie.Dump());
            Log.Debug(responseBuyCookie.Dump());

            // еще одна авторизация того же пользователя
            responseAuthorization = Server.Authorization(requestAuthorization);
            Log.Debug(requestAuthorization.Dump());
            Log.Debug(responseAuthorization.Dump());
        }
    }
}