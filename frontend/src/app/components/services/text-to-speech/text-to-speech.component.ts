import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { ApiService } from '../../../services/api.service';

@Component({
  selector: 'app-text-to-speech',
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  templateUrl: './text-to-speech.component.html',
  styleUrl: './text-to-speech.component.scss'
})
export class TextToSpeechComponent implements OnInit {
  text: string = 'Hello! Welcome to the AWS AI Services Demo. This is Amazon Polly converting text to speech.';
  selectedVoice: string = 'Joanna';
  isNeural: boolean = false;
  isLoading: boolean = false;
  audioUrl: string | null = null;
  voices: any[] = [];
  languages: any[] = [];

  constructor(
    private apiService: ApiService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadVoices();
    this.loadLanguages();
  }

  loadVoices() {
    this.apiService.getVoices().subscribe({
      next: (voices: any) => {
        this.voices = voices;
      },
      error: (error: any) => {
        console.error('Error loading voices:', error);
        // Fallback voices for demo
        this.voices = [
          { Id: 'Joanna', Name: 'Joanna', Gender: 'Female', LanguageCode: 'en-US' },
          { Id: 'Matthew', Name: 'Matthew', Gender: 'Male', LanguageCode: 'en-US' },
          { Id: 'Amy', Name: 'Amy', Gender: 'Female', LanguageCode: 'en-GB' },
          { Id: 'Brian', Name: 'Brian', Gender: 'Male', LanguageCode: 'en-GB' }
        ];
      }
    });
  }

  loadLanguages() {
    this.apiService.getLanguages().subscribe({
      next: (languages: any) => {
        this.languages = languages;
      },
      error: (error: any) => {
        console.error('Error loading languages:', error);
        // Fallback languages for demo
        this.languages = [
          { Code: 'en-US', Name: 'English (US)' },
          { Code: 'en-GB', Name: 'English (UK)' },
          { Code: 'es-ES', Name: 'Spanish' },
          { Code: 'fr-FR', Name: 'French' }
        ];
      }
    });
  }

  generateSpeech() {
    if (!this.text.trim()) {
      this.snackBar.open('Please enter some text to convert.', 'Close', { duration: 3000 });
      return;
    }

    this.isLoading = true;
    this.audioUrl = null;

    this.apiService.textToSpeech(this.text, this.selectedVoice, this.isNeural).subscribe({
      next: (audioBlob: Blob) => {
        this.audioUrl = URL.createObjectURL(audioBlob);
        this.isLoading = false;
        this.snackBar.open('Speech generated successfully!', 'Close', { duration: 3000 });
      },
      error: (error: any) => {
        console.error('Error generating speech:', error);
        this.isLoading = false;
        this.snackBar.open('Error generating speech. Please try again.', 'Close', { duration: 5000 });
      }
    });
  }

  downloadAudio() {
    if (this.audioUrl) {
      const link = document.createElement('a');
      link.href = this.audioUrl;
      link.download = `speech_${Date.now()}.mp3`;
      link.click();
    }
  }

  clearAudio() {
    if (this.audioUrl) {
      URL.revokeObjectURL(this.audioUrl);
      this.audioUrl = null;
    }
  }

  onVoiceChange() {
    this.clearAudio();
  }

  onTextChange() {
    this.clearAudio();
  }
}
