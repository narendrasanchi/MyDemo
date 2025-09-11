import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BedrockComponent } from './bedrock.component';

describe('BedrockComponent', () => {
  let component: BedrockComponent;
  let fixture: ComponentFixture<BedrockComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BedrockComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BedrockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
