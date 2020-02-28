using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace AppCenterExtensions.Tests.AppInsights
{
    public class HeadersDictionary : Dictionary<string, StringValues>, IHeaderDictionary
    {
        public long? ContentLength
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
    }
}
