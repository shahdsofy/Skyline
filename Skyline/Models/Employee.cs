using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skyline.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="You have to provide a valid full name.")]
        [MinLength(8,ErrorMessage ="Full name cannot be less than 8 characters.")]
        [MaxLength(50,ErrorMessage ="Full name mustn't exceed 50 characters.")]
        [Display (Name ="Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "You have to provide a valid National Id.")]
        [MinLength(14, ErrorMessage = "National Id cannot be less than 8 characters.")]
        [MaxLength(14, ErrorMessage = "National Id mustn't exceed 50 characters.")]
        [Display(Name = "National Id")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "You have to provide a valid Occupation.")]
        [MinLength(2, ErrorMessage = "Occupation cannot be less than 2 characters.")]
        [MaxLength(20, ErrorMessage = "Occupation mustn't exceed 20 characters.")]
        [Display(Name = "Occupation")]
        public string Position { get; set; }

        [Range(6000,60000,ErrorMessage ="Salary mustbe between 6000 EGP and 60000 EGP")]
        public decimal Salary { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name ="Hiring Date And Time")]
        public DateTime HiringDateAndTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayName("Attendence Time")]
        public DateTime AttendanceTime { get; set; }

        [DisplayName("Phone No")]
        [RegularExpression("^01\\d{9}$",ErrorMessage ="Invalid Phone Number.")]
        public string PhoneNumber { get; set; }

        [DisplayName("Email Address")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$",ErrorMessage ="Invalid email address")]
        public string EmailAddress { get; set; }

        [DisplayName("Confirm Email Address")]
        [Compare("EmailAddress",ErrorMessage = "Email Address and Confirm Email Address  Do not match")]
        [NotMapped]
        public string ConfirmEmailAddress { get; set; }

        [Display(Name ="Secret Code")]
        [MinLength(4,ErrorMessage ="Secret code cannot be less than 4 charcters")]
        [DataType(DataType.Password)]
        public string SecretCode { get; set; }

        [Display(Name = "Confirm Secret Code")]
        [Compare("SecretCode", ErrorMessage = "Secret code and Confirm Secret Code Do not match")]
        [NotMapped]
        [DataType(DataType.Password)]
        public string ConfirmSecretCode { get; set; }

        [Range(0,100,ErrorMessage ="Appraisal Must be between 0 and 100.")]
        public byte Appraisal { get; set; }

        public bool IsActive { get; set; }

        [ValidateNever]
        public string? Notes { get; set; }

        //foreign key
        [DisplayName("Department")]
        [Range(1,double.MaxValue,ErrorMessage ="Choose a Valid department")]
        public int DepartmentId {  get; set; }

        //Navigation property
        [ValidateNever]
        public Department Departments { get; set; }
        [ValidateNever]
        public string ImagePath {  get; set; }
        [NotMapped]
        [DisplayName("Image")]
        [ValidateNever]
        public IFormFile ImageFile { get; set; }

    }

}
