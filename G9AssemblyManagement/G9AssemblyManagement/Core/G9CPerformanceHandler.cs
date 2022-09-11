using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;
#if NET35 || NET40
#endif

#if !NET35
using System.Threading.Tasks;
#endif

namespace G9AssemblyManagement.Core
{
    /// <summary>
    ///     Helper class for performance
    /// </summary>
    internal static class G9CPerformanceHandler
    {
        /// <summary>
        ///     Method to test the performance of several processes.
        /// </summary>
        /// <param name="testMode">Specifies the mode of the test (single-core/multi-core)</param>
        /// <param name="numberOfRepetitions">Specifies the number of repetitions (Performance test repetitions)</param>
        /// <param name="customActionsForTest">Specifies the several actions for testing comparative performance</param>
        /// <returns>A data type for specifying the information of performance.</returns>
        /// <exception>If any exception occurs in the testing process, it's thrown back.</exception>
        /// <exception cref="ArgumentException">
        ///     The parameter 'customActionsForTest' can't be null and must have at least two
        ///     actions.
        /// </exception>
        /// <exception cref="ArgumentException">The one or few actions included in parameter 'customActionsForTest' is null.</exception>
        public static G9DtComparativePerformanceResults ComparativePerformanceTester(
            G9EPerformanceTestMode testMode = G9EPerformanceTestMode.Both, int numberOfRepetitions = 999,
            params Action[] customActionsForTest)
        {
            if (customActionsForTest == null || customActionsForTest.Length < 2)
                throw new ArgumentException(
                    $"The parameter '{nameof(customActionsForTest)}' can't be null and must have at least two actions.");

            if (customActionsForTest.Any(s => s.Equals(null)))
                throw new ArgumentException(
                    $"The one or few actions included in parameter '{nameof(customActionsForTest)}' is null.");

            return ComparativePerformanceTester(testMode, numberOfRepetitions,
                customActionsForTest.Select((t, i) => new G9DtCustomPerformanceAction($"Action {i}", t))
                    .ToArray());
        }

