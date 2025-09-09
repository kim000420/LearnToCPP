using System;
using System.Collections.Generic;

namespace LearnToDecorator
{
    // Q. 데코 패턴은 일반참조와 무엇이 다른가?
    // 문제
    // 1. 데코레이트 패턴은 언제 사용하는가?
    //  객체에 추가되는 요소가 여러가지일때 사용하면 좋을듯함
    //
    // 2. 데코레이트 패턴이 게임에 사용될 만한 예를 찾아보기
    //  장비or스킬에 인첸트하면 이름이 바뀌면서 추가효과가 달릴때
    //  
    //
    // ???? 
    
    /*
    class Program()
    {
        static void Main()
        {
            // 그냥 쿠키
            CookiesBase myCookie1 = new Cookie();
            myCookie1.PrintCookies();

            // 초코 쿠키
            CookiesBase myCookie2 = new Choco(new Cookie()) ;
            myCookie2.PrintCookies();

            // 초코 바나나 쿠키
            CookiesBase myCookie3 = new Choco(new Banana(new Cookie()));
            myCookie3.PrintCookies();
            
            // 얀데레 쿠키
            CookiesBase specialCookie = new Love(new Cookie()) ;
            specialCookie.PrintCookies();
        }
    }
    */

    // 쿠키 인터페이스
    public abstract class CookiesBase
    {
        // 쿠키 이름 재료 A,B
        protected string cookieName;
        protected string cookieResorceA;
        protected string cookieResorceB;

        // 출력용 쿠키 이름 얻기
        public abstract string GetCookieName();
        public abstract string GetCookieResorceA();
        public abstract string GetCookieResorceB();

        // 쿠키 이름, 재료 출력
        public abstract void PrintCookies();
    }

    public abstract class Decorator : CookiesBase
    {
        // 데코 객체 생성
        protected CookiesBase cookies;
    }

    // 쿠키 객체
    public class Cookie : CookiesBase
    {
        public Cookie()
        {
            this.cookieName = "쿠키";
            this.cookieResorceA = "설탕";
            this.cookieResorceB = "계란";
        }

        public override string GetCookieName()
        {
            return this.cookieName;
        }

        public override string GetCookieResorceA()
        {
            return this.cookieResorceA;
        }

        public override string GetCookieResorceB()
        {
            return this.cookieResorceB;
        }

        public override void PrintCookies()
        {
            Console.WriteLine("쿠키 이름 :" + this.cookieName);
            Console.WriteLine("쿠키 재료 :" + this.cookieResorceA + " " + this.cookieResorceB);
            Console.WriteLine("");
        }
    }

    // 초코 토핑
    public class Choco : Decorator
    {
        public Choco(CookiesBase _cookies)
        {
            this.cookies = _cookies;
            this.cookieName = "초코, " + _cookies.GetCookieName();
            this.cookieResorceA = "초코, " + _cookies.GetCookieResorceA();
        }
        public override string GetCookieName()
        {
            return this.cookieName;
        }

        public override string GetCookieResorceA()
        {
            return this.cookieResorceA;
        }

        public override string GetCookieResorceB()
        {
            return this.cookieResorceB;
        }

        public override void PrintCookies()
        { 
            Console.WriteLine("쿠키 이름 :" + this.cookieName);
            Console.WriteLine("쿠키 재료 :" + this.cookieResorceA + ", " + this.cookieResorceB);
            Console.WriteLine("");
        }
    }

    // 바나나 토핑
    public class Banana : Decorator
    {
        public Banana(CookiesBase _cookies)
        {
            this.cookies = _cookies;
            this.cookieName = "바나나, " + _cookies.GetCookieName();
            this.cookieResorceA = "바나나, " + _cookies.GetCookieResorceA();
        }
        public override string GetCookieName()
        {
            return this.cookieName;
        }

        public override string GetCookieResorceA()
        {
            return this.cookieResorceA;
        }

        public override string GetCookieResorceB()
        {
            return this.cookieResorceB;
        }

        public override void PrintCookies()
        {
            Console.WriteLine("쿠키 이름 :" + this.cookieName);
            Console.WriteLine("쿠키 재료 :" + this.cookieResorceA + ", " + this.cookieResorceB);
            Console.WriteLine("");
        }
    }

    // 사랑 가득 토핑
    public class Love: Decorator
    {
        public Love(CookiesBase _cookies)
        {
            this.cookies = _cookies;
            this.cookieName = "사랑 가득 " + _cookies.GetCookieName();
            this.cookieResorceA = "누군가의혈액, " + _cookies.GetCookieResorceA();
            this.cookieResorceB = "누군가의살점, " + _cookies.GetCookieResorceB();
        }
        public override string GetCookieName()
        {
            return this.cookieName;
        }

        public override string GetCookieResorceA()
        {
            return this.cookieResorceA;
        }

        public override string GetCookieResorceB()
        {
            return this.cookieResorceB;
        }

        public override void PrintCookies()
        {
            Console.WriteLine("쿠키 이름 :" + this.cookieName);
            Console.WriteLine("쿠키 재료 :" + this.cookieResorceA + " " + this.cookieResorceB);
            Console.WriteLine("");
        }
    }
}


