using System;

namespace QuadraticEquationSolver
{
    /// <summary>
    /// Represents the result of solving a quadratic equation
    /// </summary>
    public class QuadraticResult
    {
        public int RootCount { get; set; }
        public double Root1 { get; set; }
        public double Root2 { get; set; }
        public double Discriminant { get; set; }
        public string Description { get; set; }

        public QuadraticResult()
        {
            Root1 = double.NaN;
            Root2 = double.NaN;
        }
    }

    /// <summary>
    /// Solves quadratic equations of the form ax² + bx + c = 0
    /// </summary>
    public class QuadraticSolver
    {
        /// <summary>
        /// Solves the quadratic equation ax² + bx + c = 0
        /// </summary>
        /// <param name="a">Coefficient of x²</param>
        /// <param name="b">Coefficient of x</param>
        /// <param name="c">Constant term</param>
        /// <returns>QuadraticResult containing the solution details</returns>
        /// <exception cref="ArgumentException">Thrown when coefficient 'a' is zero</exception>
        public QuadraticResult Solve(double a, double b, double c)
        {
            if (Math.Abs(a) < double.Epsilon)
            {
                throw new ArgumentException("Coefficient 'a' cannot be zero for a quadratic equation", nameof(a));
            }

            var result = new QuadraticResult();
            
            // Calculate discriminant: b² - 4ac
            result.Discriminant = b * b - 4 * a * c;

            if (result.Discriminant > 0)
            {
                // Two distinct real roots
                result.RootCount = 2;
                double sqrtDiscriminant = Math.Sqrt(result.Discriminant);
                result.Root1 = (-b + sqrtDiscriminant) / (2 * a);
                result.Root2 = (-b - sqrtDiscriminant) / (2 * a);
                result.Description = "Two distinct real roots";
            }
            else if (Math.Abs(result.Discriminant) < double.Epsilon)
            {
                // One repeated real root
                result.RootCount = 1;
                result.Root1 = -b / (2 * a);
                result.Root2 = result.Root1; // Same root
                result.Description = "One repeated real root";
            }
            else
            {
                // No real roots (complex roots)
                result.RootCount = 0;
                result.Description = "No real roots (complex conjugate pair)";
                // For completeness, we could calculate complex roots
                // Real part: -b / (2 * a)
                // Imaginary part: ±√|discriminant| / (2 * a)
            }

            return result;
        }

        /// <summary>
        /// Validates that the given roots satisfy the original equation
        /// </summary>
        /// <param name="a">Coefficient of x²</param>
        /// <param name="b">Coefficient of x</param>
        /// <param name="c">Constant term</param>
        /// <param name="root">Root to validate</param>
        /// <param name="tolerance">Tolerance for floating-point comparison</param>
        /// <returns>True if the root is valid</returns>
        public bool ValidateRoot(double a, double b, double c, double root, double tolerance = 1e-10)
        {
            if (double.IsNaN(root))
                return false;

            double result = a * root * root + b * root + c;
            return Math.Abs(result) < tolerance;
        }

        /// <summary>
        /// Calculates the discriminant of a quadratic equation
        /// </summary>
        /// <param name="a">Coefficient of x²</param>
        /// <param name="b">Coefficient of x</param>
        /// <param name="c">Constant term</param>
        /// <returns>The discriminant value</returns>
        public double CalculateDiscriminant(double a, double b, double c)
        {
            return b * b - 4 * a * c;
        }
    }
}