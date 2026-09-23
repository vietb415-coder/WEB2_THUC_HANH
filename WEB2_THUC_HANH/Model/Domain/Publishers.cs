using System.ComponentModel.DataAnnotations;
using WEB2_THUC_HANH.Model.Domain;

namespace WEB2_THUC_HANH.Model.Domain
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //Navigation Properties - One publisher has many books
        public List<Book> Books { get; set; }
    }
}