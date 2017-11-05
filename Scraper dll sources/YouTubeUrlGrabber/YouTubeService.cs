using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;


namespace YouTubeFisher
{
    public class YouTubeUrlGrabber
    {
        private const string VideoInfoPageUrl = "http://www.youtube.com/get_video_info?&video_id={0}&el=detailpage&ps=default&eurl=&gl=US&hl=en";
        private const string VideoUrlsSeparator = ",";

        private string videoId;
        private NameValueCollection videoInfoCollection;
        private List<YouTubeVideoFile> availableVideoFormat = new List<YouTubeVideoFile>();

        public YouTubeUrlGrabber()
        {
            // Does nothing more than preventing the class from being instantiated.
        }

        public string VideoUrl { get; private set; }

        public string VideoTitle { get; private set; }

        public YouTubeVideoFile[] AvailableVideoFormat
        {
            // Get (the format of) all videos available..
            get { return this.availableVideoFormat.ToArray(); }
        }


        public static YouTubeUrlGrabber Create(string youTubeVideoUrl)
        {
            YouTubeUrlGrabber service = new YouTubeUrlGrabber();
            service.VideoUrl = youTubeVideoUrl;
            //		service.videoId = HttpUtility.ParseQueryString(new Uri(service.VideoUrl).Query)["v"];
            service.videoId = ParseQueryString(new Uri(service.VideoUrl).Query)["v"];

            service.GetVideoInfo();
            //service.GetVideoTitle();
            service.PopulateAvailableVideoFormatList();

            return service;
        }

        private void GetVideoInfo()
        {
            string videoInfoPageSource = this.DownloadString(string.Format(VideoInfoPageUrl, this.videoId));
            //		this.videoInfoCollection = HttpUtility.ParseQueryString(videoInfoPageSource);
            this.videoInfoCollection = ParseQueryString(videoInfoPageSource);
        }

        private void GetVideoTitle()
        {
            this.VideoTitle = this.videoInfoCollection["title"];

            // Remove the invalid characters in file names
            // In Windows they are: \ / : * ? " < > |
            this.VideoTitle = Regex.Replace(this.VideoTitle, @"[:\*\?""\<\>\|]", string.Empty);
            this.VideoTitle = this.VideoTitle.Replace("\\", "-").Replace("/", "-").Trim();

            if (string.IsNullOrEmpty(this.VideoTitle))
            {
                this.VideoTitle = this.videoId;
            }
        }

        private string DownloadString(string url)
        {
            WebRequest req = WebRequest.Create(url);
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            string source = new StreamReader(resp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            resp.Close();

            return source;
        }

        private void PopulateAvailableVideoFormatList()
        {
            var availableFormats = this.videoInfoCollection["url_encoded_fmt_stream_map"];

            if (availableFormats == null) { return; }
            if (availableFormats == string.Empty) { return; }

            var formatList = new List<string>();

            try
            {
                formatList = new List<string>(Regex.Split(availableFormats, VideoUrlsSeparator));
            }
            catch
            {
            }

            formatList.ForEach(
                delegate (string format)
                {
                    if (string.IsNullOrEmpty(format.Trim())) { return; }

                    //					var formatInfoCollection = HttpUtility.ParseQueryString(format);
                    var formatInfoCollection = ParseQueryString(format);
                    var urlEncoded = formatInfoCollection["url"];
                    var itag = formatInfoCollection["itag"];
                    var signature = formatInfoCollection["sig"];
                    var fallbackHost = formatInfoCollection["fallback_host"];

                    try
                    {
                        var formatCode = byte.Parse(itag);

                        var newUrlEncoded = string.Format("{0}&fallback_host={1}&signature={2}", urlEncoded, fallbackHost, signature);

                        //var url = new Uri(Uri.UnescapeDataString(Uri.UnescapeDataString(newUrlEncoded)));

                        //// for this version, only use the download URL
                        //this.availableVideoFormat.Add(new YouTubeVideoFile(url.ToString(), formatCode));


                        string url = Uri.UnescapeDataString(Uri.UnescapeDataString(newUrlEncoded));

                        // for this version, only use the download URL
                        this.availableVideoFormat.Add(new YouTubeVideoFile(url, formatCode));

                    }
                    catch
                    {
                    }
                });
        }



        public static NameValueCollection ParseQueryString(string uri)
        {
            NameValueCollection nvc = new NameValueCollection();

            var s = (uri[0] == '?' || uri[0] == '@') ? uri : "?" + uri;

            var matches = Regex.Matches(s, @"[\?&](([^&=]+)=([^&=#]*))", RegexOptions.Compiled);


            foreach (Match m in matches)
            {
                nvc.Add(Uri.UnescapeDataString(m.Groups[2].Value), Uri.UnescapeDataString(m.Groups[3].Value));
            }



            return nvc;
        }



        public YouTubeVideoFile selectTrailer(string preferredResolution)
        {
            List<string> videoResolutions = new List<string> { "1080", "720", "480", "SD" };

            var startRes = videoResolutions.FindIndex(s => s == preferredResolution);

            for (var i = startRes; i < videoResolutions.Count; i++)
            {
                var videoExtensions = "mp4,flv,webm".Split(',');

                foreach (String extension in videoExtensions)
                {
                    var q = from t in AvailableVideoFormat where (t.Resolution == videoResolutions[i] && t.Extension == extension) select t;

                    if (q.Count() > 0)
                    {
                        return q.First();
                    }
                }
            }

            return null;
        }

    }
}
