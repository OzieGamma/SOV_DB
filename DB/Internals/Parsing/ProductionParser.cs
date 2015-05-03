﻿using System;
using System.Collections.Generic;
using DB.Internals.ImportModels;
using DB.Models;

namespace DB.Internals.Parsing
{
    internal sealed class ProductionParser : ILineParser
    {
        public string FileName
        {
            get { return "Production"; }
        }

        public IEnumerable<object> Parse( string[] values )
        {
            int id = ParseUtility.Get( values[0], int.Parse, "ID" );
            string title = ParseUtility.Get( values[1], "Title" );
            int? year = ParseUtility.Map( values[2], int.Parse );
            string type = ParseUtility.Get( values[7], "Type" );
            var genre = ParseUtility.Map( values[8], ParseGenre );

            yield return new Production
            {
                Id = id,
                Title = title,
                Year = year,
                Genre = genre
            };

            switch ( type )
            {
                case "tv series":
                    var fromTo = ParseUtility.MapRef( values[6], ParseSeriesYears ) ?? new Tuple<int?, int?>( null, null );

                    yield return new Series
                    {
                        ProductionId = id,
                        BeginningYear = fromTo.Item1,
                        EndYear = fromTo.Item2
                    };
                    break;

                case "episode":
                    yield return new SeriesEpisode
                    {
                        ProductionId = id,
                        SeriesId = ParseUtility.Get( values[3], int.Parse, "SeriesID" ),
                        SeasonNumber = ParseUtility.Map( values[4], int.Parse ),
                        EpisodeNumber = ParseUtility.Map( values[5], int.Parse )
                    };
                    break;

                case "video game":
                    yield return new VideoGame
                    {
                        ProductionId = id
                    };
                    break;

                default:
                    yield return new Movie
                    {
                        ProductionId = id,
                        Type = ParseMovieType( type )
                    };
                    break;
            }
        }


        private static ProductionGenre ParseGenre( string genre )
        {
            switch ( genre )
            {
                case "Action":
                    return ProductionGenre.Action;

                case "Adventure":
                    return ProductionGenre.Adventure;

                case "Animation":
                    return ProductionGenre.Animation;

                case "Biography":
                    return ProductionGenre.Biography;

                case "Comedy":
                    return ProductionGenre.Comedy;

                case "Crime":
                    return ProductionGenre.Crime;

                case "Documentary":
                    return ProductionGenre.Documentary;

                case "Drama":
                    return ProductionGenre.Drama;

                case "Family":
                    return ProductionGenre.Family;

                case "Fantasy":
                    return ProductionGenre.Fantasy;

                case "Film-Noir":
                    return ProductionGenre.FilmNoir;

                case "Game-Show":
                    return ProductionGenre.GameShow;

                case "History":
                    return ProductionGenre.History;

                case "Horror":
                    return ProductionGenre.Horror;

                case "Music":
                case "Musical":
                    return ProductionGenre.Music;

                case "Mystery":
                    return ProductionGenre.Mystery;

                case "News":
                    return ProductionGenre.News;

                case "Reality-TV":
                    return ProductionGenre.RealityTV;

                case "Romance":
                    return ProductionGenre.Romance;

                case "Sci-Fi":
                    return ProductionGenre.SciFi;

                case "Short":
                    return ProductionGenre.Short;

                case "Sport":
                    return ProductionGenre.Sport;

                case "Talk-Show":
                    return ProductionGenre.TalkShow;

                case "Thriller":
                    return ProductionGenre.Thriller;

                case "War":
                    return ProductionGenre.War;

                case "Western":
                    return ProductionGenre.Western;

                default:
                    throw new InvalidOperationException( "Unknown production genre: " + genre );
            }
        }

        private static MovieType ParseMovieType( string type )
        {
            switch ( type )
            {
                case "movie":
                    return MovieType.Normal;

                case "video movie":
                    return MovieType.Video;

                case "tv movie":
                    return MovieType.TV;

                default:
                    throw new InvalidOperationException( "Unknown movie type: " + type );
            }
        }

        private static Tuple<int?, int?> ParseSeriesYears( string years )
        {
            if ( years == "????" )
            {
                return new Tuple<int?, int?>( null, null );
            }
            string[] split = years.Split( '-' );
            int? from = split[0] == "????" ? (int?) null : int.Parse( split[0] );
            int? to = split[1] == "????" ? (int?) null : int.Parse( split[1] );
            return Tuple.Create( from, to );
        }
    }
}