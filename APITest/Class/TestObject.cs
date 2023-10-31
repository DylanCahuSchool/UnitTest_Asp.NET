using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APITest
{
    [Table("TestObject")]
    public class TestObject
    {
        [Key]
        [Column("TEST_ID")]
        public int Id { get; set; }

        [Column("TEST_NAME")]
        public string Name { get; set; }
    }
}