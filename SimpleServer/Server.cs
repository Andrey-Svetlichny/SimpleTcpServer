using SimpleServer.Model;
using SimpleServerProto;

namespace SimpleServer
{
    public interface IServer
    {
        ResponseAuthorization Authorization(RequestAuthorization requestAuthorization);
        ResponseCookieList CookieList();
        ResponseBuyCookie BuyCookie(RequestBuyCookie requestBuyCookie);
    }

    /// <summary>
    /// Сервер раздачи печенек. Бизнес-логика.
    /// </summary>
    public class Server : IServer
    {
        readonly DbMock _db = new DbMock();
        private User _user;

        /// <summary>
        /// Если в базе есть указанный логин, проверяется правильность связки логин/пароль. 
        /// Если всё правильно, берётся информация из базы. 
        /// Если такого логина в базе нет - регистрируется новый (с указанными в команде параметрами).
        /// </summary>
        /// <returns>Ошибка или сколько у юзера денег и какие печеньки он уже купил ранее.</returns>
        public ResponseAuthorization Authorization(RequestAuthorization requestAuthorization)
        {
            var user = _db.GetUser(requestAuthorization.UserId);

            // пользователь есть в базе, но пароль другой
            if (user != null && user.Password != requestAuthorization.Password)
            {
                return new ResponseAuthorization { Error = true, ErrorMessage = "Wrong passord" };
            }

            // пользователя нет в базе, создаем
            if (user == null)
            {
                _db.AddUser(requestAuthorization.UserId, requestAuthorization.Password);
                user = _db.GetUser(requestAuthorization.UserId);
            }

            // сохраняем в сессии
            _user = user;
            return new ResponseAuthorization { Error = false, Money = user.Money, Cookies = user.Cookies };
        }

        /// <summary>
        /// Список имеющихся на сервере печенек с указанием их стоимости.
        /// </summary>
        public ResponseCookieList CookieList()
        {
            return new ResponseCookieList { Cookies = _db.GetCookieList() };
        }

        /// <summary>
        /// Покупка печенек.
        /// </summary>
        public ResponseBuyCookie BuyCookie(RequestBuyCookie requestBuyCookie)
        {
            // проверяем авторизацию
            if (_user == null)
            {
                return new ResponseBuyCookie { Error = true, ErrorMessage = "Сначала надо авторизоваться" };
            }

            // печенька из базы
            var cookie = _db.GetCookie(requestBuyCookie.Cookie.Name);

            // На сервере не таких печенек
            if (cookie == null)
            {
                return new ResponseBuyCookie { Error = true, ErrorMessage = "На сервере не таких печенек" };
            }

            // в задании не сказано, по какой цене отпускать печеньки, если на сервере и клиенте цена разная
            // на всякий случай пресекаем сделку.
            if (cookie.Price != requestBuyCookie.Cookie.Price)
            {
                return new ResponseBuyCookie { Error = true, ErrorMessage = "Цена не верна. Инфляция батенька!" };
            }

            // у пользователя мало денег
            if (_user.Money < cookie.Price * requestBuyCookie.Quantity)
            {
                return new ResponseBuyCookie { Error = true, ErrorMessage = "Не хватает денег" };
            }

            // покупка            
            _user.Money -= cookie.Price * requestBuyCookie.Quantity;
            if (!_user.Cookies.ContainsKey(cookie))
            {
                _user.Cookies.Add(cookie, 0);
            }
            _user.Cookies[cookie] += requestBuyCookie.Quantity;

            // в ответ посылаем количество купленных печенек
            return new ResponseBuyCookie { Error = false, Cookie = requestBuyCookie.Cookie, Quantity = requestBuyCookie.Quantity };
        }

        /// <summary>
        /// Close session. Reset server.
        /// </summary>
        public void Reset()
        {
            _user = null;
        }
    }
}