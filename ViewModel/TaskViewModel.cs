using System.ComponentModel.DataAnnotations;
using Task_Management_System.Models;

namespace Task_Management_System.ViewModel
{
    public class TaskViewModel
    {
        public class FutureDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value is DateTime dateTime)
                {
                    return dateTime.Date >= DateTime.Today;
                }
                return false;
            }
        }
        public string Username { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        
        public DateOnly DueDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Status { get; set; }
    }
}
