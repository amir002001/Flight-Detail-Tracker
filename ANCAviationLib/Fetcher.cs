using System;
using System.Collections.Generic;
using System.Text;

namespace ANCAviationLib
{
    /// <summary>
    /// Base interface for all Fetchers
    /// </summary>
    /// <typeparam name="T">We wanted the methods to be comboed so a generic could help us implement that feature.</typeparam>
    public interface Fetcher<T>
        where T : Fetcher<T>
    {
        T FetchRawFromApi();
        T ProcessFetch();
    }
}
