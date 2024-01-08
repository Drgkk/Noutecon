using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public interface IOneAnswer
    {
        public List<string> Answers { get; set; }
        public int RightAnswer { get; set; }
    }

    public interface IMultipleAnswer
    {
        public List<string> Answers { get; set; }
        public List<int> RightAnswers { get; set; }
    }

    public interface IManualAnswer
    {
        public string RightAnswer { get; set; }
    }
}
