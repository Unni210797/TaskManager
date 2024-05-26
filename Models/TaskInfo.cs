namespace Task_Management_System.Models
{
    public class TaskInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }
        public DateOnly DueDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Status { get; set; }
    }
}
