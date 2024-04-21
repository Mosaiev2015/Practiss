using System;
using System.Linq;
namespace PR3
{
    internal class Program
    {
        public static void Main()
        {
            var anchors = new (double, double)[] { (0, 0), (0, 10), (10, 0) };
            var objectCoordinates = (5.0, 5.0);
            var noiseVariance = 1.0;

            var distances = anchors.Select(anchor => EuclideanDistance(objectCoordinates.Item1, objectCoordinates.Item2, anchor.Item1, anchor.Item2)).ToArray();

            var angles = anchors.Select(anchor => CalculateAngle(objectCoordinates.Item1, objectCoordinates.Item2, anchor.Item1, anchor.Item2)).ToArray();

            var (trilateratedX, trilateratedY) = Trilateration(anchors, distances);

            var noisyDistances = AddNoise(distances, noiseVariance);

            var (noisyX, noisyY) = Trilateration(anchors, noisyDistances);

            var initialGuess = (1.0, 1.0);
            var learningRate = 0.01;
            var iterations = 1000;
            var (gradientX, gradientY) = GradientDescent(anchors, distances, initialGuess, learningRate, iterations);

            Console.WriteLine("Initial Data:");
            Console.WriteLine("Base Stations Coordinates: " + string.Join(", ", anchors));
            Console.WriteLine("Object Coordinates: " + objectCoordinates);

            Console.WriteLine("\nCalculations:");
            Console.WriteLine("Distances to Base Stations: " + string.Join(", ", distances));
            Console.WriteLine("Angles for Triangulation: " + string.Join(", ", angles));
            Console.WriteLine($"Trilateration Result (without noise): x = {trilateratedX:F2}, y = {trilateratedY:F2}");

            Console.WriteLine("\nExperiment with Noise:");
            Console.WriteLine("Noisy Distances: " + string.Join(", ", noisyDistances));
            Console.WriteLine($"Trilateration Result (with noise): x = {noisyX:F2}, y = {noisyY:F2}");

            Console.WriteLine("\nVarying Conditions:");
            Console.WriteLine("Changing Object Coordinates and Noise Variance:");
            objectCoordinates = (7, 3);
            var newDistances = anchors.Select(anchor => EuclideanDistance(objectCoordinates.Item1, objectCoordinates.Item2, anchor.Item1, anchor.Item2)).ToArray();
            var newNoisyDistances = AddNoise(newDistances, noiseVariance);
            var (newNoisyX, newNoisyY) = Trilateration(anchors, newNoisyDistances);
            Console.WriteLine("New Object Coordinates: " + objectCoordinates);
            Console.WriteLine("New Distances to Base Stations: " + string.Join(", ", newDistances));
            Console.WriteLine("New Noisy Distances: " + string.Join(", ", newNoisyDistances));
            Console.WriteLine($"Trilateration Result with New Object Coordinates and Noise Variance: x = {newNoisyX:F2}, y = {newNoisyY:F2}");
        }

        static double EuclideanDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        static double CalculateAngle(double x1, double y1, double x2, double y2)
        {
            return Math.Atan2(y2 - y1, x2 - x1);
        }

        static (double, double) Trilateration((double, double)[] anchors, double[] distances)
        {
            double x1 = anchors[0].Item1, y1 = anchors[0].Item2;
            double x2 = anchors[1].Item1, y2 = anchors[1].Item2;
            double x3 = anchors[2].Item1, y3 = anchors[2].Item2;

            double d1 = distances[0], d2 = distances[1], d3 = distances[2];

            double A = 2 * (x2 - x1);
            double B = 2 * (y2 - y1);
            double C = 2 * (x3 - x1);
            double D = 2 * (y3 - y1);

            double E = d1 * d1 - d2 * d2 - x1 * x1 + x2 * x2 - y1 * y1 + y2 * y2;
            double F = d1 * d1 - d3 * d3 - x1 * x1 + x3 * x3 - y1 * y1 + y3 * y3;

            double x = (E * D - F * B) / (A * D - B * C);
            double y = (F * A - E * C) / (A * D - B * C);

            return (x, y);
        }

        static double[] AddNoise(double[] distances, double variance)
        {
            var random = new Random();
            return distances.Select(distance => distance + Math.Sqrt(variance) * (random.NextDouble() - 0.5)).ToArray();
        }

        static (double, double) Gradient(double x, double y, (double, double)[] anchors, double[] distances)
        {
            double gradX = 0, gradY = 0;
            for (int i = 0; i < anchors.Length; i++)
            {
                double dx = x - anchors[i].Item1;
                double dy = y - anchors[i].Item2;
                double measuredDistance = distances[i];
                double calculatedDistance = EuclideanDistance(x, y, anchors[i].Item1, anchors[i].Item2);
                gradX += (calculatedDistance - measuredDistance) * (dx / calculatedDistance);
                gradY += (calculatedDistance - measuredDistance) * (dy / calculatedDistance);
            }
            return (gradX, gradY);
        }

        static (double, double) GradientDescent((double, double)[] anchors, double[] distances, (double, double) initialGuess, double learningRate, int iterations)
        {
            var (x, y) = initialGuess;
            for (int i = 0; i < iterations; i++)
            {
                var (gradX, gradY) = Gradient(x, y, anchors, distances);
                x -= learningRate * gradX;
                y -= learningRate * gradY;
            }
            return (x, y);
        }
    }
}
