import { WeightUnit, DurationUnit } from './enums';

export interface Exercise {
  id: string;
  addedByUserId: string;
  addedDate: Date;
  name: string;
  notes: string;
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
  notes: string;
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

export interface WorkoutResponse {
  id: string;
  addedByUserId: string;
  addedDate: Date;
  name: string;
  notes?: string | null;
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
}

export interface CreateSetRequest {
  reps?: number | null;
  weight?: number | null;
  weightUnit?: WeightUnit | null;
  duration?: number | null;
  durationUnit?: DurationUnit | null;
  notes?: string | null;
}

export interface UserResponse {
  firstName: string;
  lastName: string;
  joinedDate: Date;
}