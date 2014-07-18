using System.Collections.Generic;
using System.Linq;
using SimpleServer.Model;
using SimpleServerProto;

namespace SimpleServer
{
    /// <summary>
    /// Тестовая база.
    /// </summary>
    class DbMock
    {
        private readonly Dictionary<string, User> _users;
        private readonly Dictionary<string, Cookie> _cookies;

        public DbMock()
        {
            _cookies = new List<Cookie>(new[]
            {
                new Cookie{Name = "Шоколадное", Price = 10},
                new Cookie{Name = "Овсяное", Price = 2},
                new Cookie{Name = "Курабье", Price = 35}
            }).ToDictionary(u => u.Name);

            _users = new List<User>(new[]
            {
                new User {UserId = "user1", Password = "123", Money = 500, Cookies = new Dictionary<Cookie, uint>()},
                new User {UserId = "user2", Password = "asdf", Money = 50, Cookies = new Dictionary<Cookie, uint>()}
            }).ToDictionary(u => u.UserId);

            _users["user1"].Cookies.Add(_cookies["Шоколадное"], 3);
            _users["user2"].Cookies.Add(_cookies["Курабье"], 7);

        }

        public User GetUser(string id)
        {
            if (_users.ContainsKey(id))
            {
                return _users[id];
            }
            return null;
        }

        public void AddUser(string id, string password)
        {
            var user = new User { UserId = id, Password = password, Cookies = new Dictionary<Cookie, uint>() };
            _users.Add(user.UserId, user);
        }

        public Cookie GetCookie(string id)
        {
            if (_cookies.ContainsKey(id))
            {
                return _cookies[id];
            }
            return null;
        }

        public List<Cookie> GetCookieList()
        {
            return _cookies.Values.ToList();
        }
    }
}