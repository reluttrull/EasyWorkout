namespace EasyWorkout.Api
{
    public static class Endpoints
    {
        private const string ApiBase = "api";

        public static class Workouts
        {
            private const string Base = $"{ApiBase}/workouts";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id}}";
            public const string GetAllForUser = $"{Base}/me";
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
            public const string AddExercise = $"{Base}/{{id}}/exercises/{{exerciseId}}";
            public const string RemoveExercise = $"{Base}/{{id}}/exercises/{{exerciseId}}";
        }
        public static class Exercises
        {
            private const string Base = $"{ApiBase}/exercises";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id}}";
            public const string GetAllForUser = $"{Base}/me";
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
            public const string CreateSet = $"{Base}/{{id}}/sets";
            public const string DeleteSet = $"{Base}/{{id}}/sets/{{exerciseSetId}}";
        }
        
        public static class ExerciseSets
        {
            private const string Base = $"{ApiBase}/exercisesets";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class CompletedWorkouts
        {
            private const string Base = $"{ApiBase}/completed-workouts";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id}}";
            public const string GetAllForUser = $"{Base}/me";
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }

        public static class Users
        {
            private const string Base = $"{ApiBase}/users";
            public const string Get = $"{Base}/me";
            public const string Update = $"{Base}/me";
        }
    }
}
