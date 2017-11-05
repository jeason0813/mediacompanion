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
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Net;

namespace TheTvDB
{
    /// <summary>
    /// The class that describes an episode.
    /// </summary>
    [DataContract]
    public class TvdbEpisode
    {
        /// <summary>
        /// Get or set the episode ID.
        /// </summary>
        [DataMember(Name = "id")]
        public int Identity { get; set; }

        /// <summary>
        /// Get or set the season number.
        /// </summary>
        [DataMember(Name = "airedSeason")]
        public int SeasonNumber { get; set; }

        /// <summary>
        /// Get or set the episode number.
        /// </summary>
        [DataMember(Name = "airedEpisodeNumber")]
        public int EpisodeNumber { get; set; }

        /// <summary>
        /// Get or set the episode name.
        /// </summary>
        [DataMember(Name = "episodeName")]
        public string EpisodeName { get; set; }

        /// <summary>
        /// Get or set the first air date string.
        /// </summary>
        [DataMember(Name = "firstAired")]
        public string FirstAired { get; set; }

        /// <summary>
        /// Get or set the guest stars.
        /// </summary>
        [DataMember(Name = "guestStars")]
        public Collection<string> GuestStars { get; set; }

        /// <summary>
        /// Get or set the directors.
        /// </summary>
        [DataMember(Name = "director")]
        public string Directors { get; set; }

        /// <summary>
        /// Get or set the writers.
        /// </summary>
        [DataMember(Name = "writers")]
        public Collection<string> Writers { get; set; }

        /// <summary>
        /// Get or set the overview.
        /// </summary>
        [DataMember(Name = "overview")]
        public string Overview { get; set; }

        /// <summary>
        /// Get or set the overview.
        /// </summary>
        [DataMember(Name = "language")]
        public TvdbEpisodeLanguage Language { get; set; }

        /// <summary>
        /// Get or set the production code.
        /// </summary>
        [DataMember(Name = "productionCode")]
        public string ProductionCode { get; set; }

        /// <summary>
        /// Get or set the show URL.
        /// </summary>
        [DataMember(Name = "showUrl")]
        public string ShowUrl { get; set; }

        /// <summary>
        /// Get or set the last updated time.
        /// </summary>
        [DataMember(Name = "lastUpdated")]
        public long LastUpdated { get; set; }

        /// <summary>
        /// Get or set the DVD discID.
        /// </summary>
        [DataMember(Name = "dvdDiscid")]
        public string DVDDiscID { get; set; }

        /// <summary>
        /// Get or set the DVD season number.
        /// </summary>
        [DataMember(Name = "dvdSeason")]
        public string DVDSeason { get; set; }

        /// <summary>
        /// Get or set the DVD episode.
        /// </summary>
        [DataMember(Name = "dvdEpisodeNumber")]
        public string DVDEpisode { get; set; }
        
        /// <summary>
        /// Get or set the DVD chapter.
        /// </summary>
        [DataMember(Name = "dvdChapter")]
        public string DVDChapter { get; set; }

        /// <summary>
        /// Get or set the absolute number.
        /// </summary>
        [DataMember(Name = "absoluteNumber")]
        public string AbsoluteNumber { get; set; }

        /// <summary>
        /// Get or set the image path.
        /// </summary>
        [DataMember(Name = "filename")]
        public string Image { get; set; }

        /// <summary>
        /// Get or set the series ID.
        /// </summary>
        [DataMember(Name = "seriesId")]
        public int SeriesId { get; set; }

        /// <summary>
        /// Get or set the last updated by field.
        /// </summary>
        [DataMember(Name = "lastUpdatedBy")]
        public string LastUpdatedBy { get; set; }

        /// <summary>
        /// Get or set the airs after season field.
        /// </summary>
        [DataMember(Name = "airsAfterSeason")]
        public string AirsAfterSeason { get; set; }

        /// <summary>
        /// Get or set the airs before season field.
        /// </summary>
        [DataMember(Name = "airsBeforeSeason")]
        public string AirsBeforeSeason { get; set; }

        /// <summary>
        /// Get or set the airs before episode field.
        /// </summary>
        [DataMember(Name = "airsBeforeEpisode")]
        public string AirsBeforeEpisode { get; set; }

