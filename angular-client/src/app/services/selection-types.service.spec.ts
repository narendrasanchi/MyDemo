import { TestBed } from '@angular/core/testing';

import { SelectionTypesService } from './selection-types.service';

describe('SelectionTypesService', () => {
  let service: SelectionTypesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SelectionTypesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
