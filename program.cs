using Google.Cloud.Speech.V1;
using Grpc.Auth;
using Grpc.Core;
using System;

class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        string audioFilePath = "path_to_your_audio_file";
        string projectId = "your_project_id";

        // Authenticate with Google Cloud
        var credential = GoogleCredential.FromFile("path_to_your_service_account_key.json");
        var channel = new Channel(SpeechClient.DefaultEndpoint.Host, credential.ToChannelCredentials());
        var speechClient = SpeechClient.Create(channel);

        // Configure audio settings
        var config = new RecognitionConfig
        {
            Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
            SampleRateHertz = 16000,
            LanguageCode = LanguageCodes.English.UnitedStates
        };

        // Perform transcription
        var audio = RecognitionAudio.FromFile(audioFilePath);
        var response = await speechClient.RecognizeAsync(config, audio);
        
        // Output transcriptions
        foreach (var result in response.Results)
        {
            foreach (var alternative in result.Alternatives)
            {
                Console.WriteLine($"Transcription: {alternative.Transcript}");
            }
        }

        // Shutdown gRPC channel
        await channel.ShutdownAsync();
    }
}