        /// <summary>
        /// Get or set the thumb author.
        /// </summary>
        [DataMember(Name = "thumbAuthor")]
        public string ThumbAuthor { get; set; }

        /// <summary>
        /// Get or set the thumb added date.
        /// </summary>
        [DataMember(Name = "thumbAdded")]
        public string ThumbAdded { get; set; }

        /// <summary>
        /// Get or set the thumb width.
        /// </summary>
        [DataMember(Name = "thumbWidth")]
        public string ThumbWidth { get; set; }

        /// <summary>
        /// Get or set the thumb height.
        /// </summary>
        [DataMember(Name = "thumbHeight")]
        public string ThumbHeight { get; set; }

        /// <summary>
        /// Get or set IMDB reference number.
        /// </summary>
        [DataMember(Name = "imdbId")]
        public string ImdbId { get; set; }

        /// <summary>
        /// Get or set the user rating.
        /// </summary>
        [DataMember(Name = "siteRating")]
        public decimal Rating { get; set; }

        /// <summary>
        /// Get or set the user votes
        /// </summary>
        [DataMember(Name = "siteRatingCount")]
        public int Votes { get; set; }

        /// <summary>
        /// Get the first air date. Returns null if not present or invalid.
        /// </summary>
        public DateTime? FirstAiredDate { get { return TvdbUtils.StringToDate(FirstAired); } }
        
        /// <summary>
        /// Get the dvd episode number. Returns -1 if not present or invalid.
        /// </summary>
        public int DVDEpisodeNumber { get { return TvdbUtils.StringToInt(DVDEpisode); } }        

        /// <summary>
        /// Get the dvd season number. Returns -1 if no previous page.
        /// </summary>
        public int DVDSeasonNumber { get { return TvdbUtils.StringToInt(DVDSeason); } }

        /// <summary>
        /// Get a comma separated string of guest stars.
        /// </summary>
        public string GuestStarsDisplayString
        {
            get
            {
                return TvdbUtils.CollectionToString(GuestStars);
            }
            set
            {
                Collection<string> collection = TvdbUtils.StringToCollection(value);
                GuestStars.Clear();
                foreach (string item in collection)
                    GuestStars.Add(item.Trim());
            }
        }

        /// <summary>
        /// Get a comma separated string of writers.
        /// </summary>
        public string WritersDisplayString
        {
            get
            {
                return TvdbUtils.CollectionToString(Writers);
            }
            set
            {
                Collection<string> collection = TvdbUtils.StringToCollection(value);
                Writers.Clear();
                foreach (string item in collection)
                    Writers.Add(item.Trim());
            }
        }

