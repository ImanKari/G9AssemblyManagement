using System.Collections.Generic;

namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     The data type for specifying the result of performance between several processes.
    /// </summary>
    public struct G9DtComparativePerformanceResults
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="singleCoreFastestProcessIndex">Specifies the index of the fastest process (single-core).</param>
        /// <param name="multiCoreFastestProcessIndex">Specifies the index of the fastest process (multi-core).</param>
        /// <param name="singleCoreMostOptimalMemoryUsage">
        ///     Specifies The index of the action in terms of the most optimal memory
        ///     usage (single-core).
        /// </param>
        /// <param name="multiCoreMostOptimalMemoryUsage">
        ///     Specifies The index of the action in terms of the most optimal memory
        ///     usage (multi-core).
        /// </param>
        /// <param name="performanceResults">Specifies the collection of performance results</param>
        /// <param name="singleCoreSortedPerformanceSpeedPercentageResults">
        ///     Specifies the sorted collection of performance speed
        ///     percentages. (Order by single-core result)
        /// </param>
        /// <param name="multiCoreSortedPerformanceSpeedPercentageResults">
        ///     Specifies the sorted collection of performance speed
        ///     percentages. (Order by multi-core result)
        /// </param>
        /// <param name="sortedPerformanceOfMemoryUsageForSingleCore">
        ///     Specifies the sorted collection of performance of memory
        ///     usage. (Order by single-core result)
        /// </param>
        /// <param name="sortedPerformanceOfMemoryUsageForMultiCore">
        ///     Specifies the sorted collection of performance of memory
        ///     usage. (Order by multi-core result)
        /// </param>
        public G9DtComparativePerformanceResults(int? singleCoreFastestProcessIndex, int? multiCoreFastestProcessIndex,
            int? singleCoreMostOptimalMemoryUsage, int? multiCoreMostOptimalMemoryUsage,
            IList<G9DtPerformanceResult> performanceResults,
            IList<G9DtPerformanceInformation> singleCoreSortedPerformanceSpeedPercentageResults,
            IList<G9DtPerformanceInformation> multiCoreSortedPerformanceSpeedPercentageResults,
            IList<G9DtPerformanceInformation> sortedPerformanceOfMemoryUsageForSingleCore,
            IList<G9DtPerformanceInformation> sortedPerformanceOfMemoryUsageForMultiCore)
        {
            SingleCoreFastestProcessIndex = singleCoreFastestProcessIndex;
            MultiCoreFastestProcessIndex = multiCoreFastestProcessIndex;
            PerformanceResults = performanceResults;
            SortedPerformanceSpeedPercentageForSingleCore = singleCoreSortedPerformanceSpeedPercentageResults;
            SortedPerformanceSpeedPercentageForMultiCore = multiCoreSortedPerformanceSpeedPercentageResults;
            SortedPerformanceOfMemoryUsageForSingleCore = sortedPerformanceOfMemoryUsageForSingleCore;
            SortedPerformanceOfMemoryUsageForMultiCore = sortedPerformanceOfMemoryUsageForMultiCore;
            SingleCoreMostOptimalMemoryUsage = singleCoreMostOptimalMemoryUsage;
            MultiCoreMostOptimalMemoryUsage = multiCoreMostOptimalMemoryUsage;
        }

        /// <summary>
        ///     Specifies the index of the fastest process (single-core).
        /// </summary>
        public readonly int? SingleCoreFastestProcessIndex;

        /// <summary>
        ///     Specifies the index of the fastest process (multi-core).
        /// </summary>
        public readonly int? MultiCoreFastestProcessIndex;

        /// <summary>
        ///     Specifies The index of the action in terms of the most optimal memory usage (single-core).
        /// </summary>
        public readonly int? SingleCoreMostOptimalMemoryUsage;

        /// <summary>
        ///     Specifies The index of the action in terms of the most optimal memory usage (multi-core).
        /// </summary>
        public readonly int? MultiCoreMostOptimalMemoryUsage;

        /// <summary>
        ///     Specifies the sorted collection of performance speed percentages. (Order by single-core result)
        /// </summary>
        public readonly IList<G9DtPerformanceInformation> SortedPerformanceSpeedPercentageForSingleCore;

        /// <summary>
        ///     Specifies the sorted collection of performance of memory usage. (Order by single-core result)
        /// </summary>
        public readonly IList<G9DtPerformanceInformation> SortedPerformanceOfMemoryUsageForSingleCore;

        /// <summary>
        ///     Specifies the sorted collection of performance speed percentages. (Order by multi-core result)
        /// </summary>
        public readonly IList<G9DtPerformanceInformation> SortedPerformanceSpeedPercentageForMultiCore;

        /// <summary>
        ///     Specifies the sorted collection of performance of memory usage. (Order by multi-core result)
        /// </summary>
        public readonly IList<G9DtPerformanceInformation> SortedPerformanceOfMemoryUsageForMultiCore;

        /// <summary>
        ///     Specifies the collection of performance results
        /// </summary>
        public readonly IList<G9DtPerformanceResult> PerformanceResults;
    }
}