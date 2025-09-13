import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { PollyComponent } from './ai-services/polly/polly.component';
import { ComprehendComponent } from './ai-services/comprehend/comprehend.component';
import { RekognitionComponent } from './ai-services/rekognition/rekognition.component';
import { TranslateComponent } from './ai-services/translate/translate.component';
import { BedrockComponent } from './ai-services/bedrock/bedrock.component';
import { LexChatbotComponent } from './ai-services/lex-chatbot/lex-chatbot.component';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'polly', component: PollyComponent },
  { path: 'comprehend', component: ComprehendComponent },
  { path: 'rekognition', component: RekognitionComponent },
  { path: 'translate', component: TranslateComponent },
  { path: 'bedrock', component: BedrockComponent },
  { path: 'chatbot', component: LexChatbotComponent },
  { path: '**', redirectTo: '/dashboard' }
];
