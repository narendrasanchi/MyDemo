import { TestBed } from '@angular/core/testing';

import { LexService } from './lex.service';

describe('LexService', () => {
  let service: LexService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LexService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
