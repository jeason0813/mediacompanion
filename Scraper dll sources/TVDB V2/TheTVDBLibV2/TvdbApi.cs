////////////////////////////////////////////////////////////////////////////////// 
//                                                                              //
//      Copyright © 2005-2016 nzsjb                                             //
//                                                                              //
//  This Program is free software; you can redistribute it and/or modify        //
//  it under the terms of the GNU General Public License as published by        //
//  the Free Software Foundation; either version 2, or (at your option)         //
//  any later version.                                                          //
//                                                                              //
//  This Program is distributed in the hope that it will be useful,             //
//  but WITHOUT ANY WARRANTY; without even the implied warranty of              //
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                //
//  GNU General Public License for more details.                                //
//                                                                              //
//  You should have received a copy of the GNU General Public License           //
//  along with GNU Make; see the file COPYING.  If not, write to                //
//  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.       //
//  http://www.gnu.org/copyleft/gpl.html                                        //
//                                                                              //  
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Threading;
using System.Runtime.Serialization.Json;

namespace TheTvDB
{
    /// <summary>
    /// The class that contains the TVDB low level calls.
    /// </summary>
    public class TvdbAPI
    {        
        /// <summary>
        /// Get the current default language code.
        /// </summary>
        /// 
        public string DefaultLanguageCode { get { return defaultLanguageCode; } }

        /// <summary>
        /// The web response keys.
        /// </summary>
        public StringDictionary ResponseKeys { get; private set; }
        
        /// <summary>
        /// Get the minimum web access time.
        /// </summary>
        public int MinimumAccessTime { get; private set; }
        /// <summary>
        /// Get the total number of requests.
        /// </summary>
        public int TotalRequestCount { get; private set; }
        /// <summary>
        /// Get the total web request time.
        /// </summary>
        public TimeSpan? TotalRequestTime { get; private set; }
        /// <summary>
        /// Get the total number of delayed web requests.
        /// </summary>
        public int TotalDelays { get; private set; }
        /// <summary>
        /// Get the total web request delay time.
        /// </summary>
        public int TotalDelayTime { get; private set; }
        /// <summary>
        /// Get the total time between requests.
        /// </summary>
        public TimeSpan? TotalTimeBetweenRequests { get; private set; }
        /// <summary>
        /// Get the minimum time between web requests.
        /// </summary>
        public TimeSpan? MinimumTimeBetweenRequests { get; private set; }
        /// <summary>
        /// Get the maximum time between requests.
        /// </summary>
        public TimeSpan? MaximumTimeBetweenRequests { get; private set; }

        /// <summary>
        /// The image type.
        /// </summary>
        public enum ImageType
        {
            /// <summary>
            /// Image type is banner.
            /// </summary>
            Banner,
            /// <summary>
            /// Image type is poster.
            /// </summary>
            Poster,
            /// <summary>
            /// Image type is fan art.
            /// </summary>
            FanArt,
            /// <summary>
            /// Image type is actor.
            /// </summary>
            Actor,
            /// <summary>
            /// Image type is small poster.
            /// </summary>
            SmallPoster,
            /// <summary>
            /// Image type is small fan art.
            /// </summary>
            SmallFanArt,
            /// <summary>
            /// Image type is small actor.
            /// </summary>
            SmallActor,
            /// <summary>
            /// Image type is season.
            /// </summary>
            Season,
            /// <summary>
            /// Image type is small season.
            /// </summary>
            SmallSeason,
            /// <summary>
            /// Image type is season wide
            /// </summary>
            SeasonWide,
            /// <summary>
            /// Image type is series
            /// </summary>
            Series
        }

        /// <summary>
        /// The async request handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event parameter.</param>
        public delegate void TvdbAsyncHandler(object sender, TvdbAsyncEventArgs e);
        /// <summary>
        /// The delegate to process the JSON string.
        /// </summary>
        /// <param name="jsonString">The string to be processed.</param>
        /// <returns>The object created from the JSON deserialization.</returns>
        public delegate object ProcessJsonString(string jsonString);

