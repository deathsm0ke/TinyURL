import { TestBed } from '@angular/core/testing';

import { UrlApi } from './url-api';

describe('UrlApi', () => {
  let service: UrlApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UrlApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
