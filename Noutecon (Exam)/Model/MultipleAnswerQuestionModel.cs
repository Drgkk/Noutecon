using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public class MultipleAnswerQuestionModel : QuestionModel, IMultipleAnswer
    {
        public List<string> Answers { get; set; }
        public List<int> RightAnswers { get; set; }
    }
}
