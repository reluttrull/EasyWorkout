import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompletedWorkoutComponent } from './completed-workout';

describe('CompletedWorkout', () => {
  let component: CompletedWorkoutComponent;
  let fixture: ComponentFixture<CompletedWorkoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompletedWorkoutComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CompletedWorkoutComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
