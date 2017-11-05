using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class TvReleases
    {
        public string iso_3166_1 { get; set; }
        public string rating { get; set; }
    }

    public class TmdbTvReleases
    {
        public int id { get; set; }
        public List<TvReleases> results { get; set; }
    }
}