        /// <summary>
        ///     Method to test the performance of several processes.
        /// </summary>
        /// <param name="testMode">Specifies the mode of the test (single-core/multi-core)</param>
        /// <param name="numberOfRepetitions">Specifies the number of repetitions (Performance test repetitions)</param>
        /// <param name="customActionsForTest">Specifies the several actions for testing comparative performance</param>
        /// <returns>A data type for specifying the information of performance.</returns>
        /// <exception>If any exception occurs in the testing process, it's thrown back.</exception>
        /// <exception cref="ArgumentException">
        ///     The parameter 'customActionsForTest' can't be null and must have at least two
        ///     actions.
        /// </exception>
        /// <exception cref="ArgumentException">The one or few actions included in parameter 'customActionsForTest' is null.</exception>
        public static G9DtComparativePerformanceResults ComparativePerformanceTester(
            G9EPerformanceTestMode testMode = G9EPerformanceTestMode.Both, int numberOfRepetitions = 999,
            params G9DtCustomPerformanceAction[] customActionsForTest)
        {
            if (customActionsForTest == null || customActionsForTest.Length < 2)
                throw new ArgumentException(
                    $"The parameter '{nameof(customActionsForTest)}' can't be null and must have at least two actions.");

            if (customActionsForTest.Any(s =>
                    s.Equals(null) || s.CustomAction.Equals(null) || string.IsNullOrEmpty(s.CustomName)))
                throw new ArgumentException(
                    $"The one or few actions included in parameter '{nameof(customActionsForTest)}' is null.");

            // Create a collection of performance results
            var performanceResults = customActionsForTest
                .Select(action => new
                {
                    Name = action.CustomName,
                    Result = PerformanceTester(action.CustomAction, testMode, numberOfRepetitions / 2)
                }).Join(customActionsForTest
                        .Reverse()
                        .Select(action => new
                        {
                            Name = action.CustomName,
                            Result = PerformanceTester(action.CustomAction, testMode,
                                numberOfRepetitions / 2 + numberOfRepetitions % 2)
                        }), a => a.Name, b => b.Name,
                    (a, b) => new G9DtPerformanceResult(
                        a.Result.AverageExecutionTimeOnSingleCore == null
                            ? default
                            : new G9DtTuple<decimal>(a.Result.AverageExecutionTimeOnSingleCore.Value +
                                                     // ReSharper disable once PossibleInvalidOperationException
                                                     b.Result.AverageExecutionTimeOnSingleCore.Value,
                                // ReSharper disable once PossibleInvalidOperationException
                                a.Result.AverageMemoryUsageOnSingleCore.Value +
                                // ReSharper disable once PossibleInvalidOperationException
                                b.Result.AverageMemoryUsageOnSingleCore.Value),
                        a.Result.AverageExecutionTimeOnMultiCore == null
                            ? default
                            : new G9DtTuple<decimal>(a.Result.AverageExecutionTimeOnMultiCore.Value +
                                                     // ReSharper disable once PossibleInvalidOperationException
                                                     b.Result.AverageExecutionTimeOnMultiCore.Value,
                                // ReSharper disable once PossibleInvalidOperationException
                                a.Result.AverageMemoryUsageOnMultiCore.Value +
                                // ReSharper disable once PossibleInvalidOperationException
                                b.Result.AverageMemoryUsageOnMultiCore.Value),
                        numberOfRepetitions,
                        testMode))
                .ToArray();

            // Order by results by single-core and multi-core result
            var singleCoreResult = performanceResults.Select((s, i) =>
                    new { ActionIndex = i, ActionName = customActionsForTest[i].CustomName, result = s })
                .OrderBy(s => s.result.AverageExecutionTimeOnSingleCore).ToArray();

            var singleCoreMemoryUsageResult = performanceResults.Select((s, i) =>
                    new { ActionIndex = i, ActionName = customActionsForTest[i].CustomName, result = s })
                .OrderBy(s => s.result.AverageMemoryUsageOnSingleCore).ToArray();

            var multiCoreResult = performanceResults.Select((s, i) =>
                    new { ActionIndex = i, ActionName = customActionsForTest[i].CustomName, result = s })
                .OrderBy(s => s.result.AverageExecutionTimeOnMultiCore).ToArray();

            var multiCoreMemoryUsageResult = performanceResults.Select((s, i) =>
                    new { ActionIndex = i, ActionName = customActionsForTest[i].CustomName, result = s })
                .OrderBy(s => s.result.AverageMemoryUsageOnMultiCore).ToArray();

            // Create data type by results
            IList<G9DtPerformanceInformation> singleCorePerformancePercentages = new List<G9DtPerformanceInformation>();
            for (var i = 0; i < singleCoreResult.Length - 1; i++)
                singleCorePerformancePercentages.Add(new G9DtPerformanceInformation(singleCoreResult[i].ActionIndex,
                    singleCoreResult[i].ActionName, singleCoreResult[i + 1].ActionIndex,
                    singleCoreResult[i + 1].ActionName
                    , CalculatePercentage(singleCoreResult[i + 1].result.AverageExecutionTimeOnSingleCore, singleCoreResult[i].result.AverageExecutionTimeOnSingleCore),
                    CalculatePercentage(singleCoreResult[i + 1].result.AverageExecutionTimeOnMultiCore,
                        singleCoreResult[i].result.AverageExecutionTimeOnMultiCore),
                    CalculatePercentage(singleCoreResult[i + 1].result.AverageMemoryUsageOnSingleCore,
                        singleCoreResult[i].result.AverageMemoryUsageOnSingleCore),
                    CalculatePercentage(singleCoreResult[i + 1].result.AverageMemoryUsageOnMultiCore,
                        singleCoreResult[i].result.AverageMemoryUsageOnMultiCore), singleCoreResult[i].result
                ));

            IList<G9DtPerformanceInformation> singleCorePerformanceMemoryUsage = new List<G9DtPerformanceInformation>();
            for (var i = 0; i < singleCoreMemoryUsageResult.Length - 1; i++)
                singleCorePerformanceMemoryUsage.Add(new G9DtPerformanceInformation(
                    singleCoreMemoryUsageResult[i].ActionIndex,
                    singleCoreMemoryUsageResult[i].ActionName, singleCoreMemoryUsageResult[i + 1].ActionIndex,
                    singleCoreMemoryUsageResult[i + 1].ActionName
                    , CalculatePercentage(singleCoreMemoryUsageResult[i + 1].result.AverageExecutionTimeOnSingleCore, singleCoreMemoryUsageResult[i].result.AverageExecutionTimeOnSingleCore),
                    CalculatePercentage(singleCoreMemoryUsageResult[i + 1].result.AverageExecutionTimeOnMultiCore,
                        singleCoreMemoryUsageResult[i].result.AverageExecutionTimeOnMultiCore),
                    CalculatePercentage(singleCoreMemoryUsageResult[i + 1].result.AverageMemoryUsageOnSingleCore,
                        singleCoreMemoryUsageResult[i].result.AverageMemoryUsageOnSingleCore),
                    CalculatePercentage(singleCoreMemoryUsageResult[i + 1].result.AverageMemoryUsageOnMultiCore,
                        singleCoreMemoryUsageResult[i].result.AverageMemoryUsageOnMultiCore),
                    singleCoreMemoryUsageResult[i].result
                ));

            IList<G9DtPerformanceInformation> multiCorePerformancePercentages = new List<G9DtPerformanceInformation>();
            for (var i = 0; i < multiCoreResult.Length - 1; i++)
                multiCorePerformancePercentages.Add(new G9DtPerformanceInformation(multiCoreResult[i].ActionIndex,
                    multiCoreResult[i].ActionName, multiCoreResult[i + 1].ActionIndex,
                    multiCoreResult[i + 1].ActionName
                    , CalculatePercentage(multiCoreResult[i + 1].result.AverageExecutionTimeOnSingleCore, multiCoreResult[i].result.AverageExecutionTimeOnSingleCore),
                    CalculatePercentage(multiCoreResult[i + 1].result.AverageExecutionTimeOnMultiCore,
                        multiCoreResult[i].result.AverageExecutionTimeOnMultiCore),
                    CalculatePercentage(multiCoreResult[i + 1].result.AverageMemoryUsageOnSingleCore,
                        multiCoreResult[i].result.AverageMemoryUsageOnSingleCore),
                    CalculatePercentage(multiCoreResult[i + 1].result.AverageMemoryUsageOnMultiCore,
                        multiCoreResult[i].result.AverageMemoryUsageOnMultiCore), multiCoreResult[i].result));

            IList<G9DtPerformanceInformation> multiCorePerformanceMemoryUsage = new List<G9DtPerformanceInformation>();
            for (var i = 0; i < multiCoreMemoryUsageResult.Length - 1; i++)
                multiCorePerformanceMemoryUsage.Add(new G9DtPerformanceInformation(
                    multiCoreMemoryUsageResult[i].ActionIndex,
                    multiCoreMemoryUsageResult[i].ActionName, multiCoreMemoryUsageResult[i + 1].ActionIndex,
                    multiCoreMemoryUsageResult[i + 1].ActionName
                    , CalculatePercentage(multiCoreMemoryUsageResult[i + 1].result.AverageExecutionTimeOnSingleCore, multiCoreMemoryUsageResult[i].result.AverageExecutionTimeOnSingleCore),
                    CalculatePercentage(multiCoreMemoryUsageResult[i + 1].result.AverageExecutionTimeOnMultiCore,
                        multiCoreMemoryUsageResult[i].result.AverageExecutionTimeOnMultiCore),
                    CalculatePercentage(multiCoreMemoryUsageResult[i + 1].result.AverageMemoryUsageOnSingleCore,
                        multiCoreMemoryUsageResult[i].result.AverageMemoryUsageOnSingleCore),
                    CalculatePercentage(multiCoreMemoryUsageResult[i + 1].result.AverageMemoryUsageOnMultiCore,
                        multiCoreMemoryUsageResult[i].result.AverageMemoryUsageOnMultiCore),
                    multiCoreMemoryUsageResult[i].result));


            return new G9DtComparativePerformanceResults(singleCoreResult[0].ActionIndex,
                multiCoreResult[0].ActionIndex, multiCoreMemoryUsageResult[0].ActionIndex,
                singleCoreMemoryUsageResult[0].ActionIndex, performanceResults, singleCorePerformancePercentages,
                multiCorePerformancePercentages, singleCorePerformanceMemoryUsage, multiCorePerformanceMemoryUsage);
        }

