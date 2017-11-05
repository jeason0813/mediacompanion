namespace YouTubeFisher
{
    public class YouTubeVideoFile
    {
        private byte formatCode;

        public YouTubeVideoFile(string url, byte formatCode)
        {
            this.VideoUrl = url;
            this.FormatCode = formatCode;
        }

        public string VideoUrl { get; set; }

        public YouTubeVideoType VideoFormat { get; private set; }

        public byte FormatCode
        {
            get { return this.formatCode; }
            set
            {
                this.formatCode = value;

                switch (value)
                {
                    case 34:
                    case 35:
                    case 5:
                    case 6:
                        this.VideoFormat = YouTubeVideoType.Flash;
                        break;

                    case 18:
                    case 22:
                    case 37:
                    case 38:
                    case 82:
                    case 84:
                        this.VideoFormat = YouTubeVideoType.MP4;
                        break;

                    case 13:
                    case 17:
                    case 36:
                        this.VideoFormat = YouTubeVideoType.Mobile;
                        break;

                    case 43:
                    case 45:
                    case 46:
                        this.VideoFormat = YouTubeVideoType.WebM;
                        break;

                    default:
                        this.VideoFormat = YouTubeVideoType.Unknown;
                        break;
                }
            }
        }

        public string Resolution
        {
            get
            {
                switch (formatCode)
                {
                    case 43: return "SD";
                    case 44: return "480";
                    case 45: return "720";
                    case 46: return "1080";
                    case 38: return "4096";
                    case 37: return "1080";
                    case 22: return "720";
                    case 82: return "3D SD";
                    case 84: return "3D 720";
                    case 35: return "480";
                    case 34: return "SD";
                    case 18: return "SD";
                    //case  6 : return "LQ Flash Video [MP3.44KHz] (*.flv)|*.flv|";
                    //case  5 : return "LQ Flash Video [MP3.22KHz] (*.flv)|*.flv|";
                    //case 13 : return "Mobile Video XX-Small (*.3gp)|*.3gp|";
                    //case 17 : return "Mobile Video X-Small (*.3gp)|*.3gp|";
                    //case 36 : return "Mobile Video Small (*.3gp)|*.3gp|";
                    default: return "unknown";
                }
            }
        }

        public string Extension
        {
            get
            {
                switch (formatCode)
                {
                    case 43: return "webm";
                    case 44: return "webm";
                    case 45: return "webm";
                    case 46: return "webm";
                    case 38: return "mp4";
                    case 37: return "mp4";
                    case 22: return "mp4";
                    case 82: return "mp4";
                    case 84: return "mp4";
                    case 35: return "flv";
                    case 34: return "flv";
                    case 18: return "mp4";
                    //case  6 : return "LQ Flash Video [MP3.44KHz] (*.flv)|*.flv|";
                    //case  5 : return "LQ Flash Video [MP3.22KHz] (*.flv)|*.flv|";
                    //case 13 : return "Mobile Video XX-Small (*.3gp)|*.3gp|";
                    //case 17 : return "Mobile Video X-Small (*.3gp)|*.3gp|";
                    //case 36 : return "Mobile Video Small (*.3gp)|*.3gp|";
                    default: return "unknown";
                }
            }
        }

        public long VideoSize { get; set; }
    }
}
