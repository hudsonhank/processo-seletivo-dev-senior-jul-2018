import { async, TestBed } from '@angular/core/testing';
import { FormDinamicoModule } from './form-dinamico.module';

describe('FormDinamicoModule', () => {
  beforeEach(
    async(() => {
      TestBed.configureTestingModule({
        imports: [FormDinamicoModule]
      }).compileComponents();
    })
  );

  it('should create', () => {
    expect(FormDinamicoModule).toBeDefined();
  });
});