        private const string getSeriesUrl           = "https://api.thetvdb.com/search/series?name={0}";
        private const string getSeriesDetailsUrl    = "https://api.thetvdb.com/series/{0}";
        private const string getSeriesBannersUrl    = "https://api.thetvdb.com/series/{0}/images/query?keyType={1}";
        private const string getSeriesActorsUrl     = "https://api.thetvdb.com/series/{0}/actors";
        private const string getSeriesEpisodesUrl   = "https://api.thetvdb.com/series/{0}/episodes?page={1}";
        private const string getEpisodeActorsUrl    = "https://api.thetvdb.com/episodes/{0}/actors";
        private const string getSeriesImageSummaryUrl = "https://api.thetvdb.com/series/{0}/images";
        private const string getEpisodeDetailsUrl   = "https://api.thetvdb.com/episodes/{0}";

        private const string bannerImageUrl         = "http://www.thetvdb.com/banners/{0}";
        private const string posterImageUrl         = "http://www.thetvdb.com/banners/{0}";
        private const string smallPosterImageUrl    = "http://www.thetvdb.com/banners/_cache/{0}";
        private const string fanArtImageUrl         = "http://www.thetvdb.com/banners/{0}";
        private const string smallFanArtImageUrl    = "http://www.thetvdb.com/banners/_cache/{0}";
        private const string actorImageUrl          = "http://www.thetvdb.com/banners/{0}";
        private const string smallActorImageUrl     = "http://www.thetvdb.com/banners/_cache/{0}";
        private const string tvdblanguages          = "https://api.thetvdb.com/languages";

        /// <summary>
        /// Get or set the flag that determines if responses are logged or not.
        /// </summary>
        public bool LogResponse { get; set; }

        private string apiKey;
        private string token;
        private string defaultLanguageCode = "en";
        private WebClient webClient;

        private DateTime? lastAccessTime; 

        private TvdbAPI() { }

        /// <summary>
        /// Initialize a new instance of the TvdbAPI class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        public TvdbAPI(string apiKey)
        {
            this.apiKey = apiKey;

            token = getToken(apiKey);

            MinimumAccessTime = 260;
            TotalRequestTime = new TimeSpan();
            TotalTimeBetweenRequests = new TimeSpan();
        }

        /// <summary>
        /// Initialize a new instance of the TvdbAPI class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="defaultLanguageCode">The default language code.</param>
        public TvdbAPI(string apiKey, string defaultLanguageCode) : this(apiKey)
        {
            this.defaultLanguageCode = defaultLanguageCode;
        }

        private string getToken(string apiKey)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.thetvdb.com/login");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"apikey\":\"" + apiKey + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            WebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string response = streamReader.ReadToEnd();
                string[] responseParts = response.Split('\"');

                if (responseParts.Length < 4)
                    return null;

