import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Set } from './set';

describe('Set', () => {
  let component: Set;
  let fixture: ComponentFixture<Set>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Set]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Set);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
