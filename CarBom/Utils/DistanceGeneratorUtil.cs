namespace CarBom.Utils
{
    public static class DistanceGeneratorUtil
    {
        /// <summary>
        /// Retrives the distance between 2 locations (x, y) to (x, y) by respective latitude and longitude
        /// </summary>
        /// <param name="lat1">User Latitude</param>
        /// <param name="lon1">User Longitude</param>
        /// <param name="lat2">Origin Latitude (Mechanic)</param>
        /// <param name="lon2">Origin Longitude (Mechanic)</param>
        /// <returns></returns>
        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2)
        {
            //r = d*(π/180)
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;

            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;

            //a = [sen²(Δlat/2) + cos(lat1)] x cos(lat2) x sen²(Δlong/2)
            double dist = Math.Sin(rlat1) * Math.Sin(rlat2) +
                          Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);

            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;

            dist = (dist * 60) * 1.609344; //1.609344 To KM

            var distConverted = Math.Round(dist, 1);
            return distConverted;
        }
    }
}
