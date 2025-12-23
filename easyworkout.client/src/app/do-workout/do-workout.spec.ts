import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoWorkout } from './do-workout';

describe('DoWorkout', () => {
  let component: DoWorkout;
  let fixture: ComponentFixture<DoWorkout>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoWorkout]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoWorkout);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
