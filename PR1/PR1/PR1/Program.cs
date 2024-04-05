namespace PR1
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        // Convert Cartesian to Polar
        public (double, double) ToPolar()
        {
            double r = Math.Sqrt(X * X + Y * Y);
            double theta = Math.Atan2(Y, X);
            return (r, theta);
        }

        // Convert Polar to Cartesian
        public static Point FromPolar(double r, double theta)
        {
            double x = r * Math.Cos(theta);
            double y = r * Math.Sin(theta);
            return new Point(x, y, 0);
        }

        // Convert Cartesian to Spherical
        public (double, double, double) ToSpherical()
        {
            double r = Math.Sqrt(X * X + Y * Y + Z * Z);
            double theta = Math.Atan2(Y, X);
            double phi = Math.Acos(Z / r);
            return (r, theta, phi);
        }

        // Convert Spherical to Cartesian
        public static Point FromSpherical(double r, double theta, double phi)
        {
            double x = r * Math.Sin(phi) * Math.Cos(theta);
            double y = r * Math.Sin(phi) * Math.Sin(theta);
            double z = r * Math.Cos(phi);
            return new Point(x, y, z);
        }

        // Calculate distance between two points in Cartesian coordinate system
        public static double DistanceCartesian(Point p1, Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            double dz = p2.Z - p1.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        // Calculate distance between two points in Polar coordinate system
        public static double DistancePolar((double, double) p1, (double, double) p2)
        {
            double dx = p2.Item1 * Math.Cos(p2.Item2) - p1.Item1 * Math.Cos(p1.Item2);
            double dy = p2.Item1 * Math.Sin(p2.Item2) - p1.Item1 * Math.Sin(p1.Item2);
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // Calculate distance between two points in Spherical coordinate system
        public static double DistanceSpherical((double, double, double) p1, (double, double, double) p2)
        {
            double dx = p2.Item1 * Math.Sin(p2.Item3) * Math.Cos(p2.Item2) - p1.Item1 * Math.Sin(p1.Item3) * Math.Cos(p1.Item2);
            double dy = p2.Item1 * Math.Sin(p2.Item3) * Math.Sin(p2.Item2) - p1.Item1 * Math.Sin(p1.Item3) * Math.Sin(p1.Item2);
            double dz = p2.Item1 * Math.Cos(p2.Item3) - p1.Item1 * Math.Cos(p1.Item3);
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Generate array of point pairs in 2D Cartesian coordinate system
            Point[] points2D = { new Point(1, 2, 0), new Point(3, 4, 0), new Point(-1, -1, 0) };

            // Convert to polar and back to Cartesian
            foreach (var point in points2D)
            {
                var polar = point.ToPolar();
                var backToCartesian = Point.FromPolar(polar.Item1, polar.Item2);
                Console.WriteLine($"Original: ({point.X}, {point.Y}), Polar: ({polar.Item1}, {polar.Item2}), Back to Cartesian: ({backToCartesian.X}, {backToCartesian.Y})");
            }

            // Generate array of point pairs in 3D Cartesian coordinate system
            Point[] points3D = { new Point(1, 2, 3), new Point(3, 4, 5), new Point(-1, -1, -1) };

            // Convert to spherical and back to Cartesian
            foreach (var point in points3D)
            {
                var spherical = point.ToSpherical();
                var backToCartesian = Point.FromSpherical(spherical.Item1, spherical.Item2, spherical.Item3);
                Console.WriteLine($"Original: ({point.X}, {point.Y}, {point.Z}), Spherical: ({spherical.Item1}, {spherical.Item2}, {spherical.Item3}), Back to Cartesian: ({backToCartesian.X}, {backToCartesian.Y}, {backToCartesian.Z})");
            }

            // Calculate distances between points in different coordinate systems
            for (int i = 0; i < points2D.Length; i++)
            {
                for (int j = i + 1; j < points2D.Length; j++)
                {
                    Console.WriteLine($"Distance between points {i} and {j} in Cartesian: {Point.DistanceCartesian(points2D[i], points2D[j])}");
                    Console.WriteLine($"Distance between points {i} and {j} in Polar: {Point.DistancePolar(points2D[i].ToPolar(), points2D[j].ToPolar())}");
                }
            }

            for (int i = 0; i < points3D.Length; i++)
            {
                for (int j = i + 1; j < points3D.Length; j++)
                {
                    Console.WriteLine($"Distance between points {i} and {j} in Cartesian: {Point.DistanceCartesian(points3D[i], points3D[j])}");
                    Console.WriteLine($"Distance between points {i} and {j} in Spherical: {Point.DistanceSpherical(points3D[i].ToSpherical(), points3D[j].ToSpherical())}");
                }
            }
        }
    }
}