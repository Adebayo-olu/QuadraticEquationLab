using System;
using Xunit;
using QuadraticEquationSolver;

namespace QuadraticEquationSolver.Tests
{
    public class QuadraticSolverTests
    {
        private readonly QuadraticSolver _solver;

        public QuadraticSolverTests()
        {
            _solver = new QuadraticSolver();
        }

        #region Two Real Roots Tests

        [Fact]
        public void Solve_TwoDistinctRealRoots_ShouldReturnCorrectRoots()
        {
            // Arrange
            double a = 1, b = -5, c = 6; // x² - 5x + 6 = 0, roots: 2, 3

            // Act
            var result = _solver.Solve(a, b, c);

            // Assert
            Assert.Equal(2, result.RootCount);
            Assert.True(result.Discriminant > 0);
            Assert.Equal("Two distinct real roots", result.Description);
            
            // Check roots (order may vary)
            var roots = new[] { result.Root1, result.Root2 };
            Array.Sort(roots);
            Assert.Equal(2.0, roots[0], 10);
            Assert.Equal(3.0, roots[1], 10);
        }

        [Theory]
        [InlineData(1, -3, 2, 1.0, 2.0)]      // x² - 3x + 2 = 0
        [InlineData(1, 0, -4, -2.0, 2.0)]     // x² - 4 = 0
        [InlineData(2, -8, 6, 1.0, 3.0)]      // 2x² - 8x + 6 = 0
        [InlineData(1, -7, 10, 2.0, 5.0)]     // x² - 7x + 10 = 0
        public void Solve_TwoRealRoots_TheoryData(double a, double b, double c, double expectedRoot1, double expectedRoot2)
        {
            // Act
            var result = _solver.Solve(a, b, c);

            // Assert
            Assert.Equal(2, result.RootCount);
            Assert.True(result.Discriminant > 0);
            
            // Validate roots satisfy the equation
            Assert.True(_solver.ValidateRoot(a, b, c, result.Root1));
            Assert.True(_solver.ValidateRoot(a, b, c, result.Root2));
            
            // Check expected values (roots may be in different order)
            var actualRoots = new[] { result.Root1, result.Root2 };
            var expectedRoots = new[] { expectedRoot1, expectedRoot2 };
            Array.Sort(actualRoots);
            Array.Sort(expectedRoots);
            
            Assert.Equal(expectedRoots[0], actualRoots[0], 10);
            Assert.Equal(expectedRoots[1], actualRoots[1], 10);
        }

        #endregion

        #region One Real Root Tests

        [Fact]
        public void Solve_OneRepeatedRoot_ShouldReturnSingleRoot()
        {
            // Arrange
            double a = 1, b = -4, c = 4; // x² - 4x + 4 = 0, root: 2 (repeated)

            // Act
            var result = _solver.Solve(a, b, c);

            // Assert
            Assert.Equal(1, result.RootCount);
            Assert.Equal(0, result.Discriminant, 10);
            Assert.Equal("One repeated real root", result.Description);
            Assert.Equal(2.0, result.Root1, 10);
            Assert.Equal(result.Root1, result.Root2); // Both roots should be the same
        }

        [Theory]
        [InlineData(1, -6, 9, 3.0)]          // x² - 6x + 9 = 0, root: 3
        [InlineData(4, -4, 1, 0.5)]          // 4x² - 4x + 1 = 0, root: 0.5
        [InlineData(1, 2, 1, -1.0)]          // x² + 2x + 1 = 0, root: -1
        [InlineData(9, -6, 1, 1.0/3.0)]      // 9x² - 6x + 1 = 0, root: 1/3
        public void Solve_OneRoot_TheoryData(double a, double b, double c, double expectedRoot)
        {
            // Act
            var result = _solver.Solve(a, b, c);

            // Assert
            Assert.Equal(1, result.RootCount);
            Assert.Equal(0, result.Discriminant, 12);
            Assert.Equal(expectedRoot, result.Root1, 10);
            Assert.Equal(result.Root1, result.Root2);
            
            // Validate root satisfies the equation
            Assert.True(_solver.ValidateRoot(a, b, c, result.Root1));
        }

        #endregion

        #region No Real Roots Tests

        [Fact]
        public void Solve_NoRealRoots_ShouldReturnZeroRootCount()
        {
            // Arrange
            double a = 1, b = 0, c = 1; // x² + 1 = 0, no real roots

            // Act
            var result = _solver.Solve(a, b, c);

            // Assert
            Assert.Equal(0, result.RootCount);
            Assert.True(result.Discriminant < 0);
            Assert.Equal("No real roots (complex conjugate pair)", result.Description);
            Assert.True(double.IsNaN(result.Root1));
            Assert.True(double.IsNaN(result.Root2));
        }

