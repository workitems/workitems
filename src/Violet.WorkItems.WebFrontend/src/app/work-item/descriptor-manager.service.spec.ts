import { TestBed } from '@angular/core/testing';

import { DescriptorManagerService } from './descriptor-manager.service';

describe('DescriptorManagerService', () => {
  let service: DescriptorManagerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DescriptorManagerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
