using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetProcessing.Abstractions;

namespace TweetProcessing.ApiV2
{
    internal class TweetStreamFactory : ITweetStreamFactory
    {
        public Task<ITweetStream> Create()
        {
            
        }
    }
}
