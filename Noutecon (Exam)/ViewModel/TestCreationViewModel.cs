using Noutecon__Exam_.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
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

		private bool manualAnswerVisibility;

		public bool ManualAnswerVisibility
        {
			get { return manualAnswerVisibility; }
			set { manualAnswerVisibility = value; OnPropertyChanged(nameof(ManualAnswerVisibility)); }
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

		//

		private string rememberQuestionText;

		private string questionText;

		public string QuestionText
        {
			get { return questionText; }
			set { questionText = value; OnPropertyChanged(nameof(QuestionText)); }
		}


		private string imagePath;

		public string ImagePath
		{
			get { return imagePath; }
			set { imagePath = value; OnPropertyChanged(nameof(ImagePath)); }
		}

		private string audioPath;

		public string AudioPath
		{
			get { return audioPath; }
			set { audioPath = value; OnPropertyChanged(nameof(AudioPath)); }
		}

		//Question Answers

		private ObservableCollection<ChoicesViewModel> oneAnswerAnswers;

		public ObservableCollection<ChoicesViewModel> OneAnswerAnswers
        {
			get { return oneAnswerAnswers; }
			set { oneAnswerAnswers = value; OnPropertyChanged(nameof(OneAnswerAnswers)); }
		}

		private int oneAnswerRightAnswer;

		public int OneAnswerRightAnswer
        {
			get { return OneAnswerAnswers.Select((value, index) => new { value, index = index + 1 })
                .Where(pair => pair.value.IsChecked == true)
                .Select(pair => pair.index)
                .FirstOrDefault() - 1; }
		}

		private ObservableCollection<ChoicesViewModel> multipleAnswerAnswers;

		public ObservableCollection<ChoicesViewModel> MultipleAnswerAnswers
        {
			get { return multipleAnswerAnswers; }
			set { multipleAnswerAnswers = value; OnPropertyChanged(nameof(MultipleAnswerAnswers)); }
		}


		private List<int> multipleAnswerRightAnswers;

		public List<int> MultipleAnswerRightAnswers
        {
			get { return MultipleAnswerAnswers.Select((value, index) => new { value, index = index + 1 })
                .Where(pair => pair.value.IsChecked == true)
                .Select(pair => pair.index).ToList(); }
		}

		private string manualAnswerRightAnswer;

		public string ManualAnswerRightAnswer
        {
			get { return manualAnswerRightAnswer; }
			set { manualAnswerRightAnswer = value; OnPropertyChanged(nameof(ManualAnswerRightAnswer)); }
		}




		private string testName;
		private TeacherViewViewModel teacherViewViewModel;

		private List<QuestionModel> questions;
		private int currentQuestion;
		private Func<string, string, string, QuestionModel> createQuestion;

		public ICommand ChangeQuestionTextVisibility { get; }
		public ICommand ChangeQuestionText { get; }
		public ICommand ChangeImage { get; }
		public ICommand SetAudio {  get; }
		public ICommand NextQuestion { get; }
		public ICommand AddOneAnswerAnswer { get; }
		public ICommand AddMultipleAnswerAnswer { get; }


		public TestCreationViewModel(TeacherViewViewModel tvvm, string testName)
		{
			teacherViewViewModel = tvvm;
			this.testName = testName;
			questions = new List<QuestionModel>();
			AnswerTypes = new List<string>() { "One answer test", "Several answer test", "Manual input" };
			SelectedIndex = 0;
            ChangeQuestionTextVisibility = new ViewModelCommand(ExecuteChangeQuestionTextVisibility);
			ChangeQuestionText = new ViewModelCommand(ExecuteChangeQuestionText);
			ChangeImage = new ViewModelCommand(ExecuteChangeImage);
			SetAudio = new ViewModelCommand(ExecuteSetAudio);
			NextQuestion = new ViewModelCommand(ExecuteNextQuestion, CanExecuteNextQuestion);
            AddOneAnswerAnswer = new ViewModelCommand(ExecuteAddOneAnswerAnswer, CanExecuteAddOneAnswerAnswer);
			AddMultipleAnswerAnswer = new ViewModelCommand(ExecuteAddMultipleAnswerAnswer, CanExecuteAddMultipleAnswerAnswer);
            QuestionTextVisibility = true;
            QuestionTextBoxVisibility = false;
			QuestionText = "Question Text";
			rememberQuestionText = QuestionText;
			ImagePath = "/Images/NoImageIcon.png";
			currentQuestion = 0;
			OneAnswerAnswers = new ObservableCollection<ChoicesViewModel>();
			MultipleAnswerAnswers = new ObservableCollection<ChoicesViewModel>();
        }

        private bool CanExecuteAddMultipleAnswerAnswer(object obj)
        {
            bool isValid = true;
            if (MultipleAnswerAnswers.Count >= 6)
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecuteAddMultipleAnswerAnswer(object obj)
        {
			MultipleAnswerAnswers.Add(new ChoicesViewModel() { Answer = "Something", IsChecked = false });
        }

        private bool CanExecuteAddOneAnswerAnswer(object obj)
        {
			bool isValid = true;
			if(OneAnswerAnswers.Count >= 6)
			{
				isValid = false;
			}
			return isValid;
        }

        private void ExecuteAddOneAnswerAnswer(object obj)
        {
			OneAnswerAnswers.Add(new ChoicesViewModel() { Answer = "Something", IsChecked = false });
        }

        private bool CanExecuteNextQuestion(object obj)
        {
			bool isValid = true;
			if(currentQuestion == questions.Count + 1)
			{
				isValid = false;
			}
			return isValid;
        }

        private void ExecuteNextQuestion(object obj)
        {
            currentQuestion++;
            if (currentQuestion != questions.Count)
			{
				UpdateQuestionData(questions[currentQuestion]);
				return;
			}

			questions.Add(createQuestion.Invoke(QuestionText, ImagePath, AudioPath));

            QuestionText = "Question Text";
            rememberQuestionText = QuestionText;
            ImagePath = "/Images/NoImageIcon.png";
        }

		private void UpdateQuestionData(QuestionModel q)
		{
            QuestionText = q.QuestionText;
            ImagePath = q.ImagePath;
			AudioPath = q.AudioPath;

        }

        private void ExecuteSetAudio(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "AudioFiles|*.mp3;*.wav";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                AudioPath = openFileDialog.FileName;
            }
        }

        private void ExecuteChangeImage(object obj)
        {
			
            OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				ImagePath = openFileDialog.FileName;
			}
        }

        private void ExecuteChangeQuestionText(object obj)
        {
            if (string.IsNullOrEmpty(QuestionText))
            {
                QuestionText = "Question Text";
            }
            rememberQuestionText = QuestionText;
            QuestionTextVisibility = true;
            QuestionTextBoxVisibility = false;
            
        }

        private void OnTextBoxVisibilityChange()
        {
            if(QuestionTextBoxVisibility == false)
			{
				QuestionTextVisibility = true;
				QuestionText = rememberQuestionText;
			}
        }
        private void ExecuteChangeQuestionTextVisibility(object obj)
        {
			QuestionTextVisibility = false;
			QuestionTextBoxVisibility = true;
			rememberQuestionText = QuestionText;
        }

        private void OnIndexChanged()
        {
			OneAnswerVisibility = false;
			MultipleAnswersVisibility = false;
			ManualAnswerVisibility = false;
            if(SelectedIndex == 0)
			{
				OneAnswerVisibility = true;
				createQuestion = (QuestionText, ImagePath, AudioPath) =>
				{
					OneAnswerQuestionModel model = new OneAnswerQuestionModel();
					model.QuestionText = QuestionText;
					model.ImagePath = ImagePath;
					model.AudioPath = AudioPath;
                    foreach (var answer in OneAnswerAnswers)
                    {
                        model.Answers.Add(answer.Answer);
                    }
                    model.RightAnswer = OneAnswerRightAnswer;
                    return model;
				};
			}
            else if (SelectedIndex == 1)
            {
                MultipleAnswersVisibility = true;
                createQuestion = (QuestionText, ImagePath, AudioPath) =>
                {
                    MultipleAnswerQuestionModel model = new MultipleAnswerQuestionModel();
                    model.QuestionText = QuestionText;
                    model.ImagePath = ImagePath;
                    model.AudioPath = AudioPath;
                    foreach (var answer in MultipleAnswerAnswers)
                    {
                        model.Answers.Add(answer.Answer);
                    }
					model.RightAnswers = MultipleAnswerRightAnswers;
                    return model;
                };
            }
            else if (SelectedIndex == 2)
            {
                ManualAnswerVisibility = true;
                createQuestion = (QuestionText, ImagePath, AudioPath) =>
                {
                    ManualQuestionModel model = new ManualQuestionModel();
                    model.QuestionText = QuestionText;
                    model.ImagePath = ImagePath;
                    model.AudioPath = AudioPath;
					model.RightAnswer = ManualAnswerRightAnswer;
                    return model;
                };
            }
        }

    }

	public class ChoicesViewModel : ViewModelBase
	{

        private string answer;

        public string Answer
        {
            get { return answer; }
            set { answer = value; OnPropertyChanged(nameof(Answer)); }
        }

		private bool isChecked;

		public bool IsChecked
		{
			get { return isChecked; }
			set { isChecked = value; OnPropertyChanged(nameof(IsChecked)); }
		}


		public ChoicesViewModel()
		{

		}

	}

}
