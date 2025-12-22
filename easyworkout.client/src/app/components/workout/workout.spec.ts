import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkoutComponent } from './workout';

describe('Workout', () => {
  let component: WorkoutComponent;
  let fixture: ComponentFixture<WorkoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkoutComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkoutComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
