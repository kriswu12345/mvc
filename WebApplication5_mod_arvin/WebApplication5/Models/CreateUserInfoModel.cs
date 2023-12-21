using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class CreateUserInfoModel
    {
        [Required]
        [Display(Name = "姓名")]
        [EmailAddress]
        public string name { get; set; }

        [Required]
        [Display(Name = "年齡")]
        public string age { get; set; }

        [Display(Name = "生日")]
        public string birthday { get; set; }
    }
    public class CreateUserInfoModels
    {
        public List<CreateUserInfoModel> UserList { get; set; }
    }

}