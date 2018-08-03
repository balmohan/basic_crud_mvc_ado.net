using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentDbManagement.Models
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string firstName { get; set; }
        [Required]
        [Display(Name ="Last Name")]
        public string lastName { get; set; }
        [Required]
        [Display(Name ="Address")]
        public string address { get; set; }
        [Display(Name ="Birth Date")]
        [DataType(DataType.Date)]
        public DateTime dateOfBirth { get; set; }
    }
}