#nullable disable
using System.ComponentModel.DataAnnotations;

namespace MVC.Entities
{
	public class Resource
	{
        public int Id { get; set; }
        public string GuId { get; set; }//yukarıda #nullable disable yaptım çünkü zorunlu yapmaya gerek yok dediğim için 

        [Required]//title zorunlu olsun
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public double? Score  { get; set; }//bu alan zorunlu olmak zorunda değil çünkü soru işsareti koydum


        public DateTime Date { get; set; }
        public List<UserResource> UserResources { get; set; }


    }
}
