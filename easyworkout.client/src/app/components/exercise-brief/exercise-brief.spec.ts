import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExerciseBrief } from './exercise-brief';

describe('ExerciseBrief', () => {
  let component: ExerciseBrief;
  let fixture: ComponentFixture<ExerciseBrief>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExerciseBrief]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExerciseBrief);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
