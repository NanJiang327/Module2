using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Today
{
    public static class Constants
    {
        public static readonly string AuthenticationTokenEndpoint = "https://api.cognitive.microsoft.com/sts/v1.0";

        public static readonly string BingSpeechApiKey = "59c66fa0f6864635bde5eea325b52f2b";
        public static readonly string SpeechRecognitionEndpoint = "https://speech.platform.bing.com/recognize";
        public static readonly string AudioContentType = @"audio/wav; codec=""audio/pcm""; samplerate=16000";

        public static readonly string AudioFilename = "Todo.wav";
    }
}
