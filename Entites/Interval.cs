using System.ComponentModel.DataAnnotations.Schema;

namespace CHS.DAL.Entites
{
    public class Interval
    {
        public int Id { get; set; }
        public string StartInterval { get; set; }
        public string EndInterval { get; set; }
        public ICollection<Lecture>? Lectures { get; set; }
    }
}
