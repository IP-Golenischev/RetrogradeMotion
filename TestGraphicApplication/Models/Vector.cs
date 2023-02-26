namespace TestGraphicApplication.Models;

public class Vector
{
    private double X { get; set; }
    private double Y { get; set; }
    private double Z { get; set; }
    private double Length3D => Math.Sqrt(X * X + Y * Y + Z * Z);
    private double Length2D => Math.Sqrt(X * X + Y * Y);

    public Vector(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double Beta => Math.Asin(Z / Length3D);
    public double Lambda => Y >= 0 ? Math.Acos(X / Length2D) : 2 * Math.PI - Math.Acos(X / Length2D);

    public static Vector operator -(Vector first, Vector second) =>
        new(second.X - first.X, second.Y - first.Y, second.Z - first.Z);
}