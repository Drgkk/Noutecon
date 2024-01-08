using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionModel> Questions { get; set; }
    }
}
