﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuClassLibrary.Tests
{
    internal static class AssertionExtensions
    {
        public static void ShouldHaveExpectedValuesSetToRange(
            this IReadOnlyDictionary<int, bool> possibleValuesDictionary, int minValue, int maxValue)
        {
            int numberOfValues = maxValue - minValue + 1;
            IEnumerable<int> expectedValues = Enumerable.Range(minValue, numberOfValues);
            possibleValuesDictionary.ShouldHaveExpectedValuesSet(expectedValues);
        }

        public static void ShouldHaveExpectedValueSet(
            this IReadOnlyDictionary<int, bool> possibleValuesDictionary, int expectedValue)
        {
            IEnumerable<int> expectedValues = new int[] { expectedValue };
            possibleValuesDictionary.ShouldHaveExpectedValuesSet(expectedValues);
        }

        public static void ShouldHaveExpectedValuesSet(
            this IReadOnlyDictionary<int, bool> possibleValuesDictionary, IEnumerable<int> expectedValues)
        {
            possibleValuesDictionary.Should().NotBeNullOrEmpty().And.HaveCount(9);
            possibleValuesDictionary.Keys.Should().OnlyContain(k => k >= 1 && k <= 9);

            int expectedNumberOfValues = expectedValues.Count();
            possibleValuesDictionary.Where(kvp => kvp.Value == true && expectedValues.Contains(kvp.Key))
                .Should().HaveCount(expectedNumberOfValues);
        }

        public static void ShouldHaveExpectedValuesSetToRange(
            this IReadOnlyCollection<int> possibleValuesList, int minValue, int maxValue)
        {
            int numberOfValues = maxValue - minValue + 1;
            IEnumerable<int> expectedValues = Enumerable.Range(minValue, numberOfValues);
            possibleValuesList.ShouldHaveExpectedValuesSet(expectedValues);
        }

        public static void ShouldHaveExpectedValueSet(
            this IReadOnlyCollection<int> possibleValuesList, int expectedValue)
        {
            IEnumerable<int> expectedValues = new int[] { expectedValue };
            possibleValuesList.ShouldHaveExpectedValuesSet(expectedValues);
        }

        public static void ShouldHaveExpectedValuesSet(
            this IReadOnlyCollection<int> possibleValuesList, IEnumerable<int> expectedValues)
        {
            possibleValuesList.Should().NotBeNullOrEmpty();

            int expectedNumberOfValues = expectedValues.Count();
            possibleValuesList.Distinct().Should()
                .HaveCount(expectedNumberOfValues)
                .And.OnlyContain(pv => expectedValues.Contains(pv));
        }

        public static void ShouldHaveExpectedValue(this int? valueToCheck, int expectedValue)
        {
            valueToCheck.Should().NotBeNull();

#pragma warning disable CS8629 // Nullable value type may be null.
            valueToCheck.Value.Should().Be(expectedValue);
#pragma warning restore CS8629 // Nullable value type may be null.
        }
    }
}
