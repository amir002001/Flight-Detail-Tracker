using System;
using System.Collections.Generic;
using System.Text;

namespace ANCAviationLib
{
    public interface Fetcher<T>
        where T : Fetcher<T>
    {
        T FetchRawFromApi();
        T SaveFetch(Uri Directory);
        T ProcessFetch();
    }
}
