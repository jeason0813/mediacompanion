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
//  Addition: Languages and Languages Results                                   //
//  By Rob Miller - 2017-08-27                                                  //
//                                                                              //  
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;

namespace TheTvDB
{
    /// <summary>
    /// The class that describes a series.
    /// </summary>
    [DataContract]
    public class TvdbLanguages
    {
        /// <summary>
        /// Get or set the Language Abbreviation.
        /// </summary>
        [DataMember(Name = "abbreviation")]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Get or set the English Name of Language.
        /// </summary>
        [DataMember(Name = "englishName")]
        public string EnglishName { get; set; }

        /// <summary>
        /// Get or set the Language ID.
        /// </summary>
        [DataMember(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// Get or set the Language Name.
        /// </summary>
        [DataMember(Name = "name")]
        public string name { get; set; }
    }
    //    /// <summary>
    //    /// Get or set the series ID.
    //    /// </summary>
    //    [DataMember(Name = "seriesId")]
    //    public string SeriesId { get; set; }

    //    /// <summary>
    //    /// Get or set the status.
    //    /// </summary>
    //    [DataMember(Name = "status")]
    //    public string Status { get; set; }

    //    /// <summary>
    //    /// Get or set the first air date string.
    //    /// </summary>
    //    [DataMember(Name = "firstAired")]
    //    public string FirstAired { get; set; }

    //    /// <summary>
    //    /// Get or set the network.
    //    /// </summary>
    //    [DataMember(Name = "network")]
    //    public string Network { get; set; }

    //    /// <summary>
    //    /// Get or set the network ID.
    //    /// </summary>
    //    [DataMember(Name = "networkId")]
    //    public string NetworkId { get; set; }

    //    /// <summary>
    //    /// Get or set the runtime.
    //    /// </summary>
    //    [DataMember(Name = "runtime")]
    //    public string Runtime { get; set; }

    //    /// <summary>
    //    /// Get or set the string of genres.
    //    /// </summary>
    //    [DataMember(Name = "genre")]
    //    public Collection<string> Genres { get; set; }

    //    /// <summary>
    //    /// Get or set the overview.
    //    /// </summary>
    //    [DataMember(Name = "overview")]
    //    public string Overview { get; set; }

    //    /// <summary>
    //    /// Get or set the last update date string.
    //    /// </summary>
    //    [DataMember(Name = "lastUpdated")]
    //    public string LastUpdated { get; set; }

    //    /// <summary>
    //    /// Get or set the day of the week the series airs.
    //    /// </summary>
    //    [DataMember(Name = "airsDayOfWeek")]
    //    public string AirsDayOfWeek { get; set; }

    //    /// <summary>
    //    /// Get or set the time the series airs.
    //    /// </summary>
    //    [DataMember(Name = "airsTime")]
    //    public string AirsTime { get; set; }

    //    /// <summary>
    //    /// Get or set the content rating.
    //    /// </summary>
    //    [DataMember(Name = "rating")]
    //    public string ContentRating { get; set; }

    //    /// <summary>
    //    /// Get or set IMDB reference number.
    //    /// </summary>
    //    [DataMember(Name = "imdbId")]
    //    public string ImdbID { get; set; }

    //    /// <summary>
    //    /// Get or set the series name.
    //    /// </summary>
    //    [DataMember(Name = "zap2itId")]
    //    public string Zap2ItID { get; set; }

    //    /// <summary>
    //    /// Get or set the added date.
    //    /// </summary>
    //    [DataMember(Name = "added")]
    //    public string AddedDate { get; set; }

    //    /// <summary>
    //    /// Get or set who added the data.
    //    /// </summary>
    //    [DataMember(Name = "addedBy")]
    //    public string AddedBy { get; set; }

    //    /// <summary>
    //    /// Get or set the user rating.
    //    /// </summary>
    //    [DataMember(Name = "siteRating")]
    //    public decimal Rating { get; set; }

    //    ///// <summary>
    //    ///// Get or set the Series similarity
    //    ///// </summary>
    //    //[DataMember(Name = "Similarity")]
    //    //public double Similarity { get; set; }

    //    /// <summary>
    //    /// Get or set the first air date.
    //    /// </summary>
    //    public DateTime? FirstAiredDate { get { return TvdbUtils.StringToDate(FirstAired); } }

    //    /// <summary>
    //    /// Get the string representation of the first aired date.
    //    /// </summary>
    //    public string FirstAiredString
    //    {
    //        get
    //        {
    //            if (FirstAiredDate == null || !FirstAiredDate.HasValue)
    //                return string.Empty;
    //            else
    //                return FirstAiredDate.Value.ToShortDateString();
    //        }
    //    }

    //    /// <summary>
    //    /// Get the running time. Returns null if not available or invalid.
    //    /// </summary>
    //    public TimeSpan? RunningTime
    //    {
    //        get
    //        {
    //            int minutes = TvdbUtils.StringToInt(Runtime);
    //            if (minutes == -1)
    //                return null;

    //            return new TimeSpan(minutes * TimeSpan.TicksPerMinute);
    //        }
    //    }

    //    /// <summary>
    //    /// Get the string representation of the running time.
    //    /// </summary>
    //    public string RunningTimeString
    //    {
    //        get
    //        {
    //            if (RunningTime == null || !RunningTime.HasValue)
    //                return string.Empty;
    //            else
    //                return RunningTime.Value.ToString();
    //        }
    //    }

    //    /// <summary>
    //    /// Series Similarity.
    //    /// </summary>
    //    public double Similarity { get; set; }

    //    /// <summary>
    //    /// Get a comma separated string of genres.
    //    /// </summary>
    //    public string GenresDisplayString { get { return TvdbUtils.CollectionToString(Genres); } }

    //    /// <summary>
    //    /// Get or set the collection of actors.
    //    /// </summary>
    //    public Collection<TvdbActor> Actors { get; set; }

    //    /// <summary>
    //    /// Get the collection of actors names.
    //    /// </summary>
    //    public Collection<string> ActorsNames
    //    {
    //        get
    //        {
    //            if (Actors == null || Actors.Count == 0)
    //                return(null);

    //            Collection<string> actorsNames = new Collection<string>();

    //            foreach (TvdbActor actor in Actors)
    //            {
    //                if (!string.IsNullOrWhiteSpace(actor.Name) && !actorsNames.Contains(actor.Name))
    //                    actorsNames.Add(actor.Name);
    //            }

    //            return (actorsNames.Count != 0 ? actorsNames : null); 
    //        }
    //    }

    //    /// <summary>
    //    /// Get a comma separated string of actors.
    //    /// </summary>
    //    public string ActorsDisplayString { get { return TvdbUtils.CollectionToString(ActorsNames); } }

    //    /// <summary>
    //    /// Get or set the collection of banners.
    //    /// </summary>
    //    public Collection<TvdbBanner> Banners { get; set; }

    //    /// <summary>
    //    /// Get or set the collection of episodes.
    //    /// </summary>
    //    public Collection<TvdbEpisode> Episodes { get; set; }

    //    private bool detailsLoaded;
    //    private bool episodesLoaded;
    //    private bool actorsLoaded;
    //    private bool bannersLoaded;
    //    private string languageCodeLoaded;

    //    /// <summary>
    //    /// Initialize a new instance of the TvdbSeries class.
    //    /// </summary>
    //    public TvdbSeries() { }

    //    /// <summary>
    //    /// Load the extended data for the series for a language code.
    //    /// </summary>
    //    /// <param name="instance">An API instance.</param>
    //    /// <param name="languageCode">The language code.</param>
    //    public void LoadDetails(TvdbAPI instance, string languageCode)
    //    {
    //        if (detailsLoaded && languageCodeLoaded == languageCode)
    //            return;

    //        try
    //        {
    //            TvdbSeriesInfoResult seriesInfo;

    //            if (languageCode == null)
    //                seriesInfo = instance.GetSeriesDetails(Identity, null);
    //            else
    //                seriesInfo = instance.GetSeriesDetails(Identity, null, languageCode);

    //            Identity = seriesInfo.Series.Identity;
    //            SeriesName = seriesInfo.Series.SeriesName;
    //            Aliases = seriesInfo.Series.Aliases;
    //            Banner = seriesInfo.Series.Banner;
    //            SeriesId = seriesInfo.Series.SeriesId;
    //            Status = seriesInfo.Series.Status;
    //            FirstAired = seriesInfo.Series.FirstAired;
    //            Network = seriesInfo.Series.Network;
    //            NetworkId = seriesInfo.Series.NetworkId;
    //            Runtime = seriesInfo.Series.Runtime;
    //            Genres = seriesInfo.Series.Genres;
    //            Overview = seriesInfo.Series.Overview;
    //            LastUpdated = seriesInfo.Series.LastUpdated;
    //            AirsDayOfWeek = seriesInfo.Series.AirsDayOfWeek;
    //            AirsTime = seriesInfo.Series.AirsTime;
    //            ContentRating = seriesInfo.Series.ContentRating;
    //            ImdbID = seriesInfo.Series.ImdbID;
    //            Zap2ItID = seriesInfo.Series.Zap2ItID;
    //            AddedDate = seriesInfo.Series.AddedDate;
    //            AddedBy = seriesInfo.Series.AddedBy;
    //            Rating = seriesInfo.Series.Rating;

    //            detailsLoaded = true;
    //            languageCodeLoaded = languageCode;
    //        }
    //        catch (WebException e)
    //        {
    //            HttpWebResponse response = e.Response as HttpWebResponse;
    //            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
    //                return;
    //            throw e;
    //        }
    //    }

    //    /// <summary>
    //    /// Load the episodes in the series.
    //    /// </summary>
    //    /// <param name="instance">The API instance.</param>
    //    /// <param name="languageCode">The language code.</param>
    //    public void LoadEpisodes(TvdbAPI instance, string languageCode)
    //    {
    //        if (episodesLoaded && languageCodeLoaded == languageCode)
    //            return;

    //        Episodes = new Collection<TvdbEpisode>();

    //        try
    //        {
    //            TvdbSeriesEpisodesResult episodesInfo;

    //            if (languageCode == null)
    //                episodesInfo = instance.GetSeriesEpisodes(Identity, null);
    //            else
    //                episodesInfo = instance.GetSeriesEpisodes(Identity, null, languageCode);

    //            foreach (TvdbEpisode episode in episodesInfo.Episodes)
    //                addEpisode(episode, Episodes);

    //            while (episodesInfo.PageLinks.NextPageNumber != -1)
    //            {
    //                if (languageCode == null)
    //                    episodesInfo = instance.GetSeriesEpisodes(Identity, episodesInfo.PageLinks.NextPageNumber, null);
    //                else
    //                    episodesInfo = instance.GetSeriesEpisodes(Identity, episodesInfo.PageLinks.NextPageNumber, null, languageCode);

    //                foreach (TvdbEpisode episode in episodesInfo.Episodes)
    //                    addEpisode(episode, Episodes);
    //            }

    //            episodesLoaded = true;
    //            languageCodeLoaded = languageCode;
    //        }
    //        catch (WebException e)
    //        {
    //            HttpWebResponse response = e.Response as HttpWebResponse;
    //            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
    //                return;
    //            throw e;
    //        }
    //    }

    //    private void addEpisode(TvdbEpisode newEpisode, Collection<TvdbEpisode> episodes)
    //    {
    //        foreach (TvdbEpisode oldEpisode in episodes)
    //        {
    //            if (oldEpisode.SeasonNumber > newEpisode.SeasonNumber)
    //            {
    //                episodes.Insert(episodes.IndexOf(oldEpisode), newEpisode);
    //                return;
    //            }
    //            else
    //            {
    //                if (oldEpisode.EpisodeNumber > newEpisode.EpisodeNumber)
    //                {
    //                    episodes.Insert(episodes.IndexOf(oldEpisode), newEpisode);
    //                    return;
    //                }
    //            }
    //        }

    //        episodes.Add(newEpisode);
    //    }

    //    /// <summary>
    //    /// Load the series banners.
    //    /// </summary>
    //    /// <param name="instance">An API instance.</param>
    //    public void LoadBanners(TvdbAPI instance)
    //    {
    //        if (bannersLoaded)
    //            return;

    //        Banners = new Collection<TvdbBanner>();

    //        loadBanners(instance, Banners, TvdbAPI.ImageType.Poster);
    //        loadBanners(instance, Banners, TvdbAPI.ImageType.FanArt);
    //        loadBanners(instance, Banners, TvdbAPI.ImageType.Season);

    //        bannersLoaded = true;
    //    }

    //    private void loadBanners(TvdbAPI instance, Collection<TvdbBanner> banners, TvdbAPI.ImageType imageType)
    //    {
    //        try
    //        {
    //            TvdbBannersResult bannersResult = instance.GetSeriesBanners(Identity, imageType, null);
    //            if (bannersResult == null || bannersResult.Banners == null)
    //                return;

    //            foreach (TvdbBanner banner in bannersResult.Banners)
    //                banners.Add(banner);
    //        }
    //        catch (WebException e)
    //        {
    //            HttpWebResponse response = e.Response as HttpWebResponse;
    //            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
    //                return;
    //            throw e;
    //        }
    //    }

    //    /// <summary>
    //    /// Load the series actors.
    //    /// </summary>
    //    /// <param name="instance">An API instance.</param>
    //    public void LoadActors(TvdbAPI instance)
    //    {
    //        if (actorsLoaded)
    //            return;

    //        try
    //        {
    //            Actors = instance.GetSeriesActors(Identity, null).Actors;
    //        }
    //        catch (WebException e)
    //        {
    //            HttpWebResponse response = e.Response as HttpWebResponse;
    //            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
    //                return;
    //            throw e;
    //        }

    //        actorsLoaded = true;
    //    }

    //    /// <summary>
    //    /// Load all the data for the series.
    //    /// </summary>
    //    /// <param name="instance">The API instance.</param>
    //    /// <param name="languageCode">The language code.</param>
    //    public void LoadAllData(TvdbAPI instance, string languageCode)
    //    {
    //        LoadDetails(instance, languageCode);
    //        LoadEpisodes(instance, languageCode);
    //        LoadActors(instance);
    //        LoadBanners(instance);
    //    }

    //    /// <summary>
    //    /// Search for series given a title.
    //    /// </summary>
    //    /// <param name="instance">An API instance..</param>
    //    /// <param name="title">The title to search for.</param>
    //    /// <returns>The results object.</returns>
    //    public static TvdbSeriesSearchResult Search(TvdbAPI instance, string title)
    //    {
    //        return instance.GetSeries(title, null);
    //    }

    //    /// <summary>
    //    /// Search for a series in an specific language.
    //    /// </summary>
    //    /// <param name="instance">An API instance.</param>
    //    /// <param name="title">Part or all of the title.</param>
    //    /// <param name="languageCode">The language code.</param>
    //    /// <returns>The results object.</returns>
    //    public static TvdbSeriesSearchResult Search(TvdbAPI instance, string title, string languageCode)
    //    {
    //        return instance.GetSeries(title, null, languageCode);
    //    }

    //    /// <summary>
    //    /// Get the poster image.
    //    /// </summary>
    //    /// <param name="instance">An API instance.</param>
    //    /// <param name="fileName">The output filename.</param>
    //    /// <returns>True if the poster was downloaded; false otherwise.</returns>
    //    public bool GetPosterImage(TvdbAPI instance, string fileName)
    //    {
    //        TvdbBanner selectedBanner = findBanner(instance, TvdbAPI.ImageType.Poster, false);
    //        if (selectedBanner == null)
    //            return false;

    //        try
    //        {
    //            return instance.GetImage(TvdbAPI.ImageType.Poster, selectedBanner.FileName, 0, fileName);
    //        }
    //        catch (WebException e)
    //        {
    //            HttpWebResponse response = e.Response as HttpWebResponse;
    //            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
    //                return false;
    //            throw e;
    //        }      
    //    }

    //    /// <summary>
    //    /// Get the small poster image.
    //    /// </summary>
    //    /// <param name="instance">An API instance.</param>
    //    /// <param name="fileName">The name of the output file.</param>
    //    /// <returns>True if the image was downloaded; false otherwise.</returns>
    //    public bool GetSmallPosterImage(TvdbAPI instance, string fileName)
    //    {
    //        TvdbBanner selectedBanner = findBanner(instance, TvdbAPI.ImageType.Poster, true);
    //        if (selectedBanner == null)
    //            return false;

    //        try
    //        {
    //            return instance.GetImage(TvdbAPI.ImageType.Poster, selectedBanner.ThumbNail, 0, fileName);
    //        }
    //        catch (WebException e)
    //        {
    //            HttpWebResponse response = e.Response as HttpWebResponse;
    //            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
    //                return false;
    //            throw e;
    //        }   
    //    }

    //    /// <summary>
    //    /// Get the fan art image.
    //    /// </summary>
    //    /// <param name="instance">An API instance.</param>
    //    /// <param name="fileName">The output filename.</param>
    //    /// <returns>True if the image was downloaded; false otherwise.</returns>
    //    public bool GetFanArtImage(TvdbAPI instance, string fileName)
    //    {
    //        TvdbBanner selectedBanner = findBanner(instance, TvdbAPI.ImageType.FanArt, false);
    //        if (selectedBanner == null)
    //            return false;

    //        try
    //        {
    //            return instance.GetImage(TvdbAPI.ImageType.FanArt, selectedBanner.FileName, 0, fileName);
    //        }
    //        catch (WebException e)
    //        {
    //            HttpWebResponse response = e.Response as HttpWebResponse;
    //            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
    //                return false;
    //            throw e;
    //        }   
    //    }

    //    /// <summary>
    //    /// Get the small fan art image.
    //    /// </summary>
    //    /// <param name="instance">An API instance.</param>
    //    /// <param name="fileName">The output filename.</param>
    //    /// <returns>True if the image was downloaded; false otherwise.</returns>
    //    public bool GetSmallFanArtImage(TvdbAPI instance, string fileName)
    //    {
    //        TvdbBanner selectedBanner = findBanner(instance, TvdbAPI.ImageType.FanArt, true);
    //        if (selectedBanner == null)
    //            return false;

    //        try
    //        {
    //            return instance.GetImage(TvdbAPI.ImageType.FanArt, selectedBanner.ThumbNail, 0, fileName);
    //        }
    //        catch (WebException e)
    //        {
    //            HttpWebResponse response = e.Response as HttpWebResponse;
    //            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
    //                return false;
    //            throw e;
    //        }   
    //    }

    //    /// <summary>
    //    /// Get the banner image.
    //    /// </summary>
    //    /// <param name="instance">The API instance.</param>
    //    /// <param name="fileName">The name of the output file.</param>
    //    /// <returns>True if the banner was downloaded; false otherwise.</returns>
    //    public bool GetBannerImage(TvdbAPI instance, string fileName)
    //    {
    //        if (string.IsNullOrWhiteSpace(Banner))
    //            return false;

    //        return instance.GetImage(TvdbAPI.ImageType.Banner, Banner, 0, fileName);
    //    }

    //    /// <summary>
    //    /// Get a banner image.
    //    /// </summary>
    //    /// <param name="instance">The API instance.</param>
    //    /// <param name="index">The index of the image.</param>
    //    /// <param name="fileName">The name of the output file.</param>
    //    /// <returns>True if the banner was downloaded; false otherwise.</returns>
    //    public bool GetBannerImage(TvdbAPI instance, int index, string fileName)
    //    {
    //        LoadBanners(instance);

    //        if (Banners == null || Banners.Count == 0 || index == -1 || index >= Banners.Count)
    //            return false;

    //        if (string.IsNullOrWhiteSpace(Banners[index].FileName))
    //            return false;

    //        return instance.GetImage(TvdbAPI.ImageType.Banner, Banners[index].FileName, 0, fileName);            
    //    }

    //    private TvdbBanner findBanner(TvdbAPI instance, TvdbAPI.ImageType imageType, bool thumbnail)
    //    {
    //        LoadBanners(instance);

    //        if (Banners == null || Banners.Count == 0)
    //            return null;

    //        TvdbBanner selectedBanner = null;

    //        foreach (TvdbBanner banner in Banners)
    //        {
    //            if (banner.IsType(imageType))
    //            {
    //                if ((!thumbnail && banner.FileName != null) || (thumbnail && banner.ThumbNail != null))
    //                {
    //                    if (selectedBanner == null)
    //                        selectedBanner = banner;
    //                    else
    //                    {
    //                        if (selectedBanner.RatingsInfo != null)
    //                        {
    //                            if (selectedBanner.RatingsInfo == null)
    //                                selectedBanner = banner;
    //                            else
    //                            {
    //                                if (banner.RatingsInfo.Average > selectedBanner.RatingsInfo.Average)
    //                                    selectedBanner = banner;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        return selectedBanner;
    //    }
    //}
}
