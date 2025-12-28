import { WeightUnit, DurationUnit } from './enums';

export interface Exercise {
  id: string;
  addedByUserId: string;
  addedDate: Date;
  name: string;
  notes?: string | null;
  exerciseSets: Set[];
}

export interface Set {
  id: string;
  exerciseId: string;
  setNumber: number;
  reps: number;
  weight: number;
  weightUnit: WeightUnit;
  duration: number;
  durationUnit: DurationUnit;
}

export interface Workout {
  id: string;
  addedByUserId: string;
  addedDate: Date;
  name: string;
  notes?: string | null;
  exercises: Exercise[];
}

export interface CreateWorkoutRequest {
  name: string;
  notes?: string | null;
}

export interface UpdateWorkoutRequest {
  name: string;
  notes?: string | null;
}

export interface FinishWorkoutRequest {
  workoutId: string;
  completedDate: Date;
  completedNotes?: string | null;
  completedExercises: FinishExerciseRequest[];
}

export interface FinishExerciseRequest {
  exerciseId: string;
  completedDate: Date;
  exerciseNumber: number;
  completedExerciseSets: FinishExerciseSetRequest[];
}

export interface FinishExerciseSetRequest {
  exerciseSetId: string;
  completedDate: string;
  setNumber: number;
  reps?: number | null;
  weight?: number | null;
  duration?: number | null;
}

export interface UpdateCompletedWorkoutRequest {
  completedNotes?: string | null;
}

export interface WorkoutResponse {
  id: string;
  addedByUserId: string;
  addedDate: Date;
  name: string;
  notes?: string | null;
  lastCompletedDate?: Date | null;
  exercises: ExerciseResponse[];
}

export interface CompletedWorkoutResponse {
  id: string;
  completedByUserId: string;
  workoutId: string;
  originalName?: string | null;
  originalNotes?: string | null;
  completedDate: Date;
  completedNotes?: string | null;
  completedExercises: CompletedExerciseResponse[]
}

export interface CompletedExerciseResponse {
  id: string;
  exerciseId: string;
  completedByUserId: string;
  name?: string | null;
  originalNotes?: string | null;
  completedNotes?: string | null;
  completedDate: Date;
  completedExerciseSets: CompletedExerciseSetResponse[]
}

export interface CompletedExerciseSetResponse {
  id: string;
  exerciseName: string;
  exerciseSetId: string;
  completedWorkoutId: string;
  completedDate: Date;
  setNumber: number;
  reps?: number | null;
  goalReps?: number | null;
  weight?: number | null;
  goalWeight?: number | null;
  weightUnit?: string | null;
  duration?: number | null;
  goalDuration?: number | null;
  durationUnit?: string | null;
}

export interface CreateExerciseRequest {
  name: string;
  notes?: string | null;
}

export interface UpdateExerciseRequest {
  name: string;
  notes?: string | null;
}

export interface ExerciseResponse {
  id: string;
  addedByUserId: string;
  addedDate: Date;
  name: string;
  notes?: string | null;
  exerciseSets: Set[];
}

export interface CreateSetRequest {
  reps?: number | null;
  weight?: number | null;
  weightUnit?: WeightUnit | null;
  duration?: number | null;
  durationUnit?: DurationUnit | null;
  notes?: string | null;
}

export interface UpdateUserRequest {
  firstName: string;
  lastName: string;
}

export interface ChangeEmailRequest {
  email: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

export interface UserResponse {
  firstName: string;
  lastName: string;
  email?: string | null;
  userName?: string | null;
  joinedDate: Date;
}