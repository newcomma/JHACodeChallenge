using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetProcessing.Abstractions
{
    public interface ITweetStream
    {
        IAsyncEnumerable<TweetDto> ReadTweetsAsync(CancellationToken cancellationToken);
    }
}
