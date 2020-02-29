using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AppCenterExtensions.Tests.AppInsights
{
    [ExcludeFromCodeCoverage]
    internal class HeadersDictionary : Dictionary<string, StringValues>, IHeaderDictionary
    {
        public long? ContentLength
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
    }
}
