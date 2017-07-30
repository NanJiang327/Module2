using Today.Model;
using System.Threading.Tasks;

namespace Today.Services
{
    public interface IBingSpeechService
    {
        Task<SpeechResult> RecognizeSpeechAsync(string filename);
    }
}
