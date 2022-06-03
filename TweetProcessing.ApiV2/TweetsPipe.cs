using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetProcessing.ApiV2
{
    internal class TweetsPipe
    {
        private readonly Pipe pipe = new Pipe();
        public PipeWriter Writer { get => pipe.Writer; }
        public PipeReader Reader { get => pipe.Reader; }

    }
}
