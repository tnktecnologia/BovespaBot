using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScrapperTest
{
    public static class ToCaptchaClient
    {
        private static string APIKey = "86d58b229e242a9de2cb8dffb67792a6";

        public static IToCaptchaResponse In(string base64)
        {


            using (var hc = new HttpClient())
            {
                var result = hc.PostAsync("http://2captcha.com/in.php",
                    new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("method", "base64"),
                        new KeyValuePair<string, string>("key", APIKey),
                        new KeyValuePair<string, string>("body", base64),
                    })).Result;

                var resposta = result.Content.ReadAsStringAsync().Result;

                return ToCaptchaResponse.From(resposta);
            }
        }

        public static IToCaptchaResponse Res(string captchaId)
        {
            using (var hc = new HttpClient())
            {
                var result = hc.GetAsync($"http://2captcha.com/res.php?key={APIKey}&action=get&id={captchaId}").Result;

                var resposta = result.Content.ReadAsStringAsync().Result;

                return ToCaptchaResponse.From(resposta);
            }
        }

        public static IToCaptchaResponse InRes(string base64, TimeSpan initialWait, int tries = 10)
        {
            var inRes = In(base64);
            if (inRes.Status != ToCaptchaStatus.OK)
            {
                return inRes;
            }

            var slTime = (int)initialWait.TotalMilliseconds;

            for (int i = 0; i < tries; i++)
            {
                Thread.Sleep(slTime);
                var res = Res(inRes.Content);

                if (res.Status == ToCaptchaStatus.CAPCHA_NOT_READY)
                {
                    slTime = 6000;
                }
                else
                {
                    return res;
                }
            }

            return new ToCaptchaResponse(ToCaptchaStatus.TIMEOUT, null);
        }

        private class ToCaptchaResponse : IToCaptchaResponse
        {
            public ToCaptchaStatus Status { get; set; }
            public string Content { get; set; }

            public ToCaptchaResponse(ToCaptchaStatus status, string content)
            {
                Status = status;
                Content = content;
            }

            public static ToCaptchaResponse From(string result)
            {
                string content = null;

                result = result ?? "";

                if (result.StartsWith("OK") && result.Contains("|"))
                {
                    var split = result.Split('|');
                    if (split.Length > 1)
                    {
                        content = split[1];
                    }

                    return new ToCaptchaResponse(ToCaptchaStatus.OK, content);
                }


                ToCaptchaStatus status;
                switch ((result ?? "").ToUpper())
                {
                    case "":
                        status = ToCaptchaStatus.EMPTY;
                        break;
                    case "CAPCHA_NOT_READY":
                        status = ToCaptchaStatus.CAPCHA_NOT_READY;
                        break;
                    default:
                        status = ToCaptchaStatus.ERROR;
                        break;
                }

                return new ToCaptchaResponse(status, null);
            }
        }

    }


    public enum ProxyType
    {
        HTTP,
        HTTPS,
        SOCKS4,
        SOCKS5
    }

    public interface IToCaptchaResponse
    {
        ToCaptchaStatus Status { get; }
        string Content { get; }
    }

    public enum ToCaptchaStatus
    {
        OK,
        ERROR_WRONG_USER_KEY,
        ERROR_KEY_DOES_NOT_EXIST,
        ERROR_ZERO_BALANCE,
        ERROR_NO_SLOT_AVAILABLE,
        ERROR_ZERO_CAPTCHA_FILESIZE,
        ERROR_TOO_BIG_CAPTCHA_FILESIZE,
        ERROR_WRONG_FILE_EXTENSION,
        ERROR_IMAGE_TYPE_NOT_SUPPORTED,
        ERROR_IP_NOT_ALLOWED,
        ERROR_CAPTCHAIMAGE_BLOCKED,
        ERROR_WRONG_ID_FORMAT,
        ERROR_CAPTCHA_UNSOLVABLE,
        ERROR_WRONG_CAPTCHA_ID,
        ERROR_BAD_DUPLICATES,
        REPORT_NOT_RECORDED,
        IP_BANNED,
        CAPCHA_NOT_READY,
        ERROR,
        EMPTY,
        TIMEOUT
    }

}
