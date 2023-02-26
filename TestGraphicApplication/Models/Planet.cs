namespace TestGraphicApplication.Models;

public class Planet
{
    private double Eccentricity { get; }
    private double SemiMajorAxis { get; }
    private double Inclination { get; }
    public double ArgumentOfPeriapsis { get; }
    public double LongitudeOfAscendingNode { get; }
    public double TrueAnomaly { get; set; }
    public Vector DistanceToSun { get; private set; }

    private double DistanceToSunLength =>
        SemiMajorAxis * (1 - Math.Pow(Eccentricity, 2) / (1 + Eccentricity * Math.Cos(TrueAnomaly)));
    

    public Planet(double eccentricity, double semiMajorAxis, double inclination, double argumentOfPeriapsis, double longitudeOfAscendingNode)
    {
        Eccentricity = eccentricity;
        SemiMajorAxis = semiMajorAxis;
        Inclination = inclination;
        ArgumentOfPeriapsis = argumentOfPeriapsis;
        LongitudeOfAscendingNode = longitudeOfAscendingNode;
        DistanceToSun = new Vector(0, 0, 0);
    }

    public void Step(double dt)
    {
        TrueAnomaly += Math.Sqrt(SemiMajorAxis*(1-Math.Pow(Eccentricity,2)))/Math.Pow(DistanceToSunLength,2)*dt;
        var c = ArgumentOfPeriapsis +TrueAnomaly;
        var b = Math.Asin(Math.Sin(Inclination)*Math.Sin(c));
        var l = LongitudeOfAscendingNode;
        if (Math.Sin(c) >= 0.0) l += Math.Acos(Math.Cos(c)/Math.Cos(b));
        else l += 2.0*Math.PI - Math.Acos(Math.Cos(c)/Math.Cos(b));
        if (l > 2.0*Math.PI) l -= 2.0*Math.PI;
        DistanceToSun = new Vector(DistanceToSunLength*Math.Cos(b)*Math.Cos(l), DistanceToSunLength*Math.Cos(b)*Math.Sin(l), DistanceToSunLength*Math.Sin(b));
        
    }
}