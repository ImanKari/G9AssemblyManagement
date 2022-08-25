namespace G9AssemblyManagement.DataType
{
    /// <summary>
    ///     Data type structure for performance result.
    /// </summary>
    public readonly struct G9DtPerformanceInformation
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public G9DtPerformanceInformation(int actionIndexNumber, string actionCustomName,
            int anotherActionIndexNumberForComparison, string anotherCustomActionNameForComparison,
            decimal? singleCorePercentageOfPerformanceSpeed, decimal? multiCorePercentageOfPerformanceSpeed,
            decimal? singleCorePercentageOfOptimalMemoryUsage, decimal? multiCorePercentageOfOptimalMemoryUsage,
            G9DtPerformanceResult accessToPerformanceResult)
        {
            ActionIndexNumber = actionIndexNumber;
            ActionCustomName = actionCustomName;
            _anotherActionIndexNumberForComparison = anotherActionIndexNumberForComparison;
            _anotherCustomActionNameForComparison = anotherCustomActionNameForComparison;
            SingleCorePercentageOfPerformanceSpeed = singleCorePercentageOfPerformanceSpeed;
            MultiCorePercentageOfPerformanceSpeed = multiCorePercentageOfPerformanceSpeed;
            SingleCorePercentageOfOptimalMemoryUsage = singleCorePercentageOfOptimalMemoryUsage;
            MultiCorePercentageOfOptimalMemoryUsage = multiCorePercentageOfOptimalMemoryUsage;
            AccessToPerformanceResult = accessToPerformanceResult;
        }

        /// <summary>
        ///     Specifies the index of the action in the unsorted list
        /// </summary>
        public readonly int ActionIndexNumber;

        /// <summary>
        /// </summary>
        public readonly string ActionCustomName;

        /// <summary>
        ///     Specifies the percentage of performance speed than the previous test. (Single-Core)
        /// </summary>
        public readonly decimal? SingleCorePercentageOfPerformanceSpeed;

        /// <summary>
        ///     Specifies the percentage of performance speed than the previous test. (Single-Core)
        /// </summary>
        public readonly decimal? MultiCorePercentageOfPerformanceSpeed;

        /// <summary>
        ///     Specifies the percentage of optimal memory usage than the previous test. (Single-Core)
        /// </summary>
        public readonly decimal? SingleCorePercentageOfOptimalMemoryUsage;

        /// <summary>
        ///     Specifies the percentage of optimal memory usage than the previous test. (Multi-Core)
        /// </summary>
        public readonly decimal? MultiCorePercentageOfOptimalMemoryUsage;

        /// <summary>
        ///     Access to performance result
        /// </summary>
        public readonly G9DtPerformanceResult AccessToPerformanceResult;

        /// <summary>
        /// </summary>
        private readonly int _anotherActionIndexNumberForComparison;

        /// <summary>
        /// </summary>
        private readonly string _anotherCustomActionNameForComparison;

        /// <summary>
        ///     Access to a readable text for result
        /// </summary>
        public string ReadableResult => ToString();

        /// <inheritdoc />
        public override string ToString()
        {
            return
                SingleCorePercentageOfPerformanceSpeed != null && MultiCorePercentageOfPerformanceSpeed != null
                    ? $"The performance speed of action '{ActionCustomName}' (with index number '{ActionIndexNumber}') is {SingleCorePercentageOfPerformanceSpeed:##.#####}% (For single-core) and {MultiCorePercentageOfPerformanceSpeed:##.#####}% (For multi-core) higher/lower than action '{_anotherCustomActionNameForComparison}' (with index number '{_anotherActionIndexNumberForComparison}').\n" +
                      $"The optimal memory usage for this action is {SingleCorePercentageOfOptimalMemoryUsage:##.#####}% (For single-core) and {MultiCorePercentageOfOptimalMemoryUsage:##.#####}% (For multi-core) higher/lower than action '{_anotherCustomActionNameForComparison}' (with index number '{_anotherActionIndexNumberForComparison}')."
                    : SingleCorePercentageOfPerformanceSpeed != null
                        ? $"The performance speed of action '{ActionCustomName}' (with index number '{ActionIndexNumber}') is {SingleCorePercentageOfPerformanceSpeed:##.#####}% (For single-core) higher/lower than action '{_anotherCustomActionNameForComparison}' (with index number '{_anotherActionIndexNumberForComparison}').\n" +
                          $"The optimal memory usage for this action is {SingleCorePercentageOfOptimalMemoryUsage:##.#####}% (For single-core) higher/lower than action '{_anotherCustomActionNameForComparison}' (with index number '{_anotherActionIndexNumberForComparison}')."
                        : $"The performance speed of action '{ActionCustomName}' (with index number '{ActionIndexNumber}') is {MultiCorePercentageOfPerformanceSpeed:##.#####}% (For multi-core) higher/lower than action '{_anotherCustomActionNameForComparison}' (with index number '{_anotherActionIndexNumberForComparison}').\n" +
                          $"The optimal memory usage for this action is {MultiCorePercentageOfOptimalMemoryUsage:##.#####}% (For multi-core) higher/lower than action '{_anotherCustomActionNameForComparison}' (with index number '{_anotherActionIndexNumberForComparison}').";
        }
    }
}