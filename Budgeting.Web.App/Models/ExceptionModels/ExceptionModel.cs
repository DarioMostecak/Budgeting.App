// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using FluentAssertions.Execution;
using System.Collections;

namespace Budgeting.Web.App.Models.ExceptionModels
{
    /// <summary>
    /// Represents a custom exception class that inherits from the base Exception class.
    /// Provides additional functionality for handling exceptions and managing associated data.
    /// </summary
    public class ExceptionModel : Exception
    {
        // Default constructor
        public ExceptionModel()
        { }

        // Constructor with message parameter
        public ExceptionModel(string message) : base(message)
        { }

        // Constructor with message and inner exception parameters
        public ExceptionModel(string message, Exception innerException)
            : base(message, innerException)
        { }

        // Constructor with inner exception and data parameters
        public ExceptionModel(Exception innerException, IDictionary data)
            : base(innerException.Message, innerException)
        {
            AddData(data);
        }

        // Constructor with message, inner exception, and data parameters
        public ExceptionModel(string message, Exception innerException, IDictionary data)
            : base(message, innerException)
        {
            AddData(data);
        }

        // Upsert a string value to a list in the Data dictionary based on the provided key
        public void UpsertDataList(string key, string value)
        {
            if (Data.Contains(key))
            {
                (Data[key] as List<string>)?.Add(value);
                return;
            }

            Data.Add(key, new List<string> { value });
        }

        // Throw the exception if the Data dictionary contains any entries
        public void ThrowIfContainsErrors()
        {
            if (Data.Count > 0)
            {
                throw this;
            }
        }

        // Add key-value pairs from the provided dictionary to the Data dictionary
        public void AddData(IDictionary dictionary)
        {
            if (dictionary == null) return;

            foreach (DictionaryEntry item in dictionary)
            {
                Data.Add(item.Key, item.Value);
            }
        }

        #region Methods for unit tests

        // Add key-value pairs to the Data dictionary for unit testing
        public void AddData(string key, params string[] values)
        {
            Data.Add(key, values);
        }

        // Compare the Data dictionary with the provided dictionary for unit testing
        public bool DataEquals(IDictionary dictionary)
        {
            foreach (DictionaryEntry item in dictionary)
            {
                bool num = !Data.Contains(item.Key);
                bool flag = CompareData(Data[item.Key], dictionary[item.Key]);

                if (num || flag) return false;
            }

            return true;
        }

        //Compare two objects for unit testing
        private bool CompareData(object firstObject, object secondObject)
        {
            AssertionScope assertionScope = new AssertionScope();
            firstObject.Should().BeEquivalentTo(secondObject, "");
            return assertionScope.HasFailures();
        }
        #endregion
    }
}
