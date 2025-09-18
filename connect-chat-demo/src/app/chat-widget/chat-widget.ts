import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { environment } from '../../environments/environment';

declare const connect: any;

interface ConnectChatInterface {
  ChatInterface: {
    init: (config: any) => void;
    render: (elementId: string) => void;
  };
}

@Component({
  selector: 'app-chat-widget',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './chat-widget.html',
  styleUrl: './chat-widget.css'
})
export class ChatWidgetComponent implements OnInit, OnDestroy {
  private scriptLoaded = false;
  private chatInitialized = false;
  public isChatOpen = false;
  public isLoading = false;
  public errorMessage = '';

  ngOnInit() {
    this.loadConnectScript();
  }

  ngOnDestroy() {
    // Clean up any chat interface if needed
    if (this.isChatOpen) {
      this.closeChat();
    }
  }

  private loadConnectScript(): void {
    if (this.scriptLoaded) return;

    this.isLoading = true;
    const script = document.createElement('script');
    script.src = environment.connectWidgetUrl;
    script.async = true;
    script.onload = () => {
      this.scriptLoaded = true;
      this.isLoading = false;
      this.initializeChat();
    };
    script.onerror = () => {
      this.isLoading = false;
      this.errorMessage = 'Failed to load Amazon Connect chat script. Please check your configuration.';
    };
    document.head.appendChild(script);
  }

  private initializeChat(): void {
    if (this.chatInitialized || !this.scriptLoaded) return;

    try {
      (window as any).connect.ChatInterface.init({
        containerId: 'amazon-connect-chat-container',
        headerConfig: {
          isVisible: true,
          logoImageURL: '',
          chatTitle: 'Customer Support Chat'
        },
        widgetConfig: {
          isVisible: true,
          width: '400px',
          height: '600px'
        },
        snippetId: environment.snippetId
      });

      this.chatInitialized = true;
      this.errorMessage = '';
    } catch (error) {
      this.errorMessage = 'Failed to initialize Amazon Connect chat. Please check your snippet ID configuration.';
      console.error('Error initializing Connect chat:', error);
    }
  }

  public toggleChat(): void {
    if (!this.scriptLoaded) {
      this.loadConnectScript();
      return;
    }

    if (!this.chatInitialized) {
      this.initializeChat();
    }

    if (this.isChatOpen) {
      this.closeChat();
    } else {
      this.openChat();
    }
  }

  private openChat(): void {
    try {
      const container = document.getElementById('amazon-connect-chat-container');
      if (container) {
        container.style.display = 'block';
        this.isChatOpen = true;
      }
    } catch (error) {
      this.errorMessage = 'Failed to open chat window.';
      console.error('Error opening chat:', error);
    }
  }

  private closeChat(): void {
    try {
      const container = document.getElementById('amazon-connect-chat-container');
      if (container) {
        container.style.display = 'none';
        this.isChatOpen = false;
      }
    } catch (error) {
      console.error('Error closing chat:', error);
    }
  }
}
