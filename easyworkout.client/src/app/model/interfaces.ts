import { WeightUnit, DurationUnit, DistanceUnit } from './enums';

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
  fallbackName: string | null;
  completedDate: Date;
  exerciseNumber: number;
  completedExerciseSets: FinishExerciseSetRequest[];
}

export interface FinishExerciseSetRequest {
  exerciseSetId: string;
  completedDate: string;
  setNumber: number;
  reps?: number | null;
  goalReps?: number | null;
  weight?: number | null;
  goalWeight?: number | null;
  weightUnit?: string | null;
  duration?: number | null;
  goalDuration?: number | null;
  durationUnit?: string | null;
  distance?: number | null;
  goalDistance?: number | null;
  distanceUnit?: string | null;
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
  lastEditedDate: Date;
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
  lastEditedDate: Date;
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
  exerciseSetId: string | null;
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
  distance?: number | null;
  goalDistance?: number | null;
  distanceUnit?: string | null;
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
  exerciseNumber: number;
  name: string;
  notes?: string | null;
  lastEditedDate: Date;
  exerciseSets: ExerciseSetResponse[];
}

export interface ExerciseSetResponse {
  id: string;
  exerciseId: string;
  setNumber: number;
  reps?: number | null;
  weight?: number | null;
  weightUnit?: string | null;
  duration?: number | null;
  durationUnit?: string | null;
  distance?: number | null;
  distanceUnit?: string | null;
}

export interface CreateSetRequest {
  reps?: number | null;
  weight?: number | null;
  weightUnit?: WeightUnit | null;
  duration?: number | null;
  durationUnit?: DurationUnit | null;
  distance?: number | null;
  distanceUnit?: DistanceUnit | null;
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
  lastEditedDate: Date;
}
