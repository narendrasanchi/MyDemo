import { TestBed } from '@angular/core/testing';

import { ComprehendService } from './comprehend.service';

describe('ComprehendService', () => {
  let service: ComprehendService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ComprehendService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
