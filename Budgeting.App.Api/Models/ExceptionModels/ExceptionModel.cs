using FluentAssertions;
using FluentAssertions.Execution;
using System.Collections;

namespace Budgeting.App.Api.Models.ExceptionModels
{
    public class ExceptionModel : Exception
    {
        public ExceptionModel()
        { }

        public ExceptionModel(string message) : base(message)
        { }

        public ExceptionModel(string message, Exception innerException)
            : base(message, innerException)
        { }

        public ExceptionModel(Exception innerException, IDictionary data)
            : base(innerException.Message, innerException)
        {
            AddData(data);
        }

        public ExceptionModel(string message, Exception innerException, IDictionary data)
            : base(message, innerException)
        {
            AddData(data);
        }

        public void UpsertDataList(string key, string value)
        {
            if (Data.Contains(key))
            {
                (Data[key] as List<string>)?.Add(value);
                return;
            }

            Data.Add(key, new List<string> { value });
        }

        public void ThrowIfContainsErrors()
        {
            if (Data.Count > 0)
            {
                throw this;
            }
        }

        public void AddData(IDictionary dictionary)
        {
            if (dictionary == null) return;

            foreach (DictionaryEntry item in dictionary)
            {
                Data.Add(item.Key, item.Value);
            }
        }

        #region Methods for unit tests
        public void AddData(string key, params string[] values)
        {
            Data.Add(key, values);
        }

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

        private bool CompareData(object firstObject, object secondObject)
        {
            AssertionScope assertionScope = new AssertionScope();
            firstObject.Should().BeEquivalentTo(secondObject, "");
            return assertionScope.HasFailures();
        }
        #endregion
    }
}
