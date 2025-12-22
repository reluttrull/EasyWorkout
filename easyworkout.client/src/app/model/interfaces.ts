export interface ExerciseSet {
  id: string;
  exerciseId: string;
  setNumber: number;
  reps: number;
  weight: number;
  weightUnit: string;
  duration: number;
  durationUnit: string;
  notes: string;
}

export interface Exercise {
  id: string;
  addedByUserId: string;
  addedDate: Date;
  name: string;
  notes: string;
  exerciseSets: ExerciseSet[];
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