        /// <summary>
        ///     Method to calculate the different percentage between two numbers
        /// </summary>
        private static decimal? CalculatePercentage(decimal? firstNumber, decimal? secondNumber)
        {
            if (firstNumber == null || secondNumber == null)
                return null;

            var diff = firstNumber - secondNumber;
            if (diff == 0 || secondNumber == 0) return 0;
            return diff / secondNumber * 100;
        }

        /// <summary>
        ///     Method to test performance of a process.
        /// </summary>
        /// <param name="customActionForTest">Specifies the custom action for testing performance</param>
        /// <param name="testMode">Specifies the mode of the test (single-core/multi-core)</param>
        /// <param name="numberOfRepetitions">Specifies the number of repetitions (Performance test repetitions)</param>
        /// <returns>A data type for specifying the information of performance.</returns>
        /// <exception>If any exception occurs in the testing process, it's thrown back.</exception>
        public static G9DtPerformanceResult PerformanceTester(Action customActionForTest,
            G9EPerformanceTestMode testMode = G9EPerformanceTestMode.Both, int numberOfRepetitions = 999)
        {
            G9DtTuple<decimal> singleCoreAverageInformation = default;
            G9DtTuple<decimal> multiCoreAverageInformation = default;
            switch (testMode)
            {
                case G9EPerformanceTestMode.SingleCore:
                    singleCoreAverageInformation =
                        PerformanceTesterForSingleCore(customActionForTest, numberOfRepetitions);
                    break;
                case G9EPerformanceTestMode.MultiCore:
                    multiCoreAverageInformation =
                        PerformanceTesterForMultiCore(customActionForTest, numberOfRepetitions);
                    break;
                case G9EPerformanceTestMode.Both:
                    singleCoreAverageInformation =
                        PerformanceTesterForSingleCore(customActionForTest, numberOfRepetitions);
                    multiCoreAverageInformation =
                        PerformanceTesterForMultiCore(customActionForTest, numberOfRepetitions);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(testMode), testMode, null);
            }

