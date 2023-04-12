using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

namespace Editor
{
    [TestFixture]
    public class 网络请求
    {
        [UnityTest]
        public IEnumerator testBaidu()
        {
            //isHttpError和isNetworkError是不鼓励使用了，参考下一用例
            var request = UnityWebRequest.Get("http://www.baidu.com");
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }

        [UnityTest]
        public IEnumerator testBaidu2()
        {
            //isHttpError和isNetworkError是不鼓励使用了，参考下一用例
            var request = UnityWebRequest.Get("http://www.baidu.com");
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }


        //显示进度条下载
        [UnityTest]
        public IEnumerator GetShowProgress()
        {
            UnityWebRequest request = UnityWebRequest.Get("www.baidu.com");
            request.SendWebRequest();
            while (!request.isDone)
            {
                Debug.Log($"{GetType()} progress:{request.downloadProgress}");
                yield return null;
            }

            Debug.Log($"{GetType()} progress:{request.downloadProgress}");

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError($"{GetType()} error:{request.error}");
            }
            else
            {
                Debug.Log($"{GetType()} text:{request.downloadHandler.text}");
                Debug.Log($"{GetType()} bytes.length:{request.downloadHandler.data.Length}");
            }
        }
        class BypassCertificate : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }
        //http请求某个https链接并使用正则表达式解析内容

        [UnityTest]
        public IEnumerator getVideoLink()
        {
            var videoId = "3472738";
            UnityWebRequest req = new UnityWebRequest($"https://www.gexo.com/a/{videoId}");
            //绕过SSL证书验证
            req.certificateHandler = new BypassCertificate();
            req.downloadHandler = new DownloadHandlerBuffer();
            var x = req.SendWebRequest();
            yield return x;
            Debug.Log($"请求结束 {x.isDone} req={req}");
            Debug.Log($@"result==success:{req.result == UnityWebRequest.Result.Success}
error={req.error}
method={req.method}
url={req.url}
result={req.result}
responseCode={req.responseCode}
downLoaderHandler={req.downloadHandler}
downloadBytes={req.downloadedBytes}
toString={req.ToString()}
downloadHandler==null:{req.downloadHandler == null}
");
            if (req.downloadHandler != null)
            {
                Debug.Log($"content={req.downloadHandler.text}");
                var s = req.downloadHandler.text;
                var a = Regex.Matches(s, "\"url\":\"(.+?)\"");
                for (int i = 0; i < a.Count; i++)
                {
                    var m = a[i];
                    var g = m.Groups[1];
                    var url = g.Value;
                    url = url.Replace("\\/", "/");
                    Debug.Log($"match:{url}");
                }
            }
        }

        /*
         * 网络请求获取远程Texture
         */
        public static async Task<Texture2D> GetRemoteTexture(string url)
        {
            using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
            {
                req.certificateHandler = new BypassCertificate();
                var asyncOp = req.SendWebRequest();

                // await until it's done:
                while (asyncOp.isDone == false)
                    await Task.Delay(1000 / 30); //30 hertz

                // read results:
                if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log($"{req.error}, URL:{req.url}");
                    return null;
                }

                return DownloadHandlerTexture.GetContent(req);
            }
        }
    }
}