        [Theory]
        [InlineData(1, 0, 5)]                 // x² + 5 = 0
        [InlineData(2, 1, 3)]                 // 2x² + x + 3 = 0
        [InlineData(1, 2, 5)]                 // x² + 2x + 5 = 0
        [InlineData(3, -2, 4)]                // 3x² - 2x + 4 = 0
        public void Solve_NoRealRoots_TheoryData(double a, double b, double c)
        {
            // Act
            var result = _solver.Solve(a, b, c);

            // Assert
            Assert.Equal(0, result.RootCount);
            Assert.True(result.Discriminant < 0);
            Assert.True(double.IsNaN(result.Root1));
            Assert.True(double.IsNaN(result.Root2));
        }

        #endregion

        #region Edge Cases and Error Handling

        [Fact]
        public void Solve_CoefficientAIsZero_ShouldThrowArgumentException()
        {
            // Arrange
            double a = 0, b = 1, c = 1;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _solver.Solve(a, b, c));
            Assert.Contains("Coefficient 'a' cannot be zero", exception.Message);
            Assert.Equal("a", exception.ParamName);
        }

        [Theory]
        [InlineData(double.Epsilon / 2, 1, 1)]    // Very small positive a
        [InlineData(-double.Epsilon / 2, 1, 1)]   // Very small negative a
        public void Solve_CoefficientAVeryCloseToZero_ShouldThrowArgumentException(double a, double b, double c)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _solver.Solve(a, b, c));
        }

        [Theory]
        [InlineData(1e10, 1e5, 1)]            // Large coefficients
        [InlineData(1e-10, 1e-5, 1e-15)]      // Small coefficients
        [InlineData(-1, -2, -3)]               // All negative coefficients
        public void Solve_ExtremeValues_ShouldHandleGracefully(double a, double b, double c)
        {
            // Act
            var result = _solver.Solve(a, b, c);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.RootCount >= 0 && result.RootCount <= 2);
            
            // If roots exist, they should satisfy the equation
            if (result.RootCount > 0 && !double.IsNaN(result.Root1))
            {
                Assert.True(_solver.ValidateRoot(a, b, c, result.Root1, 1e-8));
            }
            if (result.RootCount == 2 && !double.IsNaN(result.Root2))
            {
                Assert.True(_solver.ValidateRoot(a, b, c, result.Root2, 1e-8));
            }
        }

        #endregion

        #region Discriminant Tests

        [Theory]
        [InlineData(1, -5, 6, 1)]             // b² - 4ac = 25 - 24 = 1
        [InlineData(1, -4, 4, 0)]             // b² - 4ac = 16 - 16 = 0
        [InlineData(1, 0, 1, -4)]             // b² - 4ac = 0 - 4 = -4
        [InlineData(2, -3, 1, 1)]             // b² - 4ac = 9 - 8 = 1
        public void CalculateDiscriminant_VariousInputs_ShouldReturnCorrectValue(double a, double b, double c, double expected)
        {
            // Act
            var discriminant = _solver.CalculateDiscriminant(a, b, c);

            // Assert
            Assert.Equal(expected, discriminant, 10);
        }

        #endregion

        #region Root Validation Tests

        [Theory]
        [InlineData(1, -5, 6, 2.0, true)]     // Valid root
        [InlineData(1, -5, 6, 3.0, true)]     // Valid root
        [InlineData(1, -5, 6, 1.0, false)]    // Invalid root
        [InlineData(1, -4, 4, 2.0, true)]     // Valid repeated root
        public void ValidateRoot_VariousRoots_ShouldReturnCorrectValidation(double a, double b, double c, double root, bool expected)
        {
            // Act
            var isValid = _solver.ValidateRoot(a, b, c, root);

            // Assert
            Assert.Equal(expected, isValid);
        }

        [Fact]
        public void ValidateRoot_NaNRoot_ShouldReturnFalse()
        {
            // Act
            var isValid = _solver.ValidateRoot(1, 2, 3, double.NaN);

            // Assert
            Assert.False(isValid);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void Solve_CompleteWorkflow_ShouldProduceConsistentResults()
        {
            // Arrange - Test multiple equations in sequence
            var testCases = new[]
            {
                new { a = 1.0, b = -5.0, c = 6.0, expectedRootCount = 2 },
                new { a = 1.0, b = -4.0, c = 4.0, expectedRootCount = 1 },
                new { a = 1.0, b = 0.0, c = 1.0, expectedRootCount = 0 }
            };

            foreach (var testCase in testCases)
            {
                // Act
                var result = _solver.Solve(testCase.a, testCase.b, testCase.c);
                var discriminant = _solver.CalculateDiscriminant(testCase.a, testCase.b, testCase.c);

                // Assert
                Assert.Equal(testCase.expectedRootCount, result.RootCount);
                Assert.Equal(discriminant, result.Discriminant, 10);
                
                // Validate roots if they exist
                if (result.RootCount > 0 && !double.IsNaN(result.Root1))
                {
                    Assert.True(_solver.ValidateRoot(testCase.a, testCase.b, testCase.c, result.Root1));
                }
                if (result.RootCount == 2 && !double.IsNaN(result.Root2))
                {
                    Assert.True(_solver.ValidateRoot(testCase.a, testCase.b, testCase.c, result.Root2));
                }
            }
        }

        #endregion
    }
}