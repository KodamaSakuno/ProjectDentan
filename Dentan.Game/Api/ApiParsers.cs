using Moen.KanColle.Dentan.Proxy;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace Moen.KanColle.Dentan.Api
{
    public static class ApiParsers
    {
        static Dictionary<string, HashSet<ApiParserBase>> r_Parsers = new Dictionary<string, HashSet<ApiParserBase>>();
        static ActionBlock<ApiSession> r_Block;
        static ActionBlock<Tuple<ApiSession, Exception>> r_ExceptionBlock;

        public static event Action<Exception> NewException;

        static Lazy<Regex> r_TokenRegex = new Lazy<Regex>(() => new Regex(@"(?<=api_token=)\w+"));

        static ApiParsers()
        {
            var rCurrentAssembly = Assembly.GetExecutingAssembly();
            var rParserTypes = rCurrentAssembly.GetTypes().Where(r => !r.IsAbstract && r.IsSubclassOf(typeof(ApiParserBase)));
            foreach (var rType in rParserTypes)
            {
                var rParser = (ApiParserBase)Activator.CreateInstance(rType);

                var rAttributes = rType.GetCustomAttributes<ApiAttribute>();
                foreach (var rAttribute in rAttributes)
                {
                    var rPath = rAttribute.Path;

                    HashSet<ApiParserBase> rParsers;
                    if (!r_Parsers.TryGetValue(rPath, out rParsers))
                        r_Parsers.Add(rPath, new HashSet<ApiParserBase>() { rParser });
                    else
                        rParsers.Add(rParser);
                }
            }

            r_Block = new ActionBlock<ApiSession>(rpSession =>
            {
                var rStopwatch = Stopwatch.StartNew();

                /*
                if (Debugger.IsAttached)
                    ProcessSessionCore(rpSession);
                /*/
                try
                {
                    ProcessSessionCore(rpSession);
                }
                catch (Exception e)
                {
                    r_ExceptionBlock.Post(Tuple.Create(rpSession, e));

                    if (Debugger.IsAttached)
                        Debugger.Break();
                }
                //*/

                rStopwatch.Stop();
                rpSession.ParseTime = rStopwatch.ElapsedMilliseconds.ToString();
            });

            r_ExceptionBlock = new ActionBlock<Tuple<ApiSession, Exception>>(rpData =>
            {
                var rSession = rpData.Item1;
                var rException = rpData.Item2;

                rSession.Message = rException.ToString();
                NewException(rException);

                if (!Directory.Exists(@"Log\Exception"))
                    Directory.CreateDirectory(@"Log\Exception");

                var rPrefix = DateTime.Now.ToString("yyMMdd");
                var rRegex = new Regex(rPrefix + @"_(?<Index>\d+)\.json$");
                var rSaved = Directory.EnumerateFiles(@"Log\Exception", rPrefix + "_*.json");
                var rLast = 0;
                if (rSaved.Any())
                    rLast = rSaved.Max(r => int.Parse(rRegex.Match(r).Groups["Index"].Value));

                using (var rStreamWriter = new StreamWriter(string.Format(@"Log\Exception\{0}_{1}.json", rPrefix, rLast + 1), false, new UTF8Encoding(true)))
                {
                    rStreamWriter.WriteLine(r_TokenRegex.Value.Replace(rpData.Item1.FullUrl, "***************************"));
                    rStreamWriter.WriteLine();
                    rStreamWriter.WriteLine("Exception:");
                    rStreamWriter.WriteLine(rException.ToString());
                    rStreamWriter.WriteLine();
                    rStreamWriter.WriteLine("Response Data:");
                    rStreamWriter.WriteLine(Regex.Unescape(rSession.ResponseString));
                }
            });
        }

        static void ProcessSessionCore(ApiSession rpSession)
        {
            var rApi = rpSession.Url;
            var rRequestBody = rpSession.RequestBody;
            var rContent = rpSession.ResponseString.Replace("svdata=", string.Empty);

            HashSet<ApiParserBase> rParsers;
            if (!rContent.IsNullOrEmpty() && !rContent.StartsWith("[Fiddler]") && r_Parsers.TryGetValue(rApi, out rParsers))
            {
                Dictionary<string, string> rRequests = null;
                if (!rRequestBody.IsNullOrEmpty() && rRequestBody.Contains('&'))
                    rRequests = rRequestBody.Split('&').Where(r => r.Length > 0).Select(r => r.Split('='))
                        .ToDictionary(r => Uri.UnescapeDataString(r[0]), r => Uri.UnescapeDataString(r[1]));

                var rJson = JObject.Parse(rContent);

                foreach (var rParser in rParsers)
                {
                    rParser.Request = rRequests;
                    rParser.Path = rApi;
                    rParser.Process(rJson);
                }
            }
        }
        public static void Post(ApiSession rpSession)
        {
            r_Block.Post(rpSession);
        }
    }
}
