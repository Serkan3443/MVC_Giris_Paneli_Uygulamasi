#nullable disable
using MVC.Enums;
using System.ComponentModel.DataAnnotations;

namespace MVC.Entities
{
	public class User
	{
        public int Id { get; set; }//Özellik(property)
        public string Guid { get; set; }//en yukarıdaki #nullable disable yazmamızın sebei bu özelliği zorunlu yapmamak için kullandık.

        [Required]//alttaki özelliği zorunlu yapıyoruz demektir
        [StringLength(10,MinimumLength =3)]//kullanıcı adının en fazla 10 karakter olması demektir sql deki nvarchar misali
        public string UserName { get; set; }
        [Required]
        [StringLength(10)]//şifre de 10 karakterli olsun diyoruz
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public Statuses Status { get; set; }
        public DateTime? Birthdate { get; set; }//soru işareti koymamın nedeni bu alanın girilmesi zorunlu olmasın demektir
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public List<UserResource> UserResources { get; set; }




    }
}
