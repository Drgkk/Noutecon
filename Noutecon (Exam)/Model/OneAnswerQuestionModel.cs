using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public class OneAnswerQuestionModel : QuestionModel, IOneAnswer
    {
        public List<string> Answers { get; set; }
        public int RightAnswer { get; set; }
    }
}
