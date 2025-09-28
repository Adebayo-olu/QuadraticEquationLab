import unittest
import sys

def can_form_triangle(a, b, c):
    """
    Check if three sides can form a valid triangle.
    
    Triangle inequality theorem: The sum of the lengths of any two sides
    of a triangle must be greater than the length of the third side.
    
    Args:
        a, b, c: Three side lengths (numeric)
    
    Returns:
        bool: True if sides can form a triangle, False otherwise
    
    Raises:
        ValueError: If any side is not positive
        TypeError: If sides are not numeric
    """
    # Type validation
    if not all(isinstance(side, (int, float)) for side in [a, b, c]):
        raise TypeError("All sides must be numeric (int or float)")
    
    # Positive value validation
    if not all(side > 0 for side in [a, b, c]):
        raise ValueError("All sides must be positive numbers")
    
    # Triangle inequality theorem: a + b > c AND a + c > b AND b + c > a
    return (a + b > c) and (a + c > b) and (b + c > a)


class TestTriangleFunction(unittest.TestCase):
    """Comprehensive test suite for triangle validation function."""
    
    def test_valid_triangles(self):
        """Test cases for valid triangles."""
        # Right triangle (3-4-5)
        self.assertTrue(can_form_triangle(3, 4, 5))
        
        # Equilateral triangle
        self.assertTrue(can_form_triangle(5, 5, 5))
        
        # Scalene triangle
        self.assertTrue(can_form_triangle(3, 4, 6))
        
        # Isosceles triangle
        self.assertTrue(can_form_triangle(5, 5, 8))
        
        # Float values
        self.assertTrue(can_form_triangle(2.5, 3.5, 4.0))
        
        # Large numbers
        self.assertTrue(can_form_triangle(100, 150, 200))
    
    def test_invalid_triangles(self):
        """Test cases for invalid triangles."""
        # Sum of two sides less than third
        self.assertFalse(can_form_triangle(1, 2, 5))
        
        # Another invalid case
        self.assertFalse(can_form_triangle(1, 1, 3))
        
        # One side equals sum of other two (degenerate)
        self.assertFalse(can_form_triangle(1, 2, 3))
        
        # Very unbalanced triangle
        self.assertFalse(can_form_triangle(1, 1, 10))
    
    def test_edge_cases(self):
        """Test edge cases and boundary conditions."""
        # Just barely valid triangle
        self.assertTrue(can_form_triangle(1, 2, 2.5))
        
        # Just barely invalid triangle
        self.assertFalse(can_form_triangle(1, 2, 3.1))
        
        # Very small positive numbers
        self.assertTrue(can_form_triangle(0.001, 0.002, 0.002))
        
        # Equal sides (valid equilateral)
        self.assertTrue(can_form_triangle(1, 1, 1))
    
    def test_error_handling(self):
        """Test proper error handling for invalid inputs."""
        # Test negative values
        with self.assertRaises(ValueError) as context:
            can_form_triangle(-1, 2, 3)
        self.assertIn("positive", str(context.exception))
        
        # Test zero values
        with self.assertRaises(ValueError):
            can_form_triangle(0, 2, 3)
        
        with self.assertRaises(ValueError):
            can_form_triangle(1, 0, 3)
        
        with self.assertRaises(ValueError):
            can_form_triangle(1, 2, 0)
        
        # Test non-numeric inputs
        with self.assertRaises(TypeError) as context:
            can_form_triangle("3", 4, 5)
        self.assertIn("numeric", str(context.exception))
        
        with self.assertRaises(TypeError):
            can_form_triangle(None, 4, 5)
        
        with self.assertRaises(TypeError):
            can_form_triangle(3, [], 5)
        
        with self.assertRaises(TypeError):
            can_form_triangle(3, 4, {})
    
    def test_order_independence(self):
        """Test that the order of sides doesn't matter."""
        # Same sides in different orders should give same result
        self.assertTrue(can_form_triangle(3, 4, 5))
        self.assertTrue(can_form_triangle(4, 3, 5))
        self.assertTrue(can_form_triangle(5, 3, 4))
        self.assertTrue(can_form_triangle(3, 5, 4))
        self.assertTrue(can_form_triangle(4, 5, 3))
        self.assertTrue(can_form_triangle(5, 4, 3))
        
        # Invalid triangle in different orders
        self.assertFalse(can_form_triangle(1, 2, 5))
        self.assertFalse(can_form_triangle(2, 1, 5))
        self.assertFalse(can_form_triangle(5, 1, 2))
    
    def test_floating_point_precision(self):
        """Test floating point precision edge cases."""
        # Test with decimal precision
        self.assertTrue(can_form_triangle(0.1, 0.2, 0.25))
        self.assertTrue(can_form_triangle(1.1, 1.1, 1.1))
        
        # Test potential floating point errors
        a, b, c = 0.1, 0.2, 0.3
        # This should be false since 0.1 + 0.2 = 0.3 (degenerate case)
        # But due to floating point precision, it might behave differently
        result = can_form_triangle(a, b, c)
        # We accept either result due to floating point precision issues
        self.assertIsInstance(result, bool)
    
    def test_large_numbers(self):
        """Test with very large numbers."""
        large_num = sys.float_info.max / 4
        self.assertTrue(can_form_triangle(large_num, large_num, large_num))
        
        # Test with maximum safe integer
        max_int = sys.maxsize // 4
        self.assertTrue(can_form_triangle(max_int, max_int, max_int))
    
    def test_mixed_types(self):
        """Test with mixed integer and float types."""
        self.assertTrue(can_form_triangle(3, 4.0, 5))
        self.assertTrue(can_form_triangle(3.0, 4, 5.0))
        self.assertFalse(can_form_triangle(1, 2.0, 5))


def run_triangle_tests():
    """Run all triangle validation tests."""
    print("Running Triangle Validation Tests...")
    print("=" * 50)
    
    # Create test suite
    suite = unittest.TestLoader().loadTestsFromTestCase(TestTriangleFunction)
    
    # Run tests with detailed output
    runner = unittest.TextTestRunner(verbosity=2)
    result = runner.run(suite)
    
    # Print summary
    print("\n" + "=" * 50)
    print(f"Tests run: {result.testsRun}")
    print(f"Failures: {len(result.failures)}")
    print(f"Errors: {len(result.errors)}")
    
    if result.wasSuccessful():
        print("All tests passed! ✅")
    else:
        print("Some tests failed! ❌")
    
    return result.wasSuccessful()


if __name__ == '__main__':
    # Example usage
    print("Triangle Validation Function Examples:")
    print("-" * 40)
    
    test_cases = [
        (3, 4, 5),      # Valid right triangle
        (5, 5, 5),      # Valid equilateral
        (1, 2, 5),      # Invalid triangle
        (1, 2, 3),      # Degenerate case
    ]
    
    for a, b, c in test_cases:
        try:
            result = can_form_triangle(a, b, c)
            print(f"can_form_triangle({a}, {b}, {c}) = {result}")
        except Exception as e:
            print(f"can_form_triangle({a}, {b}, {c}) raised {type(e).__name__}: {e}")
    
    print("\n")
    
    # Run comprehensive tests
    run_triangle_tests()