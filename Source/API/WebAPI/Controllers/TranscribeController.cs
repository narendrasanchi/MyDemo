using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TranscribeController : ControllerBase
{
    private readonly IAmazonTranscribeService _transcribeClient;
    private readonly ILogger<TranscribeController> _logger;
    private readonly IConfiguration _configuration;

    public TranscribeController(IAmazonTranscribeService transcribeClient, ILogger<TranscribeController> logger, IConfiguration configuration)
    {
        _transcribeClient = transcribeClient;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost("transcribe")]
    public async Task<IActionResult> TranscribeAudio(IFormFile audioFile, [FromQuery] string languageCode = "en-US")
    {
        try
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("No audio file uploaded.");
            }

            // Validate file format
            var allowedExtensions = new[] { ".mp3", ".mp4", ".wav", ".flac" };
            var fileExtension = Path.GetExtension(audioFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest($"Unsupported file format. Allowed formats: {string.Join(", ", allowedExtensions)}");
            }

            // For demo purposes, we'll simulate transcription since real transcription requires S3 setup
            // In a real implementation, you would:
            // 1. Upload the file to S3
            // 2. Start a transcription job
            // 3. Poll for completion
            // 4. Return the results

            var jobName = $"transcription-{Guid.NewGuid()}";
            
            // Simulate transcription result
            var simulatedTranscription = await SimulateTranscription(audioFile, languageCode);
            
            return Ok(new TranscriptionResponse
            {
                JobName = jobName,
                Status = "COMPLETED",
                Transcript = simulatedTranscription,
                LanguageCode = languageCode,
                CreatedTime = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error transcribing audio: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("job/{jobName}")]
    public async Task<IActionResult> GetTranscriptionJob(string jobName)
    {
        try
        {
            // In a real implementation, you would call GetTranscriptionJobAsync
            // For demo purposes, we'll return a mock response
            
            return Ok(new TranscriptionResponse
            {
                JobName = jobName,
                Status = "COMPLETED",
                Transcript = "This is a sample transcription for demo purposes.",
                LanguageCode = "en-US",
                CreatedTime = DateTime.UtcNow.AddMinutes(-5),
                CompletedTime = DateTime.UtcNow.AddMinutes(-3)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting transcription job: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("supported-languages")]
    public IActionResult GetSupportedLanguages()
    {
        var supportedLanguages = new[]
        {
            new { Code = "en-US", Name = "English (US)" },
            new { Code = "en-GB", Name = "English (UK)" },
            new { Code = "es-ES", Name = "Spanish (Spain)" },
            new { Code = "es-US", Name = "Spanish (US)" },
            new { Code = "fr-FR", Name = "French (France)" },
            new { Code = "fr-CA", Name = "French (Canada)" },
            new { Code = "de-DE", Name = "German" },
            new { Code = "it-IT", Name = "Italian" },
            new { Code = "pt-BR", Name = "Portuguese (Brazil)" },
            new { Code = "ja-JP", Name = "Japanese" },
            new { Code = "ko-KR", Name = "Korean" },
            new { Code = "zh-CN", Name = "Chinese (Simplified)" }
        };

        return Ok(supportedLanguages);
    }

    private async Task<string> SimulateTranscription(IFormFile audioFile, string languageCode)
    {
        // Simulate processing time
        await Task.Delay(1000);

        // Return different sample transcriptions based on language
        return languageCode.ToLower() switch
        {
            "en-us" or "en-gb" => "Hello, this is a sample transcription of your audio file. The transcription service has successfully converted your speech to text.",
            "es-es" or "es-us" => "Hola, esta es una transcripción de muestra de su archivo de audio. El servicio de transcripción ha convertido exitosamente su voz en texto.",
            "fr-fr" or "fr-ca" => "Bonjour, ceci est un échantillon de transcription de votre fichier audio. Le service de transcription a converti avec succès votre discours en texte.",
            "de-de" => "Hallo, dies ist eine Beispieltranskription Ihrer Audiodatei. Der Transkriptionsdienst hat Ihre Sprache erfolgreich in Text umgewandelt.",
            "it-it" => "Ciao, questa è una trascrizione di esempio del tuo file audio. Il servizio di trascrizione ha convertito con successo il tuo discorso in testo.",
            "pt-br" => "Olá, esta é uma transcrição de amostra do seu arquivo de áudio. O serviço de transcrição converteu com sucesso sua fala em texto.",
            "ja-jp" => "こんにちは、これはあなたのオーディオファイルのサンプル転写です。転写サービスはあなたの音声をテキストに正常に変換しました。",
            "ko-kr" => "안녕하세요, 이것은 오디오 파일의 샘플 전사입니다. 전사 서비스가 음성을 텍스트로 성공적으로 변환했습니다.",
            "zh-cn" => "您好，这是您音频文件的示例转录。转录服务已成功将您的语音转换为文本。",
            _ => "This is a sample transcription of your audio file. The transcription service has successfully converted your speech to text."
        };
    }
}

public class TranscriptionResponse
{
    public string JobName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Transcript { get; set; } = string.Empty;
    public string LanguageCode { get; set; } = string.Empty;
    public DateTime CreatedTime { get; set; }
    public DateTime? CompletedTime { get; set; }
    public double? ConfidenceScore { get; set; }
}