            return new G9DtPerformanceResult(singleCoreAverageInformation, multiCoreAverageInformation,
                numberOfRepetitions, testMode);
        }

        /// <summary>
        ///     Helper method for testing performance on single-core.
        /// </summary>
        /// <param name="customActionForTest">Specifies the custom action for testing performance</param>
        /// <param name="numberOfRepetitions">Specifies the number of repetitions (Performance test repetitions)</param>
        /// <returns>Result for single-core</returns>
        private static G9DtTuple<decimal> PerformanceTesterForSingleCore(Action customActionForTest,
            int numberOfRepetitions)
        {
            PrepareSituationForPerformanceTest(true);

            // run once outside of loop to avoid initialization costs
            // warm up
            customActionForTest();

            var startUsedMemory = GetCurrentUsedMemory(true);
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < numberOfRepetitions; i++) customActionForTest();
            sw.Stop();
            var endUsedMemory = GetCurrentUsedMemory();

            PrepareSituationForPerformanceTest();

            return new G9DtTuple<decimal>((decimal)sw.ElapsedMilliseconds / numberOfRepetitions,
                (endUsedMemory - startUsedMemory) / numberOfRepetitions);
        }

        /// <summary>
        ///     Helper method for testing performance on multi-core.
        /// </summary>
        /// <param name="customActionForTest">Specifies the custom action for testing performance</param>
        /// <param name="numberOfRepetitions">Specifies the number of repetitions (Performance test repetitions)</param>
        /// <returns>Result for multi-core</returns>
        private static G9DtTuple<decimal> PerformanceTesterForMultiCore(Action customActionForTest,
            int numberOfRepetitions)
        {
            if (numberOfRepetitions <= 0)
                throw new Exception($"The parameter '{numberOfRepetitions}' can't be under 1.");

            PrepareSituationForPerformanceTest(true);

            // run once outside of loop to avoid initialization costs
            // warm up
            customActionForTest();

            var startUsedMemory = GetCurrentUsedMemory(true);
            var sw = Stopwatch.StartNew();

#if NET35

            var counter = 1;
            for (var i = 0; i < numberOfRepetitions - 1; i++)
                ThreadPool.QueueUserWorkItem(state =>
                {
                    try
                    {
                        customActionForTest();
                    }
                    finally
                    {
                        Interlocked.Increment(ref counter);
                    }
                });

            while (counter < numberOfRepetitions)
                Thread.Sleep(1);
#else
            var option = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount <= 1 ? 10 : Environment.ProcessorCount * 5
            };
            Parallel.For(0, numberOfRepetitions - 1, option, i =>
            {
                customActionForTest();
            });
