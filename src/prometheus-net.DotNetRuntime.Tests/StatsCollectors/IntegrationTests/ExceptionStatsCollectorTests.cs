using NUnit.Framework;
using Prometheus.DotNetRuntime.StatsCollectors;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prometheus.DotNetRuntime.Tests.StatsCollectors.IntegrationTests
{
    [TestFixture]
    internal class ExceptionStatsCollectorTests : StatsCollectorIntegrationTestBase<ExceptionStatsCollector>
    {
        protected override ExceptionStatsCollector CreateStatsCollector()
        {
            return new ExceptionStatsCollector();
        }

        [Test]
        public void Will_measure_when_occurring_an_exception()
        {
            // act
            var divider = 0;
            
            try
            {
                var result = 1 / divider;
            }
            catch (System.DivideByZeroException divZeroEx)
            {
            }

            // assert
            Assert.That(() => StatsCollector.ExceptionCount.Labels("System.DivideByZeroException").Value, Is.EqualTo(1).After(100, 1000));
        }

        [Test]
        public void Will_measure_when_not_occurring_an_exception()
        {
            // arrange
            int divider = 1;
            
            // act
            var result = 1 / 1;

            // assert
            Assert.That(() => StatsCollector.ExceptionCount.CollectAllValues().Count(), Is.EqualTo(0).After(100, 1000));
        }
    }
}