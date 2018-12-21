using System.Collections.Generic;
using System.Linq;
using System.Net;
using Foundation;

namespace ModernHttpClient
{
    public class NativeCookieHandler
    {
        public void SetCookies(IEnumerable<Cookie> cookies)
        {
            foreach (var v in cookies.Select(ToNativeCookie)) {
                NSHttpCookieStorage.SharedStorage.SetCookie(v);
            }
        }

        public void DeleteCookies()
        {
            var CookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach (var cookie in CookieStorage.Cookies)
                CookieStorage.DeleteCookie(cookie);
        }

        public void SetCookie(Cookie cookie)
        {
            var nc = ToNativeCookie(cookie);
            NSHttpCookieStorage.SharedStorage.SetCookie(nc);
        }

        public void DeleteCookie(Cookie cookie)
        {
            var nc = FindNativeCookie(cookie);
            if (nc != null)
            {
                NSHttpCookieStorage.SharedStorage.DeleteCookie(nc);
            }
        }

        public List<Cookie> Cookies
        {
            get
            {
                return NSHttpCookieStorage.SharedStorage.Cookies
                    .Select(ToNetCookie)
                    .ToList();
            }
        }

        static NSHttpCookie FindNativeCookie(Cookie cookie)
        {
            return NSHttpCookieStorage.SharedStorage.Cookies.FirstOrDefault(c => c.Name.Equals(cookie.Name) && c.Value.Equals(cookie.Value) && c.Path.Equals(cookie.Path) && c.Domain.Equals(cookie.Domain));
        }

        static NSHttpCookie ToNativeCookie(Cookie cookie)
        {
            return new NSHttpCookie(cookie);
        }

        static Cookie ToNetCookie(NSHttpCookie cookie)
        {
            var nc = new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain);
            nc.Secure = cookie.IsSecure;

            return nc;
        }
    }
}