#endif
            sw.Stop();
            var endUsedMemory = GetCurrentUsedMemory();

            PrepareSituationForPerformanceTest();

            return new G9DtTuple<decimal>((decimal)sw.ElapsedMilliseconds / numberOfRepetitions,
                (endUsedMemory - startUsedMemory) / numberOfRepetitions);
        }


        /// <summary>
        ///     Method to test an action (process) in multi-thread shocking test.
        /// </summary>
        /// <param name="customActionForTest">
        ///     Specifies the custom action for testing performance.
        ///     <para />
        ///     The 'uint' param of the actions is a semi-random number for use if needed.
        /// </param>
        /// <param name="numberOfRepetitions">Specifies the number of repetitions</param>
        /// <exception>If any exception occurs in the multi-thread shocking test, it's thrown back.</exception>
        public static void MultiThreadShockTest(Action<int> customActionForTest, int numberOfRepetitions = 999)
        {
            if (numberOfRepetitions <= 0)
                throw new Exception($"The parameter '{numberOfRepetitions}' can't be under 1.");

            PrepareSituationForPerformanceTest(true);

            // run once outside of loop to avoid initialization costs
            // warm up
            customActionForTest(Thread.CurrentThread.ManagedThreadId);

#if NET35

            var counter = 1;
            for (var i = 0; i < numberOfRepetitions - 1; i++)
            {
                var i1 = i;
                ThreadPool.QueueUserWorkItem(state =>
                {
                    try
                    {
                        customActionForTest(Thread.CurrentThread.ManagedThreadId + i1);
                    }
                    finally
                    {
                        Interlocked.Increment(ref counter);
                    }
                });
            }

            while (counter < numberOfRepetitions)
                Thread.Sleep(1);
#else
            var option = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount <= 1 ? 10 : Environment.ProcessorCount * 5
            };
            Parallel.For(0, numberOfRepetitions - 1, option, i =>
            {
                customActionForTest(Thread.CurrentThread.ManagedThreadId + i);
            });
#endif

            PrepareSituationForPerformanceTest();
        }

        /// <summary>
        ///     Method to get the current used memory by application.
        /// </summary>
        private static decimal GetCurrentUsedMemory(bool start = false)
        {
            if (start) GC.Collect();

            return G9Assembly.GeneralTools.ConvertByteSizeToAnotherSize(GC.GetTotalMemory(start),
                G9ESizeUnits.KiloByte);
        }

        /// <summary>
        ///     Helper method for prepare situation for testing the performance
        /// </summary>
        private static void PrepareSituationForPerformanceTest(bool starter = false)
        {
            if (starter)
            {
                // Prevent the JIT Compiler from optimizing Fkt calls away
                // ReSharper disable once UnusedVariable
                long seed = Environment.TickCount;
                // Run at highest priority to minimize fluctuations caused by other processes/threads
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
            }

            // Clean up
            var processorCount = Environment.ProcessorCount <= 1 ? 5 : Environment.ProcessorCount;
            Thread.AllocateDataSlot();
            ThreadPool.SetMinThreads(processorCount, processorCount);
            ThreadPool.SetMaxThreads(processorCount * 5, processorCount * 5);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}