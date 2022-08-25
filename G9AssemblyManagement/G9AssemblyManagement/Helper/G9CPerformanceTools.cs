using System;
using G9AssemblyManagement.Core;
using G9AssemblyManagement.DataType;
using G9AssemblyManagement.Enums;

namespace G9AssemblyManagement.Helper
{
    /// <summary>
    ///     Performance tools helper
    /// </summary>
    public class G9CPerformanceTools
    {
        /// <summary>
        ///     Method to test the performance of several processes.
        /// </summary>
        /// <param name="testMode">Specifies the mode of the test (single-core/multi-core)</param>
        /// <param name="numberOfRepetitions">Specifies the number of repetitions (Performance test repetitions)</param>
        /// <param name="customActionsForTest">Specifies the several actions for testing comparative performance</param>
        /// <returns>A data type for specifying the information of performance.</returns>
        /// <exception>If any exception occurs in the testing process, it's thrown back.</exception>
        public G9DtComparativePerformanceResults ComparativePerformanceTester(
            G9EPerformanceTestMode testMode = G9EPerformanceTestMode.Both, int numberOfRepetitions = 999,
            params Action[] customActionsForTest)
        {
            return G9CPerformanceHandler.ComparativePerformanceTester(testMode, numberOfRepetitions,
                customActionsForTest);
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
        public G9DtComparativePerformanceResults ComparativePerformanceTester(
            G9EPerformanceTestMode testMode = G9EPerformanceTestMode.Both, int numberOfRepetitions = 999,
            params G9DtCustomPerformanceAction[] customActionsForTest)
        {
            return G9CPerformanceHandler.ComparativePerformanceTester(testMode, numberOfRepetitions,
                customActionsForTest);
        }

        /// <summary>
        ///     Method to test performance of a process.
        /// </summary>
        /// <param name="customActionForTest">Specifies the custom action for testing performance</param>
        /// <param name="testMode">Specifies the mode of the test (single-core/multi-core)</param>
        /// <param name="numberOfRepetitions">Specifies the number of repetitions (Performance test repetitions)</param>
        /// <returns>A data type for specifying the information of performance.</returns>
        /// <exception>If any exception occurs in the testing process, it's thrown back.</exception>
        public G9DtPerformanceResult PerformanceTester(Action customActionForTest,
            G9EPerformanceTestMode testMode = G9EPerformanceTestMode.Both, int numberOfRepetitions = 999)
        {
            return G9CPerformanceHandler.PerformanceTester(customActionForTest, testMode, numberOfRepetitions);
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
        public void MultiThreadShockTest(Action<int> customActionForTest, int numberOfRepetitions = 999)
        {
            G9CPerformanceHandler.MultiThreadShockTest(customActionForTest, numberOfRepetitions);
        }
    }
}