                return responseParts[3];
            }
        }

        /// <summary>
        /// Search for the basic series data. 
        /// </summary>
        /// <param name="title">Part or all of the title.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <returns>The results object.</returns>
        public TvdbSeriesSearchResult GetSeries(string title, TvdbAsyncHandler completionHandler)
        {
            return GetSeries(title, completionHandler, defaultLanguageCode);
        }

        /// <summary>
        /// Search for the basic series data for a language code. 
        /// </summary>
        /// <param name="title">Part or all of the title.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The results object.</returns>
        public TvdbSeriesSearchResult GetSeries(string title, TvdbAsyncHandler completionHandler, string languageCode)
        {
            initializeFunction();

            string url = string.Format(getSeriesUrl, escapeQueryString(title));
            TvdbAsyncHandler asyncHandler = completionHandler;
            ProcessJsonString processString = new ProcessJsonString(seriesSearchResponse);

            return (TvdbSeriesSearchResult)getData(url, languageCode, new TvdbDelegates(asyncHandler, processString));
        }

        private object seriesSearchResponse(string responseString)
        {
            logResponse("Series Search", responseString);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TvdbSeriesSearchResult));
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(responseString));

            return serializer.ReadObject(stream) as TvdbSeriesSearchResult;
        }

        /// <summary>
        /// Gets list of available languages from TVDb
        /// </summary>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <returns></returns>
        public TvdbLanguagesResult GetTvdbLanguages(TvdbAsyncHandler completionHandler)
        {
            initializeFunction();
            string url = string.Format(tvdblanguages);
            TvdbAsyncHandler asyncHandler = completionHandler;
            ProcessJsonString processString = new ProcessJsonString(LanguageSearchResponse);

            return (TvdbLanguagesResult)getData(url, "en", new TvdbDelegates(asyncHandler, processString));
        }

        private object LanguageSearchResponse(string responseString)
        {
            logResponse("Language Search", responseString);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TvdbLanguagesResult));
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(responseString));

            return serializer.ReadObject(stream) as TvdbLanguagesResult;
        }

        /// <summary>
        /// Get the details of a series.
        /// </summary>
        /// <param name="identity">The series identity.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <returns>The results object.</returns>
        public TvdbSeriesInfoResult GetSeriesDetails(int identity, TvdbAsyncHandler completionHandler)
        {
            return GetSeriesDetails(identity, completionHandler, defaultLanguageCode);
        }

        /// <summary>
        /// Get the details of a series for a language code.
        /// </summary>
        /// <param name="identity">The series identity.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The results object.</returns>
        public TvdbSeriesInfoResult GetSeriesDetails(int identity, TvdbAsyncHandler completionHandler, string languageCode)
        {
            initializeFunction();

            string url = string.Format(getSeriesDetailsUrl, identity);
            TvdbAsyncHandler asyncHandler = completionHandler;
            ProcessJsonString processString = new ProcessJsonString(seriesDetailResponse);

            return (TvdbSeriesInfoResult)getData(url, languageCode, new TvdbDelegates(asyncHandler, processString));
        }

        private object seriesDetailResponse(string responseString)
        {
            logResponse("Series Detail", responseString);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TvdbSeriesInfoResult));
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(responseString));

            return serializer.ReadObject(stream) as TvdbSeriesInfoResult;            
        }





        /// <summary>
        /// Get the first page of episodes for a series.
        /// </summary>
        /// <param name="identity">The identity of the series.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <returns>The results object.</returns>
        public TvdbSeriesEpisodesResult GetSeriesEpisodes(int identity, TvdbAsyncHandler completionHandler)
        {
            return GetSeriesEpisodes(identity, 1, completionHandler);
        }

        /// <summary>
        /// Get a page of the episodes for a series.
        /// </summary>
        /// <param name="identity">The identity of the series.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <returns>The results object.</returns>
        public TvdbSeriesEpisodesResult GetSeriesEpisodes(int identity, int pageNumber, TvdbAsyncHandler completionHandler)
        {
            return GetSeriesEpisodes(identity, pageNumber, completionHandler, defaultLanguageCode);
        }

        /// <summary>
        /// Get the episodes of a series for a language code.
        /// </summary>
        /// <param name="identity">The identity of the series.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The results object.</returns>
        public TvdbSeriesEpisodesResult GetSeriesEpisodes(int identity, TvdbAsyncHandler completionHandler, string languageCode)
        {
            return GetSeriesEpisodes(identity, 1, completionHandler, languageCode);
        }

        /// <summary>
        /// Get a page of the episodes of a series for a language code.
        /// </summary>
        /// <param name="identity">The identity of the series.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The results object.</returns>
        public TvdbSeriesEpisodesResult GetSeriesEpisodes(int identity, int pageNumber, TvdbAsyncHandler completionHandler, string languageCode)
        {
            initializeFunction();

            string url = string.Format(getSeriesEpisodesUrl, identity, pageNumber);
            TvdbAsyncHandler asyncHandler = completionHandler;
            ProcessJsonString processString = new ProcessJsonString(seriesEpisodesResponse);

            return (TvdbSeriesEpisodesResult)getData(url, languageCode, new TvdbDelegates(asyncHandler, processString));
        }

        /// <summary>
        ///  Get all episodes for a series for a language code.
        /// </summary>
        /// <param name="identity">The identity of the series.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <param name="instance"></param>
        /// <param name="alldetails">If True, populate all episode data</param>
        /// <returns>The results object.</returns>
        public Collection<TvdbEpisode> GetSeriesAllEpisodes(int identity, string languageCode, TvdbAsyncHandler completionHandler, TvdbAPI instance, bool alldetails = false)
        {
            Collection<TvdbEpisode> allepisodes = new Collection<TvdbEpisode>();
            TvdbSeriesEpisodesResult episodesinfo;

            if (languageCode == null)
                episodesinfo = GetSeriesEpisodes(identity, null);
            else
                episodesinfo = GetSeriesEpisodes(identity, null, languageCode);

            foreach (TvdbEpisode episode in episodesinfo.Episodes)
            {
                if (alldetails == true)
                {
                    episode.LoadDetails(instance, languageCode);
                }
                addEpisode(episode, allepisodes);
            }

            while (episodesinfo.PageLinks.NextPageNumber != -1)
            {
                if (languageCode == null)
                    episodesinfo = GetSeriesEpisodes(identity, episodesinfo.PageLinks.NextPageNumber, null);
                else
                    episodesinfo = GetSeriesEpisodes(identity, episodesinfo.PageLinks.NextPageNumber, null, languageCode);

                foreach (TvdbEpisode episode in episodesinfo.Episodes)
                {
                    if (alldetails == true)
                    {
                        episode.LoadDetails(instance, languageCode);
                    }
                    addEpisode(episode, allepisodes);
                }
            }
            return allepisodes;
        }

        private void addEpisode(TvdbEpisode newEpisode, Collection<TvdbEpisode> episodes)
        {
            foreach (TvdbEpisode oldEpisode in episodes)
            {
                if (oldEpisode.SeasonNumber > newEpisode.SeasonNumber)
                {
                    episodes.Insert(episodes.IndexOf(oldEpisode), newEpisode);
                    return;
                }
                else
                {
                    if (oldEpisode.EpisodeNumber > newEpisode.EpisodeNumber)
                    {
                        episodes.Insert(episodes.IndexOf(oldEpisode), newEpisode);
                        return;
                    }
                }
            }

            episodes.Add(newEpisode);
        }



        private object seriesEpisodesResponse(string responseString)
        {
            logResponse("Series Episodes", responseString);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TvdbSeriesEpisodesResult));
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(responseString));

            return serializer.ReadObject(stream) as TvdbSeriesEpisodesResult;
        }

        /// <summary>
        /// Get the details of an episode.
        /// </summary>
        /// <param name="episodeIdentity">The episode identity.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <returns>The results object.</returns>
        public TvdbEpisodeInfoResult GetEpisodeDetails(int episodeIdentity, TvdbAsyncHandler completionHandler)
        {
            return GetEpisodeDetails(episodeIdentity, completionHandler, defaultLanguageCode);
        }

        /// <summary>
        /// Get the details of an episode for a language code.
        /// </summary>
        /// <param name="episodeIdentity">The episode identity.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The results object.</returns>
        public TvdbEpisodeInfoResult GetEpisodeDetails(int episodeIdentity, TvdbAsyncHandler completionHandler, string languageCode)
        {
            initializeFunction();

            string url = string.Format(getEpisodeDetailsUrl, episodeIdentity);
            TvdbAsyncHandler asyncHandler = completionHandler;
            ProcessJsonString processString = new ProcessJsonString(episodeDetailResponse);

            return (TvdbEpisodeInfoResult)getData(url, languageCode, new TvdbDelegates(asyncHandler, processString));
        }

        private object episodeDetailResponse(string responseString)
        {
            logResponse("Episode Detail", responseString);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TvdbEpisodeInfoResult));
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(responseString));

            return serializer.ReadObject(stream) as TvdbEpisodeInfoResult;
        }

        /// <summary>
        /// Get the banners for a series.
        /// </summary>
        /// <param name="identity">The series identity.</param>
        /// <param name="imageType">The type of image to get.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The results object.</returns>
        public TvdbBannersResult GetSeriesBanners(int identity, ImageType imageType, TvdbAsyncHandler completionHandler, String languageCode)
        {
            initializeFunction();

            string url = string.Format(getSeriesBannersUrl, identity, imageType.ToString());
            TvdbAsyncHandler asyncHandler = completionHandler;
            ProcessJsonString processString = new ProcessJsonString(seriesBannersResponse);

            return (TvdbBannersResult)getData(url, languageCode, new TvdbDelegates(asyncHandler, processString));
        }

        private object seriesBannersResponse(string responseString)
        {
            logResponse("Series Banners", responseString);
            
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TvdbBannersResult));
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(responseString));

            return serializer.ReadObject(stream) as TvdbBannersResult; 
        }

        /// <summary>
        /// Get the image summary for a series.
        /// </summary>
        /// <param name="identity">The series identity.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <returns>The results object.</returns>
        public TvdbImageSummaryResult GetSeriesImageSummary(int identity,  TvdbAsyncHandler completionHandler)
        {
            initializeFunction();

            string url = string.Format(getSeriesImageSummaryUrl, identity);
            TvdbAsyncHandler asyncHandler = completionHandler;
            ProcessJsonString processString = new ProcessJsonString(seriesImageSummaryResponse);

            return (TvdbImageSummaryResult)getData(url, null, new TvdbDelegates(asyncHandler, processString));
        }

        private object seriesImageSummaryResponse(string responseString)
        {
            logResponse("Series Image Summary", responseString);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TvdbImageSummaryResult));
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(responseString));

            return serializer.ReadObject(stream) as TvdbImageSummaryResult;
        }

        /// <summary>
        /// Get the actors for a series.
        /// </summary>
        /// <param name="identity">The series identity.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <returns>A results object.</returns>
        public TvdbActorsResult GetSeriesActors(int identity, TvdbAsyncHandler completionHandler)
        {
            initializeFunction();

            string url = string.Format(getSeriesActorsUrl, identity);
            TvdbAsyncHandler asyncHandler = completionHandler;
            ProcessJsonString processString = new ProcessJsonString(seriesActorsResponse);

            return (TvdbActorsResult)getData(url, null, new TvdbDelegates(asyncHandler, processString));
        }

        private object seriesActorsResponse(string responseString)
        {
            logResponse("Series Actors", responseString);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TvdbActorsResult));
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(responseString));

            return serializer.ReadObject(stream) as TvdbActorsResult; 
        }

        /// <summary>
        /// Get the actors for an episode.
        /// </summary>
        /// <param name="identity">The episode identity number.</param>
        /// <param name="completionHandler">The async completion handler. May be null.</param>
        /// <returns>The results object.</returns>
        public TvdbActorsResult GetEpisodeActors(int identity, TvdbAsyncHandler completionHandler)
        {
            initializeFunction();

            string url = string.Format(getEpisodeActorsUrl, identity);
            TvdbAsyncHandler asyncHandler = completionHandler;
            ProcessJsonString processString = new ProcessJsonString(episodeActorsResponse);

            return (TvdbActorsResult)getData(url, null, new TvdbDelegates(asyncHandler, processString));
        }

        private object episodeActorsResponse(string responseString)
        {
            logResponse("Episode Actors", responseString);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TvdbActorsResult));
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(responseString));

            return serializer.ReadObject(stream) as TvdbActorsResult;
        }

        /// <summary>
        /// Download an image.
        /// </summary>
        /// <param name="imageType">The type of image.</param>
        /// <param name="filePath">The filename of the image.</param>
        /// <param name="size">The size of the image. Currently not used.</param>
        /// <param name="outputPath">The output path for the downloaded image.</param>
        /// <returns>True if the image is downloaded; false otherwise.</returns>
        public bool GetImage(ImageType imageType, string filePath, int size, string outputPath)
        {
            string url;

            switch (imageType)
            {
                case ImageType.Actor:
                    url = string.Format(actorImageUrl, filePath);
                    break;
                case ImageType.SmallActor:
                    url = string.Format(smallActorImageUrl, filePath);
                    break;
                case ImageType.Banner:
                    url = string.Format(bannerImageUrl, filePath);
                    break;
                case ImageType.FanArt:
                    url = string.Format(fanArtImageUrl, filePath);
                    break;
                case ImageType.SmallFanArt:
                    url = string.Format(smallFanArtImageUrl, filePath);
                    break;
                case ImageType.Poster:
                    url = string.Format(posterImageUrl, filePath);
                    break;
                case ImageType.SmallPoster:
                    url = string.Format(smallPosterImageUrl, filePath);
                    break;
                default:
                    return false;
            }

            try
            {
                checkRequestRate();

                getWebClient(null).DownloadFile(url, outputPath);

                TotalRequestCount++;
                TotalRequestTime += DateTime.Now - lastAccessTime.Value;
            }
            catch (WebException)
            {
                if (webClient.ResponseHeaders != null)
                {
                    ResponseKeys = new StringDictionary();

                    for (int index = 0; index < webClient.ResponseHeaders.Count; index++)
                    {
                        string header = webClient.ResponseHeaders.GetKey(index);
                        string[] values = webClient.ResponseHeaders.GetValues(index);

                        if (values != null)
                        {
                            foreach (string headerValue in values)
                                ResponseKeys.Add(header, headerValue);
                        }
                    }
                }

                throw;
            }

            return true;
        }

        private void initializeFunction()
        {
            if (apiKey == null)
                throw new InvalidOperationException("The API key has not been set");
            if (token == null)
                throw new InvalidOperationException("The token has not been set");

            if (webClient != null && webClient.IsBusy)
                throw new InvalidOperationException("Request in progress");
        }

        private string escapeQueryString(string inputString)
        {
            return Uri.EscapeDataString(inputString);
        }

        private object getData(string url, string languageCode, TvdbDelegates delegates)
        {
            webClient = getWebClient(languageCode);            

            if (delegates.AsyncHandler != null)
            {
                checkRequestRate();

                webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(webClientDownloadDataCompleted);
                webClient.DownloadDataAsync(new Uri(url), delegates);
                
                return null;
            }
            else
            {
                try
                {
                    checkRequestRate();

                    byte[] response = webClient.DownloadData(new Uri(url));

                    TotalRequestCount++;
                    TotalRequestTime += DateTime.Now - lastAccessTime.Value;

                    return delegates.JsonHandler(Encoding.UTF8.GetString(response));
                }
                catch (WebException e)
                {
                    if (webClient.ResponseHeaders != null)
                    {
                        ResponseKeys = new StringDictionary();

                        for (int index = 0; index < webClient.ResponseHeaders.Count; index++)
                        {
                            string header = webClient.ResponseHeaders.GetKey(index);
                            string[] values = webClient.ResponseHeaders.GetValues(index);

                            if (values != null)
                            {
                                foreach (string headerValue in values)
                                    ResponseKeys.Add(header, headerValue);
                            }
                        }
                    }

                    if (e.Message.Contains("404"))
                        return (null);

                    throw;
                }
            }
        }

        private void webClientDownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            TotalRequestCount++;
            TotalRequestTime += DateTime.Now - lastAccessTime.Value;

            TvdbDelegates delegates = e.UserState as TvdbDelegates;
            delegates.AsyncHandler(null, new TvdbAsyncEventArgs(delegates.JsonHandler(Encoding.UTF8.GetString(e.Result))));            
        }

        private WebClient getWebClient(string languageCode)
        {
            if (webClient == null)
                webClient = new WebClient();

            webClient.Headers.Clear();
            webClient.Headers["Accept"] = "application/json"; 
            webClient.Headers["Authorization"] = "Bearer " + token;
            
            if (languageCode != null)
                webClient.Headers["Accept-Language"] = languageCode; 

            return webClient;
        }

        private void logResponse(string fileName, string xmlString)
        {
            if (!LogResponse)
                return;

            FileStream fileStream = new FileStream(@"c:\temp\TVDB " + fileName + ".txt", FileMode.Create, FileAccess.Write);            
            StreamWriter streamWriter = new StreamWriter(fileStream);

            streamWriter.Write(xmlString);
            
            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();

        }

        private void checkRequestRate()
        {
            if (lastAccessTime != null)
            {
                TimeSpan gap = DateTime.Now - lastAccessTime.Value;

                TotalTimeBetweenRequests += gap;
                if (MinimumTimeBetweenRequests == null || gap < MinimumTimeBetweenRequests.Value)
                    MinimumTimeBetweenRequests = gap;
                if (MaximumTimeBetweenRequests == null || gap > MaximumTimeBetweenRequests.Value)
                    MaximumTimeBetweenRequests = gap;

                if (gap.TotalMilliseconds < MinimumAccessTime)
                {
                    int waitTime = (int)(MinimumAccessTime - gap.TotalMilliseconds);

                    TotalDelays++;
                    TotalDelayTime += waitTime;

                    Thread.Sleep(waitTime);
                }
            }

            lastAccessTime = DateTime.Now;
        }
    }
}
