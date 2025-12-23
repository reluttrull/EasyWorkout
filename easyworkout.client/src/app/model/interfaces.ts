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
  notes: string;
}

export interface UpdateWorkoutRequest {
  name: string;
  notes: string;
}

export interface WorkoutResponse {
  id: string;
  addedByUserId: string;
  addedDate: Date;
  name: String;
  notes: String;
}

export interface CreateExerciseRequest {
  name: string;
  notes: string;
}