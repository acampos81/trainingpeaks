namespace trainingpeaks
{
    public class Workout
    {
        public int user_id { get; set; }
        public DateTime datetime_completed { get; set; }
        public List<ExerciseBlock> blocks { get; set; }
    }
}
