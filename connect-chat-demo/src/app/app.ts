import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ChatWidgetComponent } from './chat-widget/chat-widget';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ChatWidgetComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Amazon Connect Chat Demo');
}
