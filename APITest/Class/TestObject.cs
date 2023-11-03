using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APITest
{
    [Table("TestObject")]
    public class TestObject
    {
        [Key]
        [Column("TEST_ID")]
        public int Id { get; set; }

        [JsonIgnore]
        [Column("TEST_NAME")]
        public string Name { get; set; }
    }
}