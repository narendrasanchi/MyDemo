// Polly Models
export interface TextToSpeechRequest {
  text: string;
  voiceId: string;
  outputFormat: string;
}

export interface TextToSpeechResponse {
  audioData: ArrayBuffer;
  contentType: string;
  success: boolean;
  errorMessage?: string;
}

// Comprehend Models
export interface SentimentAnalysisRequest {
  text: string;
  languageCode: string;
}

export interface SentimentAnalysisResponse {
  sentiment: string;
  sentimentScore: SentimentScore;
  success: boolean;
  errorMessage?: string;
}

export interface SentimentScore {
  positive: number;
  negative: number;
  neutral: number;
  mixed: number;
}

export interface EntityDetectionRequest {
  text: string;
  languageCode: string;
}

export interface EntityDetectionResponse {
  entities: DetectedEntity[];
  success: boolean;
  errorMessage?: string;
}

export interface DetectedEntity {
  text: string;
  type: string;
  score: number;
  beginOffset: number;
  endOffset: number;
}

// Translate Models
export interface TranslateTextRequest {
  text: string;
  sourceLanguageCode: string;
  targetLanguageCode: string;
}

export interface TranslateTextResponse {
  translatedText: string;
  sourceLanguageCode: string;
  targetLanguageCode: string;
  success: boolean;
  errorMessage?: string;
}

// Rekognition Models
export interface ImageAnalysisResponse {
  labels: DetectedLabel[];
  success: boolean;
  errorMessage?: string;
}

export interface DetectedLabel {
  name: string;
  confidence: number;
  categories: string[];
}

export interface FaceDetectionResponse {
  faces: DetectedFace[];
  success: boolean;
  errorMessage?: string;
}

export interface DetectedFace {
  boundingBox: BoundingBox;
  landmarks: Landmark[];
  ageRangeLow: number;
  ageRangeHigh: number;
  gender: string;
  emotions: Emotion[];
}

export interface BoundingBox {
  width: number;
  height: number;
  left: number;
  top: number;
}

export interface Landmark {
  type: string;
  x: number;
  y: number;
}

export interface Emotion {
  type: string;
  confidence: number;
}

// Bedrock Models
export interface TextGenerationRequest {
  prompt: string;
  modelId: string;
  maxTokens: number;
  temperature: number;
  topP: number;
}

export interface TextGenerationResponse {
  generatedText: string;
  stopReason: string;
  tokensUsed: number;
  success: boolean;
  errorMessage?: string;
}

// Lex Models
export interface ChatMessage {
  message: string;
  userId: string;
  sessionId: string;
  sessionAttributes?: { [key: string]: string };
  requestAttributes?: { [key: string]: string };
}

export interface ChatResponse {
  message: string;
  dialogAction: string;
  intentName: string;
  sessionAttributes: { [key: string]: string };
  slots: { [key: string]: any };
  success: boolean;
  errorMessage?: string;
}