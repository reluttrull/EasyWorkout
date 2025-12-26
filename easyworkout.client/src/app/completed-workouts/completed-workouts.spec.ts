import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompletedWorkoutsComponent } from './completed-workouts';

describe('CompletedWorkouts', () => {
  let component: CompletedWorkoutsComponent;
  let fixture: ComponentFixture<CompletedWorkoutsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompletedWorkoutsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CompletedWorkoutsComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
