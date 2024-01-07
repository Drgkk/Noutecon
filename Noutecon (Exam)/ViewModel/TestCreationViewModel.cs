using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    public class TestCreationViewModel : ViewModelBase
    {
		private List<string> answerTypes;

		public List<string> AnswerTypes
        {
			get { return answerTypes; }
			set { answerTypes = value; OnPropertyChanged(nameof(AnswerTypes)); }
		}

		private int selectedIndex;

		public int SelectedIndex
        {
			get { return selectedIndex; }
			set { selectedIndex = value; OnPropertyChanged(nameof(SelectedIndex)); OnIndexChanged(); }
		}

        

        //Visibilities

        private bool oneAnswerVisibility;

		public bool OneAnswerVisibility
        {
			get { return oneAnswerVisibility; }
			set { oneAnswerVisibility = value; OnPropertyChanged(nameof(OneAnswerVisibility)); }
		}

		private bool multipleAnswersVisibility;

		public bool MultipleAnswersVisibility
        {
			get { return multipleAnswersVisibility; }
			set { multipleAnswersVisibility = value; OnPropertyChanged(nameof(MultipleAnswersVisibility)); }
		}

		private bool manualAnswerShortVisibility;

		public bool ManualAnswerShortVisibility
        {
			get { return manualAnswerShortVisibility; }
			set { manualAnswerShortVisibility = value; OnPropertyChanged(nameof(ManualAnswerShortVisibility)); }
		}

		private bool manualAnswerLongVisibility;

		public bool ManualAnswerLongVisibility
        {
			get { return manualAnswerLongVisibility; }
			set { manualAnswerLongVisibility = value; OnPropertyChanged(nameof(ManualAnswerLongVisibility)); }
		}

		private bool questionTextBoxVisibility;

		public bool QuestionTextBoxVisibility
        {
			get { return questionTextBoxVisibility; }
			set { questionTextBoxVisibility = value; OnPropertyChanged(nameof(QuestionTextBoxVisibility)); OnTextBoxVisibilityChange(); }
		}

        

        private bool questionTextVisibility;

		public bool QuestionTextVisibility
        {
			get { return questionTextVisibility; }
			set { questionTextVisibility = value; OnPropertyChanged(nameof(QuestionTextVisibility)); }
		}



		public ICommand ChangeQuestionText { get; }


		public TestCreationViewModel()
		{
			AnswerTypes = new List<string>() { "One answer test", "Several answer test", "Manual input short", "Manual input long" };
			SelectedIndex = 0;
			ChangeQuestionText = new ViewModelCommand(ExecuteChangeQuestionText);
            QuestionTextVisibility = true;
            QuestionTextBoxVisibility = false;
        }

        private void OnTextBoxVisibilityChange()
        {
            if(QuestionTextBoxVisibility == false)
			{
				QuestionTextVisibility = true;
			}
        }
        private void ExecuteChangeQuestionText(object obj)
        {
			QuestionTextVisibility = false;
			QuestionTextBoxVisibility = true;
        }

        private void OnIndexChanged()
        {
			OneAnswerVisibility = false;
			MultipleAnswersVisibility = false;
			ManualAnswerShortVisibility = false;
			ManualAnswerLongVisibility = false;
            if(SelectedIndex == 0)
			{
				OneAnswerVisibility = true;
			}
            else if (SelectedIndex == 1)
            {
                MultipleAnswersVisibility = true;
            }
            else if (SelectedIndex == 2)
            {
                ManualAnswerShortVisibility = true;
            }
            else if (SelectedIndex == 3)
            {
                ManualAnswerLongVisibility = true;
            }
        }

    }
}
