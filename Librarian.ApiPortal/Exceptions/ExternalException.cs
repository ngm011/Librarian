using System;
using System.Collections.Generic;

namespace Librarian.ApiPortal.Exceptions
{
    [Serializable]
    public class ExternalException : Exception
    {
        private readonly Dictionary<string, IList<string>> _messages = new();

        public ExternalException() { }
        public ExternalException(string key, string value) =>
            this.AddMessage(key, value);

        public void AddMessage(string key, string value)
        {
            if (_messages.ContainsKey(key))
                _messages[key].Add(value);
            else
                _messages.Add(key, new List<string> { value });
        }

        public static explicit operator Dictionary<string, IList<string>>(ExternalException ex) =>
            ex._messages;
    }
}
