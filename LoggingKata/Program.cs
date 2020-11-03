using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {

            logger.LogInfo("Log initialized");
            
            var lines = File.ReadAllLines(csvPath);

            logger.LogInfo($"Lines: {lines[0]}");

            var parser = new TacoParser();

            var locations = lines.Select(parser.Parse).ToArray();

            ITrackable tacobell1 = new TacoBell();
            ITrackable tacobell2 = new TacoBell();

            double distanceBetween = 0;
            double longestDistance = 0;
            foreach (var locA in locations)
            {
                GeoCoordinate pinA = new GeoCoordinate();
                pinA.Latitude = locA.Location.Latitude;
                pinA.Longitude = locA.Location.Longitude;

                foreach (var locB in locations)
                {
                    GeoCoordinate pinB = new GeoCoordinate();
                    pinB.Longitude = locB.Location.Longitude;
                    pinB.Latitude = locB.Location.Latitude;
                    distanceBetween = pinA.GetDistanceTo(pinB);
                    if (distanceBetween > longestDistance)
                    {
                        longestDistance = distanceBetween;
                        tacobell1 = locA;
                        tacobell2 = locB;
                    }
                }

                    
            }
            Console.WriteLine($"Longest distance is: {longestDistance}.");
            Console.WriteLine($"The two TacoBells are {tacobell1.Name} and {tacobell2.Name}.");

        }
    }
}
