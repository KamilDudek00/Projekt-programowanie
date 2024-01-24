using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> Users { get;}
    }

    public enum RoleType
    {
        Client,
        Seller
    }
}