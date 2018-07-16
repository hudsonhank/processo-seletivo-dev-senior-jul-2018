import { async, TestBed } from '@angular/core/testing';
import { LivrosModule } from './livros.module';

describe('LivrosModule', () => {
  beforeEach(
    async(() => {
      TestBed.configureTestingModule({
        imports: [LivrosModule]
      }).compileComponents();
    })
  );

  it('should create', () => {
    expect(LivrosModule).toBeDefined();
  });
});