        /// <summary>
        /// Get the collection of director names.
        /// </summary>
        public Collection<string> DirectorsNames
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Directors))
                    return null;

                int startIndex = 0;
                int length = Directors.Length;

                if (Directors[0] == '|')
                {
                    startIndex = 1;
                    length--;
                }

                if (Directors.EndsWith("|"))
                    length--;

                string[] directorsNames = Directors.Substring(startIndex, length).Split(new char[] { '|' });

                Collection<string> directors = new Collection<string>();

                foreach (string directorsName in directorsNames)
                    directors.Add(directorsName);

                return directors;
            }
        }

        /// <summary>
        /// Get a comma separated string of directors.
        /// </summary>
        public string DirectorsDisplayString
        {
            get
            {
                return TvdbUtils.CollectionToString(DirectorsNames);
            }
            set
            {
                Collection<string> collection = TvdbUtils.StringToCollection(value);
                DirectorsNames.Clear();
                foreach (string item in collection)
                    DirectorsNames.Add(item.Trim());
            }
        }

        /// <summary>
        /// Get or set the collection of actors.
        /// </summary>
        public Collection<TvdbActor> Actors { get; set; }        

        private bool detailsLoaded;
        private bool actorsLoaded;
        private string languageCodeLoaded;        

        /// <summary>
        /// Initialize a new instance of the TvdbEpisode class.
        /// </summary>
        public TvdbEpisode() { }

        /// <summary>
        /// Load the extended data for an episode.
        /// </summary>
        /// <param name="instance">The API instance.</param>
        /// <param name="languageCode">The language code. May be null.</param>
        public void LoadDetails(TvdbAPI instance, string languageCode)
        {
            if (detailsLoaded && languageCodeLoaded == languageCode)
                return;

            try
            {
                TvdbEpisodeInfoResult episodeInfo;

                if (languageCode == null)
                    episodeInfo = instance.GetEpisodeDetails(Identity, null);
                else
                    episodeInfo = instance.GetEpisodeDetails(Identity, null, languageCode);

                Identity = episodeInfo.Episode.Identity;
                SeasonNumber = episodeInfo.Episode.SeasonNumber;
                EpisodeNumber = episodeInfo.Episode.EpisodeNumber;
                EpisodeName = episodeInfo.Episode.EpisodeName;
                FirstAired = episodeInfo.Episode.FirstAired;
                GuestStars = episodeInfo.Episode.GuestStars;
                Directors = episodeInfo.Episode.Directors;
                Writers = episodeInfo.Episode.Writers;
                Overview = episodeInfo.Episode.Overview;
                Language = episodeInfo.Episode.Language;
                ProductionCode = episodeInfo.Episode.ProductionCode;
                ShowUrl = episodeInfo.Episode.ShowUrl;
                LastUpdated = episodeInfo.Episode.LastUpdated;
                DVDDiscID = episodeInfo.Episode.DVDDiscID;
                DVDSeason = episodeInfo.Episode.DVDSeason;
                DVDEpisode = episodeInfo.Episode.DVDEpisode;
                DVDChapter = episodeInfo.Episode.DVDChapter;
                AbsoluteNumber = episodeInfo.Episode.AbsoluteNumber;
                Image = episodeInfo.Episode.Image;
                SeriesId = episodeInfo.Episode.SeriesId;
                LastUpdatedBy = episodeInfo.Episode.LastUpdatedBy;
                AirsAfterSeason = episodeInfo.Episode.AirsAfterSeason;
                AirsBeforeSeason = episodeInfo.Episode.AirsBeforeSeason;
                AirsBeforeEpisode = episodeInfo.Episode.AirsBeforeEpisode;
                ThumbAuthor = episodeInfo.Episode.ThumbAuthor;
                ThumbAdded = episodeInfo.Episode.ThumbAdded;
                ThumbWidth = episodeInfo.Episode.ThumbWidth;
                ThumbHeight = episodeInfo.Episode.ThumbHeight;
                ImdbId = episodeInfo.Episode.ImdbId;
                Rating = episodeInfo.Episode.Rating;

                detailsLoaded = true;
                languageCodeLoaded = languageCode;
            }
            catch (WebException e)
            {
                HttpWebResponse response = e.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    return;
                throw e;
            }
        }

        /// <summary>
        /// Get the actors for the episode.
        /// </summary>
        /// <param name="instance">An API instance.</param>
        public void LoadActors(TvdbAPI instance)
        {
            if (actorsLoaded)
                return;

            try
            {
                Actors = instance.GetEpisodeActors(Identity, null).Actors;
                actorsLoaded = true;
            }
            catch (WebException e)
            {
                HttpWebResponse response = e.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    return;
                throw e;
            }
        }

        /// <summary>
        /// Get the episode image.
        /// </summary>
        /// <param name="instance">An API instance.</param>
        /// <param name="fileName">The output filename.</param>
        /// <returns>True if the images is downloaded; false otherwise.</returns>
        public bool GetImage(TvdbAPI instance, string fileName)
        {
            if (string.IsNullOrWhiteSpace(Image))
                return false;

            return instance.GetImage(TvdbAPI.ImageType.Poster, Image, 0, fileName);
        }

        /// <summary>
        /// Get the small episode image.
        /// </summary>
        /// <param name="instance">An API instance.</param>
        /// <param name="fileName">The output filename.</param>
        /// <returns>True if the images is downloaded; false otherwise.</returns>
        public bool GetSmallImage(TvdbAPI instance, string fileName)
        {
            if (string.IsNullOrWhiteSpace(Image))
                return false;

            return instance.GetImage(TvdbAPI.ImageType.SmallPoster, Image, 0, fileName);
        }
    }
}
