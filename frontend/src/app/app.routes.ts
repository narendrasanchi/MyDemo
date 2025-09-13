import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { TextToSpeechComponent } from './components/services/text-to-speech/text-to-speech.component';
import { SpeechToTextComponent } from './components/services/speech-to-text/speech-to-text.component';
import { TranslationComponent } from './components/services/translation/translation.component';
import { ImageAnalysisComponent } from './components/services/image-analysis/image-analysis.component';
import { SentimentAnalysisComponent } from './components/services/sentiment-analysis/sentiment-analysis.component';
import { ChatbotComponent } from './components/services/chatbot/chatbot.component';
import { TextGenerationComponent } from './components/services/text-generation/text-generation.component';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'text-to-speech', component: TextToSpeechComponent },
  { path: 'speech-to-text', component: SpeechToTextComponent },
  { path: 'translation', component: TranslationComponent },
  { path: 'image-analysis', component: ImageAnalysisComponent },
  { path: 'sentiment-analysis', component: SentimentAnalysisComponent },
  { path: 'chatbot', component: ChatbotComponent },
  { path: 'text-generation', component: TextGenerationComponent },
  { path: '**', redirectTo: '/dashboard' }
];
