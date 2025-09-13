import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';

@Component({
  selector: 'app-dashboard',
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatGridListModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  services = [
    {
      title: 'Text to Speech',
      description: 'Convert text to natural-sounding speech using Amazon Polly',
      icon: 'record_voice_over',
      route: '/text-to-speech',
      color: '#FF6B6B'
    },
    {
      title: 'Speech to Text',
      description: 'Convert audio recordings to text using Amazon Transcribe',
      icon: 'mic',
      route: '/speech-to-text',
      color: '#4ECDC4'
    },
    {
      title: 'Translation',
      description: 'Translate text between languages using Amazon Translate',
      icon: 'translate',
      route: '/translation',
      color: '#45B7D1'
    },
    {
      title: 'Image Analysis',
      description: 'Analyze images for objects, faces, and text using Amazon Rekognition',
      icon: 'image_search',
      route: '/image-analysis',
      color: '#96CEB4'
    },
    {
      title: 'Sentiment Analysis',
      description: 'Analyze text sentiment and extract entities using Amazon Comprehend',
      icon: 'sentiment_satisfied',
      route: '/sentiment-analysis',
      color: '#FFEAA7'
    },
    {
      title: 'Chatbot',
      description: 'Create conversational interfaces using Amazon Lex',
      icon: 'smart_toy',
      route: '/chatbot',
      color: '#DDA0DD'
    },
    {
      title: 'Text Generation',
      description: 'Generate text and chat with AI models using Amazon Bedrock',
      icon: 'auto_stories',
      route: '/text-generation',
      color: '#74B9FF'
    }
  ];
}
