import { async, TestBed } from '@angular/core/testing';
import { LivrosStateModule } from './livros-state.module';

describe('LivrosStateModule', () => {
  beforeEach(
    async(() => {
      TestBed.configureTestingModule({
        imports: [LivrosStateModule]
      }).compileComponents();
    })
  );

  it('should create', () => {
    expect(LivrosStateModule).toBeDefined();
  });
});
