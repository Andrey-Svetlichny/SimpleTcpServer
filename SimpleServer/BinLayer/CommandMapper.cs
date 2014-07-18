using System;
using System.Collections.Generic;
using System.Linq;
using SimpleServerProto;

namespace SimpleServer.BinLayer
{
    /// <summary>
    /// Маппинг команд на int.
    /// </summary>
    static class CommandMapper
    {
        private static readonly IDictionary<int, Type> TypeLookup = new Dictionary<int, Type>
        {
            {1, typeof(RequestAuthorization)}, {2, typeof(RequestCookieList)}, {3, typeof(RequestBuyCookie)}
        };

        public static int Map(Object obj)
        {
            Type type = obj.GetType();
            return TypeLookup.Single(pair => pair.Value == type).Key;
        }
        public static Type Map(int val)
        {
            return TypeLookup[val];
        }
    }
}