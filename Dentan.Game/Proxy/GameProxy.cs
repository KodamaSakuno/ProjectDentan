﻿using Fiddler;
using Moen.KanColle.Dentan.Api;
using System;
using System.Diagnostics;
using System.IO;
using FiddlerSession = Fiddler.Session;

namespace Moen.KanColle.Dentan.Proxy
{
    public class GameProxy
    {
        public int Port { get; private set; }

        public bool EnableUpstreamProxy { get; set; }
        public string UpstreamProxy { get; set; }
        public bool UseSSL { get; set; }

        public event Action<Session> NewSession = delegate { };
        public event Action ProxyShutdown = delegate { };
        public event Action<string> GameToken = delegate { };

        public GameProxy()
        {
            FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
            FiddlerApplication.OnReadResponseBuffer += FiddlerApplication_OnReadResponseBuffer;
            FiddlerApplication.BeforeResponse += FiddlerApplication_BeforeResponse;
            FiddlerApplication.BeforeReturningError += FiddlerApplication_BeforeReturningError;
            FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;
        }

        void FiddlerApplication_BeforeRequest(FiddlerSession rpSession)
        {
            var rStopwatch = Stopwatch.StartNew();

            if (EnableUpstreamProxy)
                rpSession["x-OverrideGateway"] = UpstreamProxy;

            var rRequest = rpSession.oRequest;
            var rPath = rpSession.PathAndQuery;
            var rIsGame = rPath.StartsWith("/kcs");
            var rIsResource = rPath.StartsWith("/kcs/") && !rPath.StartsWith("/kcs/mainD2.swf");

            if (rPath.Contains("/kcs/mainD2.swf"))
                GameToken(rpSession.fullUrl);

            Session rSession;
            if (!rIsGame)
                rSession = new Session(rpSession.fullUrl);
            else if (rIsResource)
            {
                var rResourceSession = new ResourceSession(rpSession.fullUrl, rpSession.PathAndQuery.Substring(1));
                rSession = rResourceSession;

                LoadFromCache(rpSession, rResourceSession);
            }
            else
            {
                var rUrl = rpSession.PathAndQuery;
                var rPosition = rUrl.IndexOf("/kcsapi/");
                if (rPosition != -1)
                    rUrl = rUrl.Substring(rPosition + 8);
                rSession = new ApiSession(rpSession.fullUrl, rUrl) { RequestBody = Uri.UnescapeDataString(rpSession.GetRequestBodyAsString()) };
            }

            rpSession.Tag = rSession;

            rSession.Stopwatch = rStopwatch;
            if (rSession.Status != SessionStatus.LoadedFromCache)
                rSession.Status = SessionStatus.Request;

            NewSession(rSession);

            Debug.WriteLine("Request - " + rpSession.fullUrl);
        }

        void FiddlerApplication_OnReadResponseBuffer(object sender, RawReadEventArgs e)
        {
            var rSession = e.sessionOwner.Tag as Session;
            if (rSession != null)
                rSession.LoadedBytes += e.iCountOfBytes;
        }
        void FiddlerApplication_BeforeResponse(FiddlerSession rpSession)
        {
            var rSession = rpSession.Tag as Session;
            if (rSession != null)
            {
                if (rSession.Status == SessionStatus.Request)
                    rSession.Status = SessionStatus.Responsed;

                var rApiSession = rSession as ApiSession;
                if (rApiSession != null)
                {
                    rApiSession.ResponseString = rpSession.GetResponseBodyAsString();
                    ApiParsers.Post(rApiSession);
                }

                var rResourceSession = rSession as ResourceSession;
                if (rResourceSession != null && ResourceCache.IsEnabled && rpSession.responseCode == 200 && !File.Exists(rResourceSession.CachePath) && rpSession.oResponse["Last-Modified"] != null)
                {
                    rResourceSession.Data = rpSession.ResponseBody;
                    rResourceSession.LastModifiedTime = Convert.ToDateTime(rpSession.oResponse["Last-Modified"]);
                    ResourceCache.SaveFile(rResourceSession);
                }

                if (rSession.Url.Contains("kcs/sound/titlecall/") || rSession.Url.Contains("api_start2"))
                    KanColleGame.Current.RaiseGameLaunchedEvent();
            }

            Debug.WriteLine("Response - " + rpSession.fullUrl);
        }
        void FiddlerApplication_BeforeReturningError(FiddlerSession rpSession)
        {
            var rSession = rpSession.Tag as Session;
            if (rSession != null)
            {
                rSession.Message = rpSession.GetResponseBodyAsString();
                rSession.Status = SessionStatus.Error;
            }
        }
        void FiddlerApplication_AfterSessionComplete(FiddlerSession rpSession)
        {
            var rSession = rpSession.Tag as Session;
            if (rSession != null)
            {
                rSession.Stopwatch.Stop();
                rSession.TotalTime = rSession.Stopwatch.ElapsedMilliseconds.ToString();
                rSession.StatusCode = rpSession.responseCode;
            }
        }

        public void Start(int rpPort)
        {
            Port = rpPort;
            var rStartupFlags = FiddlerCoreStartupFlags.ChainToUpstreamGateway;
            if (UseSSL)
                rStartupFlags |= FiddlerCoreStartupFlags.DecryptSSL;

            FiddlerApplication.Startup(rpPort, rStartupFlags);
        }
        public void Shutdown()
        {
            FiddlerApplication.Shutdown();

            ProxyShutdown();
        }

        public void SetUpstreamProxy(string rpAddress, int rpPort)
        {
            UpstreamProxy = string.Format("{0}:{1}", rpAddress, rpPort);
        }

        static void LoadFromCache(FiddlerSession rpSession, ResourceSession rpResourceSession)
        {
            if (!ResourceCache.IsEnabled) return;

            var rPath = rpResourceSession.OriginalCachePath;
            var rExtension = Path.GetExtension(rpResourceSession.CachePath);

            var rForcePath = Path.Combine(ResourceCache.CacheFolder, rpResourceSession.FileNameWithoutVersion + ".hack" + rExtension);
            if (File.Exists(rForcePath))
                LoadFromCacheCore(rpSession, rpResourceSession, rForcePath);
            else if (File.Exists(rpResourceSession.CachePath))
            {
                if (File.Exists(rPath) && File.GetLastWriteTime(rPath) > File.GetLastWriteTime(rpResourceSession.CachePath))
                {
                    File.Delete(rpResourceSession.CachePath);
                    File.Move(rPath, rpResourceSession.CachePath);
                }

                LoadFromCacheCore(rpSession, rpResourceSession, rpResourceSession.CachePath);
            }
            else if (File.Exists(rPath))
            {
                File.Move(rPath, rpResourceSession.CachePath);
                LoadFromCacheCore(rpSession, rpResourceSession, rpResourceSession.CachePath);
            }
        }
        static void LoadFromCacheCore(FiddlerSession rpSession, ResourceSession rpResourceSession, string rpPath)
        {
            rpSession.utilCreateResponseAndBypassServer();
            rpSession.ResponseBody = File.ReadAllBytes(rpPath);

            rpResourceSession.LoadedBytes = rpSession.ResponseBody.Length;
            rpResourceSession.Status = SessionStatus.LoadedFromCache;
        }
    }
}
