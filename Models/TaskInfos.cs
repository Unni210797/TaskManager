using static Task_Management_System.ViewModel.TaskViewModel;
using System.ComponentModel.DataAnnotations;

namespace Task_Management_System.Models
{
    public class TaskInfos
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [FutureDate(ErrorMessage = "Date must be in the future")]
        public DateOnly DueDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Status { get; set; }
    }
}
