using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type structure for performance result.
    /// </summary>
    public struct G9DtPerformanceResult
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="averageInfoOnSingleCore">Specifies the average information on the single-core.</param>
        /// <param name="averageInfoOnMultiCore">Specifies the average information on the multi-core.</param>
        /// <param name="numberOfRepetitions">Specifies the number of repetitions.</param>
        /// <param name="testMode">Specifies the mode of the test (single-core/multi-core)</param>
        public G9DtPerformanceResult(G9DtTuple<decimal> averageInfoOnSingleCore,
            G9DtTuple<decimal> averageInfoOnMultiCore,
            int numberOfRepetitions, G9EPerformanceTestMode testMode)
        {
            if (Equals(averageInfoOnSingleCore, default(G9DtTuple<decimal>)))
            {
                AverageExecutionTimeOnSingleCore = null;
                AverageMemoryUsageOnSingleCore = null;
            }
            else
            {
                AverageExecutionTimeOnSingleCore = averageInfoOnSingleCore.Item1;
                AverageMemoryUsageOnSingleCore = averageInfoOnSingleCore.Item2;
            }

            if (Equals(averageInfoOnMultiCore, default(G9DtTuple<decimal>)))
            {
                AverageExecutionTimeOnMultiCore = null;
                AverageMemoryUsageOnMultiCore = null;
            }
            else
            {
                AverageExecutionTimeOnMultiCore = averageInfoOnMultiCore.Item1;
                AverageMemoryUsageOnMultiCore = averageInfoOnMultiCore.Item2;
            }

            NumberOfRepetitions = numberOfRepetitions;
            TestMode = testMode;
        }

        /// <summary>
        ///     Specifies the average execution time on the single-core (In milliseconds).
        /// </summary>
        public readonly decimal? AverageExecutionTimeOnSingleCore;

        /// <summary>
        ///     Specifies the average memory usage on the single-core (In KiloByte).
        /// </summary>
        public readonly decimal? AverageMemoryUsageOnSingleCore;

        /// <summary>
        ///     Specifies the average execution time on the multi-core (In milliseconds).
        /// </summary>
        public readonly decimal? AverageExecutionTimeOnMultiCore;

        /// <summary>
        ///     Specifies the average memory usage on the multi-core (In KiloByte).
        /// </summary>
        public readonly decimal? AverageMemoryUsageOnMultiCore;

        /// <summary>
        ///     Specifies the number of repetitions (Performance test repetitions).
        /// </summary>
        public readonly int NumberOfRepetitions;

        /// <summary>
        ///     Specifies the mode of the test (single-core/multi-core)
        /// </summary>
        public readonly G9EPerformanceTestMode TestMode;
    }
}