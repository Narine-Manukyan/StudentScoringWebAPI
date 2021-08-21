namespace ScoringPortal.Core
{
    public class Score
    {
        public int ID { get; set; }
	    public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public double Grade { get; set; }
    }
}
