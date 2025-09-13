import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-root',
  imports: [
    CommonModule,
    RouterOutlet,
    RouterModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatListModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'AWS AI Services Demo';
  
  menuItems = [
    { path: '/dashboard', label: 'Dashboard', icon: 'dashboard' },
    { path: '/text-to-speech', label: 'Text to Speech', icon: 'record_voice_over' },
    { path: '/speech-to-text', label: 'Speech to Text', icon: 'mic' },
    { path: '/translation', label: 'Translation', icon: 'translate' },
    { path: '/image-analysis', label: 'Image Analysis', icon: 'image_search' },
    { path: '/sentiment-analysis', label: 'Sentiment Analysis', icon: 'sentiment_satisfied' },
    { path: '/chatbot', label: 'Chatbot', icon: 'smart_toy' },
    { path: '/text-generation', label: 'Text Generation', icon: 'auto_stories' }
  ];
}
