using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Entities
{
	public class UserResource
	{
        [Key]//sql`de tek bir tabloda iki tane primary key oluyprdu ya işte onun gibi  
       
        public int UserId { get; set; }

        public User User { get; set; }

        [Key]
	
		public int ResourceId { get; set; }

        public Resource Resource { get; set; }
    }
}
