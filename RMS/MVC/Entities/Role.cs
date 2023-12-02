#nullable disable
using System.ComponentModel.DataAnnotations;

namespace MVC.Entities
{
	public class Role
	{
        public int Id { get; set; }
        public string GuId { get; set; }
        public string Guid { get; internal set; }
        [Required]//isim alanının zorunlu olması için bunu yazdık
        [StringLength(15)]//isim kaç kalrakter olmasını belirledik
        public string Name { get; set; }
        //public ICollection<User> MyProperty { get; set; }
        //public IEnumerable<User> MyProperty1 { get; set; }
        //public List<User> Users { get; set; }//Bunu yazarak bire sonsuz ilişkisi yapmış olduk

        public List<User> Users { get; set; }//Şu yukarıkdaki 3 tane interface`i yazmaya gerek yok çünkü bu interfaceler List<User> implemente ettiğimiz için bu üç interace`nin özelliklerini almış oluyor.
       
    }
}
