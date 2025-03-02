using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace SamLab.Structural.Tests
{
    public class MathNetNumericTests
    {
        [Fact]
        public void BasicExample()
        {
            var A = Matrix<double>.Build.DenseOfArray(new double[,] {
                { 3, 2, -1 },
                { 2, -2, 4 },
                { -1, 0.5, -1 }
            });
            var b = Vector<double>.Build.Dense(new double[] { 1, -2, 0 });
            var x = A.Solve(b);

            Assert.Equal(1, x[0], 5);
            Assert.Equal(-2, x[1], 5);
            Assert.Equal(-2, x[2], 5);

        }
    }
}