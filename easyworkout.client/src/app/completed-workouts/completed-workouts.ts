import { Component, ViewChild, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { provideNativeDateAdapter } from '@angular/material/core';
import {MatIconModule} from '@angular/material/icon';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { WorkoutsService } from '../workouts/workouts.service';
import { ExercisesService } from '../exercises/exercises.service';
import { CompletedWorkoutsService } from './completed-workouts.service';
import { CompletedWorkoutResponse, WorkoutResponse, ExerciseResponse, GetAllCompletedWorkoutsRequest } from '../model/interfaces';
import { CompletedWorkoutComponent } from '../components/completed-workout/completed-workout';
import { OrderByPipe } from '../pipes/order-by-pipe';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Component({
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [CommonModule, ReactiveFormsModule, MatProgressSpinnerModule, MatButtonModule, MatFormFieldModule, MatIconModule,
    MatInputModule, MatSelectModule, MatDatepickerModule, MatPaginator, CompletedWorkoutComponent, OrderByPipe],
  templateUrl: './completed-workouts.html',
  styleUrl: './completed-workouts.css'
})
export class CompletedWorkoutsComponent {
  fb = inject(FormBuilder);
  filters:FormGroup = this.fb.nonNullable.group({
    minDate: [null],
    maxDate: [null],
    containsText: [null],
    basedOnWorkoutId: [null],
    containsExerciseId: [null]
  });
  areFiltersVisible = signal(false);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  totalItems = 0;
  pageSize = 10;
  pageSizeOptions: number[] = [5, 10, 25];

  workoutsService = inject(WorkoutsService);
  exercisesService = inject(ExercisesService);
  workouts = signal<WorkoutResponse[]>([]);
  exercises = signal<ExerciseResponse[]>([]);
  completedWorkouts = signal<CompletedWorkoutResponse[]>([]);
  isLoaded = signal(false);
  isCreateVisible = false;

  constructor(private completedWorkoutsService: CompletedWorkoutsService) {
    this.loadOnce();
  }
  
  ngAfterViewInit() { // paginator's page event
    this.paginator.page
      .pipe(
        tap(() => this.getCompletedWorkouts())
      )
      .subscribe();
      
    this.getCompletedWorkouts(); 
  }
  
  onPageChange(event: PageEvent) {
    this.getCompletedWorkouts();
  }

  loadOnce() {
    this.workoutsService.getAll().subscribe(w => {
      this.workouts.set(w);
    });
    this.exercisesService.getAll().subscribe(e => {
      this.exercises.set(e);
    });
  }

  applyFilters() {
    if (this.paginator) { // reset pagination with filters
      this.paginator.pageIndex = 0;
      this.paginator.firstPage();
    }
    this.getCompletedWorkouts();
  }
  
  clearFilters() {
    this.filters = this.fb.nonNullable.group({
      minDate: [null],
      maxDate: [null],
      containsText: [null],
      basedOnWorkoutId: [null],
      containsExerciseId: [null]
    });
    this.applyFilters();
  }

  getCompletedWorkouts() {
    const getAllFilters:GetAllCompletedWorkoutsRequest = {
      minDate: this.filters.value.minDate,
      maxDate: this.filters.value.maxDate,
      basedOnWorkoutId: this.filters.value.basedOnWorkoutId,
      containsExerciseId: this.filters.value.containsExerciseId,
      containsText: this.filters.value.containsText,
      page: this.paginator?.pageIndex ?? 0,
      pageSize: this.paginator?.pageSize ?? this.pageSize
    };

    this.completedWorkoutsService.getAll(getAllFilters).subscribe(response => {
      this.totalItems = response.total;
      this.completedWorkouts.set(response.items);
      this.areFiltersVisible.set(false);
      this.isLoaded.set(true);
    });
  }

  toggleFiltersVisible() {
    this.areFiltersVisible.set(true);
  }

  showAddWorkout() {
    this.isCreateVisible = true;
  }

  handleCancelCreate() {
    this.isCreateVisible = false;
    this.getCompletedWorkouts();
  }
}