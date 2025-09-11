import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LexChatbotComponent } from './lex-chatbot.component';

describe('LexChatbotComponent', () => {
  let component: LexChatbotComponent;
  let fixture: ComponentFixture<LexChatbotComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LexChatbotComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LexChatbotComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
