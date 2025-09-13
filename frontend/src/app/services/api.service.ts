import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Polly - Text to Speech
  textToSpeech(text: string, voiceId: string = 'Joanna', neural: boolean = false): Observable<Blob> {
    return this.http.post(`${this.baseUrl}/Polly/text-to-speech`, {
      text,
      voiceId,
      neural
    }, { responseType: 'blob' });
  }

  getVoices(languageCode?: string): Observable<any> {
    const params = languageCode ? { languageCode } : {};
    return this.http.get(`${this.baseUrl}/Polly/voices`, { params });
  }

  getLanguages(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Polly/languages`);
  }

  // Transcribe - Speech to Text
  transcribeAudio(audioFile: File, languageCode: string = 'en-US'): Observable<any> {
    const formData = new FormData();
    formData.append('audioFile', audioFile);
    return this.http.post(`${this.baseUrl}/Transcribe/transcribe?languageCode=${languageCode}`, formData);
  }

  getTranscriptionJob(jobName: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/Transcribe/job/${jobName}`);
  }

  getSupportedTranscribeLanguages(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Transcribe/supported-languages`);
  }

  // Translate
  translateText(text: string, sourceLanguageCode: string = 'auto', targetLanguageCode: string = 'en'): Observable<any> {
    return this.http.post(`${this.baseUrl}/Translate/translate`, {
      text,
      sourceLanguageCode,
      targetLanguageCode
    });
  }

  detectLanguage(text: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/Translate/detect-language`, { text });
  }

  getSupportedTranslateLanguages(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Translate/supported-languages`);
  }

  batchTranslate(texts: string[], sourceLanguageCode: string, targetLanguageCode: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/Translate/batch-translate`, {
      texts,
      sourceLanguageCode,
      targetLanguageCode
    });
  }

  // Comprehend - Sentiment Analysis
  analyzeSentiment(text: string, languageCode: string = 'en'): Observable<any> {
    return this.http.post(`${this.baseUrl}/Comprehend/sentiment`, {
      text,
      languageCode
    });
  }

  detectEntities(text: string, languageCode: string = 'en'): Observable<any> {
    return this.http.post(`${this.baseUrl}/Comprehend/entities`, {
      text,
      languageCode
    });
  }

  detectKeyPhrases(text: string, languageCode: string = 'en'): Observable<any> {
    return this.http.post(`${this.baseUrl}/Comprehend/key-phrases`, {
      text,
      languageCode
    });
  }

  detectDominantLanguage(text: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/Comprehend/language`, { text });
  }

  comprehensiveAnalysis(text: string, languageCode: string = 'en'): Observable<any> {
    return this.http.post(`${this.baseUrl}/Comprehend/comprehensive-analysis`, {
      text,
      languageCode
    });
  }

  // Rekognition - Image Analysis
  detectLabels(imageFile: File, maxLabels: number = 10, minConfidence: number = 70): Observable<any> {
    const formData = new FormData();
    formData.append('imageFile', imageFile);
    return this.http.post(`${this.baseUrl}/Rekognition/detect-labels?maxLabels=${maxLabels}&minConfidence=${minConfidence}`, formData);
  }

  detectFaces(imageFile: File): Observable<any> {
    const formData = new FormData();
    formData.append('imageFile', imageFile);
    return this.http.post(`${this.baseUrl}/Rekognition/detect-faces`, formData);
  }

  detectModeration(imageFile: File, minConfidence: number = 60): Observable<any> {
    const formData = new FormData();
    formData.append('imageFile', imageFile);
    return this.http.post(`${this.baseUrl}/Rekognition/detect-moderation?minConfidence=${minConfidence}`, formData);
  }

  detectTextInImage(imageFile: File): Observable<any> {
    const formData = new FormData();
    formData.append('imageFile', imageFile);
    return this.http.post(`${this.baseUrl}/Rekognition/detect-text`, formData);
  }

  // Lex - Chatbot
  recognizeText(message: string, sessionId: string, sessionAttributes: any = {}): Observable<any> {
    return this.http.post(`${this.baseUrl}/Lex/recognize-text`, {
      message,
      sessionId,
      sessionAttributes
    });
  }

  recognizeSpeech(audioFile: File, sessionId: string, sessionAttributes: any = {}): Observable<any> {
    const formData = new FormData();
    formData.append('audioFile', audioFile);
    // Note: For form data with additional parameters, we need to append them
    formData.append('sessionId', sessionId);
    formData.append('sessionAttributes', JSON.stringify(sessionAttributes));
    return this.http.post(`${this.baseUrl}/Lex/recognize-speech`, formData);
  }

  startConversation(): Observable<any> {
    return this.http.post(`${this.baseUrl}/Lex/start-conversation`, {});
  }

  getConversationHistory(sessionId: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/Lex/session/${sessionId}/history`);
  }

  // Bedrock - Text Generation
  generateText(prompt: string, modelId: string = 'amazon.titan-text-express-v1', maxTokens: number = 1000, temperature: number = 0.7): Observable<any> {
    return this.http.post(`${this.baseUrl}/Bedrock/generate-text`, {
      prompt,
      modelId,
      maxTokens,
      temperature
    });
  }

  generateWithClaude(messages: any[], maxTokens: number = 1000, temperature: number = 0.7): Observable<any> {
    return this.http.post(`${this.baseUrl}/Bedrock/generate-claude`, {
      messages,
      maxTokens,
      temperature
    });
  }

  generateWithTitan(inputText: string, maxTokenCount: number = 1000, temperature: number = 0.7): Observable<any> {
    return this.http.post(`${this.baseUrl}/Bedrock/generate-titan`, {
      inputText,
      maxTokenCount,
      temperature
    });
  }

  chatCompletion(messages: any[], model: string = 'anthropic.claude-3-sonnet-20240229-v1:0', maxTokens: number = 1000, temperature: number = 0.7): Observable<any> {
    return this.http.post(`${this.baseUrl}/Bedrock/chat`, {
      messages,
      model,
      maxTokens,
      temperature
    });
  }

  getAvailableModels(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Bedrock/models`);
  }

  generateEmbeddings(inputs: string[], modelId: string = 'amazon.titan-embed-text-v1'): Observable<any> {
    return this.http.post(`${this.baseUrl}/Bedrock/embeddings`, {
      inputs,
      modelId
    });
  }
}
