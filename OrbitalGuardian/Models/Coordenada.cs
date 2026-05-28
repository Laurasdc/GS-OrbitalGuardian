namespace OrbitalGuardian.Models
{
    public struct Coordenada
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public Coordenada(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            return $"Latitude: {Latitude}, Longitude: {Longitude}";
        }
    }
}