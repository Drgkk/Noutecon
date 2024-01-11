using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public abstract class QuestionModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string QuestionText { get; set; }
        public string ImagePath { get; set; }
        public string AudioPath { get; set; }
    }
}
