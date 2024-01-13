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

		private ObservableCollection<OneChoicesViewModel> oneAnswerAnswers;

		public ObservableCollection<OneChoicesViewModel> OneAnswerAnswers
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

		private ObservableCollection<MultipleChoicesViewModel> multipleAnswerAnswers;

		public ObservableCollection<MultipleChoicesViewModel> MultipleAnswerAnswers
        {
			get { return multipleAnswerAnswers; }
			set { multipleAnswerAnswers = value; OnPropertyChanged(nameof(MultipleAnswerAnswers)); }
		}


		private List<int> multipleAnswerRightAnswers;

		public List<int> MultipleAnswerRightAnswers
        {
			get { return MultipleAnswerAnswers.Select((value, index) => new { value, index = index + 1 })
                .Where(pair => pair.value.IsChecked == true)
                .Select(pair => pair.index).ToList().Select(o => o - 1).ToList(); }
		}

		private string manualAnswerRightAnswer;

		public string ManualAnswerRightAnswer
        {
			get { return manualAnswerRightAnswer; }
			set { manualAnswerRightAnswer = value; OnPropertyChanged(nameof(ManualAnswerRightAnswer)); }
		}


        private int currentQuestion;

        public int CurrentQuestion
        {
            get { return currentQuestion; }
            set { currentQuestion = value; OnPropertyChanged(nameof(CurrentQuestion)); OnCurrentQuestionChanged(); }
        }

        

        private int currentQuestionDisplay;

        public int CurrentQuestionDisplay
        {
            get { return currentQuestionDisplay; }
            set { currentQuestionDisplay = value; OnPropertyChanged(nameof(CurrentQuestionDisplay)); }
        }




        private TestModel testModel;
        private TestModel? testModelToEdit;

        private TeacherViewViewModel teacherViewViewModel;

		private List<QuestionModel> questions;
		private Func<string, string, string, QuestionModel> createQuestion;

		public ICommand ChangeQuestionTextVisibility { get; }
		public ICommand ChangeQuestionText { get; }
		public ICommand ChangeImage { get; }
		public ICommand SetAudio {  get; }
		public ICommand NextQuestion { get; }
		public ICommand PreviousQuestion { get; }
		public ICommand AddOneAnswerAnswer { get; }
		public ICommand AddMultipleAnswerAnswer { get; }
        public ICommand DeleteQuestion { get; }
        public ICommand CreateQuestion { get; }
        public ICommand DeleteTest {  get; }
        public ICommand CreateTest { get; }
        public ICommand SaveQuestion { get; }

		public TestCreationViewModel(TeacherViewViewModel tvvm, TestModel testModel, TestModel? testModelToEdit)
		{
			teacherViewViewModel = tvvm;
			this.testModel = testModel;
            this.testModelToEdit = testModelToEdit;
			questions = new List<QuestionModel>();
			AnswerTypes = new List<string>() { "One answer test", "Several answer test", "Manual input" };
            ChangeQuestionTextVisibility = new ViewModelCommand(ExecuteChangeQuestionTextVisibility);
			ChangeQuestionText = new ViewModelCommand(ExecuteChangeQuestionText);
			ChangeImage = new ViewModelCommand(ExecuteChangeImage);
			SetAudio = new ViewModelCommand(ExecuteSetAudio);
			NextQuestion = new ViewModelCommand(ExecuteNextQuestion, CanExecuteNextQuestion);
            PreviousQuestion = new ViewModelCommand(ExecutePreviousQuestion, CanExecutePreviousQuestion);
            AddOneAnswerAnswer = new ViewModelCommand(ExecuteAddOneAnswerAnswer, CanExecuteAddOneAnswerAnswer);
			AddMultipleAnswerAnswer = new ViewModelCommand(ExecuteAddMultipleAnswerAnswer, CanExecuteAddMultipleAnswerAnswer);
            DeleteQuestion = new ViewModelCommand(ExecuteDeleteQuestion, CanExecuteDeleteQuestion);
            CreateQuestion = new ViewModelCommand(ExecuteCreateQuestion);
            DeleteTest = new ViewModelCommand(ExecuteDeleteTest);
            CreateTest = new ViewModelCommand(ExecuteCreateTest, CanExecuteCreateTest);
            SaveQuestion = new ViewModelCommand(ExecuteSaveQuestion);
            CurrentQuestion = 0;
            ClearData();
            if(testModelToEdit == null)
            {
                questions.Add(createQuestion.Invoke(QuestionText, ImagePath, AudioPath));
            }
            else
            {
                questions = testModelToEdit.Questions;
                UpdateQuestionData(questions[0]);
            }
        }

        private void ExecuteSaveQuestion(object obj)
        {
            questions[currentQuestion].QuestionText = QuestionText;
            questions[currentQuestion].ImagePath = ImagePath;
            questions[currentQuestion].AudioPath = AudioPath;
            if ((questions[currentQuestion] is IOneAnswer && SelectedIndex != 0) ||
                (questions[currentQuestion] is IMultipleAnswer && SelectedIndex != 1) ||
                (questions[currentQuestion] is IManualAnswer && SelectedIndex != 2))
            {
                questions[currentQuestion] = createQuestion.Invoke(QuestionText, ImagePath, AudioPath);
            }
            else if (questions[currentQuestion] is IOneAnswer oneAnswer)
            {
                oneAnswer.Answers = new List<string>();
                foreach (var answer in OneAnswerAnswers)
                {
                    oneAnswer.Answers.Add(answer.Answer);
                }
                oneAnswer.RightAnswer = OneAnswerRightAnswer;
            }
            else if (questions[currentQuestion] is IMultipleAnswer multipleAnswer)
            {
                multipleAnswer.Answers = new List<string>();
                foreach (var answer in MultipleAnswerAnswers)
                {
                    multipleAnswer.Answers.Add(answer.Answer);
                }
                multipleAnswer.RightAnswers = new List<int>();
                multipleAnswer.RightAnswers = MultipleAnswerRightAnswers;
            }
            else if (questions[currentQuestion] is IManualAnswer manualAnswer)
            {
                manualAnswer.RightAnswer = ManualAnswerRightAnswer;
            }
        }

        private bool CanExecuteCreateTest(object obj)
        {
            bool isValid = true;
            foreach(var question in questions)
            {
                if(question is IOneAnswer oneAnswer)
                {
                    if(oneAnswer.RightAnswer == null || oneAnswer.RightAnswer == -1)
                    {
                        isValid = false;
                    }
                }
                else if (question is IMultipleAnswer multipleAnswer)
                {
                    if (multipleAnswer.RightAnswers == null || multipleAnswer.RightAnswers.Count == 0)
                    {
                        isValid = false;
                    }
                }
                else if (question is IManualAnswer manualAnswer)
                {
                    if (manualAnswer.RightAnswer == null || string.IsNullOrEmpty(manualAnswer.RightAnswer))
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }

        private void ExecuteCreateTest(object obj)
        {
            testModel.Category = "Math";
            testModel.TeacherId = teacherViewViewModel.CurrentTeacher.Id;
            testModel.Questions = this.questions;
            testModel.Students = new List<StudentAccountModel>();
            if(testModelToEdit == null)
            {
                teacherViewViewModel.ShowTestsAssignClassesView.Execute(new object[] { teacherViewViewModel, null, testModel, null });
            }
            else
            {
                testModelToEdit.Category = testModel.Category;
                testModelToEdit.TeacherId = teacherViewViewModel.CurrentTeacher.Id;
                testModelToEdit.Questions = this.questions;
                teacherViewViewModel.ShowTestsAssignClassesView.Execute(new object[] { teacherViewViewModel, null, testModel, testModelToEdit });
            }
        }

        private void ExecuteDeleteTest(object obj)
        {
            teacherViewViewModel.ShowTestsView.Execute(obj);
        }

        private void ExecuteCreateQuestion(object obj)
        {
            questions[currentQuestion].QuestionText = QuestionText;
            questions[currentQuestion].ImagePath = ImagePath;
            questions[currentQuestion].AudioPath = AudioPath;
            if ((questions[currentQuestion] is IOneAnswer && SelectedIndex != 0) ||
                (questions[currentQuestion] is IMultipleAnswer && SelectedIndex != 1) ||
                (questions[currentQuestion] is IManualAnswer && SelectedIndex != 2))
            {
                questions[currentQuestion] = createQuestion.Invoke(QuestionText, ImagePath, AudioPath);
            }
            else if (questions[currentQuestion] is IOneAnswer oneAnswer)
            {
                oneAnswer.Answers = new List<string>();
                foreach (var answer in OneAnswerAnswers)
                {
                    oneAnswer.Answers.Add(answer.Answer);
                }
                oneAnswer.RightAnswer = OneAnswerRightAnswer;
            }
            else if (questions[currentQuestion] is IMultipleAnswer multipleAnswer)
            {
                multipleAnswer.Answers = new List<string>();
                foreach (var answer in MultipleAnswerAnswers)
                {
                    multipleAnswer.Answers.Add(answer.Answer);
                }
                multipleAnswer.RightAnswers = new List<int>();
                multipleAnswer.RightAnswers = MultipleAnswerRightAnswers;
            }
            else if (questions[currentQuestion] is IManualAnswer manualAnswer)
            {
                manualAnswer.RightAnswer = ManualAnswerRightAnswer;
            }

            CurrentQuestion = questions.Count;
            ClearData();
            questions.Add(createQuestion.Invoke(QuestionText, ImagePath, AudioPath));

        }

        private bool CanExecuteDeleteQuestion(object obj)
        {
            bool isValid = true;
            if(questions.Count <= 1)
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecuteDeleteQuestion(object obj)
        {
            questions.RemoveAt(CurrentQuestion);
            CurrentQuestion--;
            ClearData();
            UpdateQuestionData(questions[CurrentQuestion]);
        }

        private bool CanExecuteAddMultipleAnswerAnswer(object obj)
        {
            bool isValid = true;
            if (MultipleAnswerAnswers.Count >= 6 || MultipleAnswerAnswers.Where(o => o.AnswerTextBoxVisibility == true).FirstOrDefault() != null)
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecuteAddMultipleAnswerAnswer(object obj)
        {
			MultipleAnswerAnswers.Add(new MultipleChoicesViewModel(this) { Answer = "", IsChecked = false });
            MultipleAnswerAnswers[MultipleAnswerAnswers.Count - 1].AnswerTextVisibility = false;
            MultipleAnswerAnswers[MultipleAnswerAnswers.Count - 1].AnswerTextBoxVisibility = true;
        }

        private bool CanExecuteAddOneAnswerAnswer(object obj)
        {
			bool isValid = true;
			if(OneAnswerAnswers.Count >= 6 || OneAnswerAnswers.Where(o => o.AnswerTextBoxVisibility == true).FirstOrDefault() != null)
			{
				isValid = false;
			}
			return isValid;
        }

        private void ExecuteAddOneAnswerAnswer(object obj)
        {
			OneAnswerAnswers.Add(new OneChoicesViewModel(this) { Answer = "", IsChecked = false });
			OneAnswerAnswers[OneAnswerAnswers.Count - 1].AnswerTextVisibility = false;
			OneAnswerAnswers[OneAnswerAnswers.Count - 1].AnswerTextBoxVisibility = true;
        }

        private bool CanExecutePreviousQuestion(object obj)
        {
            bool isValid = true;
            if (CurrentQuestion == 0)
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecutePreviousQuestion(object obj)
        {
            //if (CurrentQuestion >= questions.Count)
            //{
              //  questions.Add(createQuestion.Invoke(QuestionText, ImagePath, AudioPath));
            //}
            //else
            //{
                questions[currentQuestion].QuestionText = QuestionText;
                questions[currentQuestion].ImagePath = ImagePath;
                questions[currentQuestion].AudioPath = AudioPath;
                if ((questions[currentQuestion] is IOneAnswer && SelectedIndex != 0) ||
                    (questions[currentQuestion] is IMultipleAnswer && SelectedIndex != 1) ||
                    (questions[currentQuestion] is IManualAnswer && SelectedIndex != 2))
                {
                    questions[currentQuestion] = createQuestion.Invoke(QuestionText, ImagePath, AudioPath);
                }
                else if (questions[currentQuestion] is IOneAnswer oneAnswer)
                {
                    oneAnswer.Answers = new List<string>();
                    foreach (var answer in OneAnswerAnswers)
                    {
                        oneAnswer.Answers.Add(answer.Answer);
                    }
                    oneAnswer.RightAnswer = OneAnswerRightAnswer;
                }
                else if (questions[currentQuestion] is IMultipleAnswer multipleAnswer)
                {
                    multipleAnswer.Answers = new List<string>();
                    foreach (var answer in MultipleAnswerAnswers)
                    {
                        multipleAnswer.Answers.Add(answer.Answer);
                    }
                    multipleAnswer.RightAnswers = new List<int>();
                    multipleAnswer.RightAnswers = MultipleAnswerRightAnswers;
                }
                else if (questions[currentQuestion] is IManualAnswer manualAnswer)
                {
                    manualAnswer.RightAnswer = ManualAnswerRightAnswer;
                }
           // }

            CurrentQuestion--;
            ClearData();
            UpdateQuestionData(questions[CurrentQuestion]);
        }

        private bool CanExecuteNextQuestion(object obj)
        {
			bool isValid = true;
            if (CurrentQuestion + 1 == questions.Count)
            {
                isValid = false;
            }
            return isValid;
        }


        private void ExecuteNextQuestion(object obj)
        {

            questions[currentQuestion].QuestionText = QuestionText;
            questions[currentQuestion].ImagePath = ImagePath;
            questions[currentQuestion].AudioPath = AudioPath;
            if ((questions[currentQuestion] is IOneAnswer && SelectedIndex != 0) ||
                (questions[currentQuestion] is IMultipleAnswer && SelectedIndex != 1) ||
                (questions[currentQuestion] is IManualAnswer && SelectedIndex != 2))
            {
                questions[currentQuestion] = createQuestion.Invoke(QuestionText, ImagePath, AudioPath);
            }
            else if (questions[currentQuestion] is IOneAnswer oneAnswer)
            {
                oneAnswer.Answers = new List<string>();
                foreach (var answer in OneAnswerAnswers)
                {
                    oneAnswer.Answers.Add(answer.Answer);
                }
                oneAnswer.RightAnswer = OneAnswerRightAnswer;
            }
            else if (questions[currentQuestion] is IMultipleAnswer multipleAnswer)
            {
                multipleAnswer.Answers = new List<string>();
                foreach (var answer in MultipleAnswerAnswers)
                {
                    multipleAnswer.Answers.Add(answer.Answer);
                }
                multipleAnswer.RightAnswers = new List<int>();
                multipleAnswer.RightAnswers = MultipleAnswerRightAnswers;
            }
            else if (questions[currentQuestion] is IManualAnswer manualAnswer)
            {
                manualAnswer.RightAnswer = ManualAnswerRightAnswer;
            }
            CurrentQuestion++;
            ClearData();
            UpdateQuestionData(questions[CurrentQuestion]);

            
        }

        private void ClearData()
        {
            QuestionTextVisibility = true;
            QuestionTextBoxVisibility = false;
            QuestionText = "Question Text";
            rememberQuestionText = QuestionText;
            ImagePath = "/Images/NoImageIcon.png";
            OneAnswerAnswers = new ObservableCollection<OneChoicesViewModel>();
            MultipleAnswerAnswers = new ObservableCollection<MultipleChoicesViewModel>();
            ManualAnswerRightAnswer = string.Empty;
            SelectedIndex = 0;
        }

		private void UpdateQuestionData(QuestionModel q)
		{
            QuestionText = q.QuestionText;
            rememberQuestionText = QuestionText;
            ImagePath = q.ImagePath;
			AudioPath = q.AudioPath;
            if(q is IOneAnswer ioa)
            {
                OneAnswerAnswers = new ObservableCollection<OneChoicesViewModel>();
                int i = 0;
                foreach(var answer in ioa.Answers)
                {
                    OneAnswerAnswers.Add(new OneChoicesViewModel(this) { Answer = answer });
                    if(i == ioa.RightAnswer)
                    {
                        OneAnswerAnswers[i].IsChecked = true;
                    }
                    i++;
                }
                SelectedIndex = 0;
            }
            else if (q is IMultipleAnswer ima)
            {
                MultipleAnswerAnswers = new ObservableCollection<MultipleChoicesViewModel>();
                int i = 0;
                foreach (var answer in ima.Answers)
                {
                    MultipleAnswerAnswers.Add(new MultipleChoicesViewModel(this) { Answer = answer });
                    if (ima.RightAnswers.Contains(i))
                    {
                        MultipleAnswerAnswers[i].IsChecked = true;
                    }
                    i++;
                }
                SelectedIndex = 1;
            }
            else if (q is IManualAnswer im)
            {
                ManualAnswerRightAnswer = im.RightAnswer;
                SelectedIndex = 2;
            }
        }

        private void OnCurrentQuestionChanged()
        {
            CurrentQuestionDisplay = CurrentQuestion + 1;
        }

        private void ExecuteSetAudio(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "AudioFiles|*.mp3";
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
                    model.Answers = new List<string>();
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
                    model.Answers = new List<string>();
                    foreach (var answer in MultipleAnswerAnswers)
                    {
                        model.Answers.Add(answer.Answer);
                    }
                    model.RightAnswers = new List<int>();
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

	public class OneChoicesViewModel : ViewModelBase
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

        private bool answerTextVisibility;

        public bool AnswerTextVisibility
        {
            get { return answerTextVisibility; }
            set { answerTextVisibility = value; OnPropertyChanged(nameof(AnswerTextVisibility)); }
        }

        private bool answerTextBoxVisibility;

        public bool AnswerTextBoxVisibility
        {
            get { return answerTextBoxVisibility; }
            set { answerTextBoxVisibility = value; OnPropertyChanged(nameof(AnswerTextBoxVisibility)); OnAnswerTextBoxVisibilityChange(); }
        }

        private string answerEditText;

        public string AnswerEditText
        {
            get { return answerEditText; }
            set { answerEditText = value; OnPropertyChanged(nameof(AnswerEditText)); }
        }

        public ICommand CompleteChangeAnswerText { get; }

        public ICommand DeleteAnswer { get; }
        public ICommand ChangeAnswerText { get; }

        private TestCreationViewModel testCreationViewModel;

        public OneChoicesViewModel(TestCreationViewModel tcvm)
        {
            testCreationViewModel = tcvm;
            answerTextVisibility = true;
            answerTextBoxVisibility = false;
			CompleteChangeAnswerText = new ViewModelCommand(ExecuteCompleteChangeAnswerText, CanExecuteCompleteChangeAnswerText);
            DeleteAnswer = new ViewModelCommand(ExecuteDeleteAnswer);
            ChangeAnswerText = new ViewModelCommand(ExecuteChangeAnswerText);
        }

        private void ExecuteChangeAnswerText(object obj)
        {
            AnswerTextVisibility = false;
            AnswerTextBoxVisibility = true;
            AnswerEditText = Answer;
        }

        private void ExecuteDeleteAnswer(object obj)
        {
            testCreationViewModel.OneAnswerAnswers.Remove(testCreationViewModel.OneAnswerAnswers.Where(o => o.Answer == this.Answer).First());
        }

        private bool CanExecuteCompleteChangeAnswerText(object obj)
        {
			bool isValid = true;
			if (string.IsNullOrEmpty(AnswerEditText) || testCreationViewModel.OneAnswerAnswers.Where(o => o.Answer == this.AnswerEditText).FirstOrDefault() != null)
			{
				isValid = false;
			}
			return isValid;
        }

        private void ExecuteCompleteChangeAnswerText(object obj)
        {
			Answer = AnswerEditText;
            AnswerTextVisibility = true;
            AnswerTextBoxVisibility = false;
        }

        private void OnAnswerTextBoxVisibilityChange()
        {
			if(!AnswerTextBoxVisibility)
            {
                AnswerTextVisibility = true;
                if(string.IsNullOrEmpty(Answer))
                {
                    testCreationViewModel.OneAnswerAnswers.Remove(testCreationViewModel.OneAnswerAnswers.Where(o => o.Answer == this.Answer).First());
                }
            }
        }

    }


    public class MultipleChoicesViewModel : ViewModelBase
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

        private bool answerTextVisibility;

        public bool AnswerTextVisibility
        {
            get { return answerTextVisibility; }
            set { answerTextVisibility = value; OnPropertyChanged(nameof(AnswerTextVisibility)); }
        }

        private bool answerTextBoxVisibility;

        public bool AnswerTextBoxVisibility
        {
            get { return answerTextBoxVisibility; }
            set { answerTextBoxVisibility = value; OnPropertyChanged(nameof(AnswerTextBoxVisibility)); OnAnswerTextBoxVisibilityChange(); }
        }

        private string answerEditText;

        public string AnswerEditText
        {
            get { return answerEditText; }
            set { answerEditText = value; OnPropertyChanged(nameof(AnswerEditText)); }
        }

        public ICommand CompleteChangeAnswerText { get; }

        public ICommand DeleteAnswer { get; }
        public ICommand ChangeAnswerText { get; }

        private TestCreationViewModel testCreationViewModel;

        public MultipleChoicesViewModel(TestCreationViewModel tcvm)
        {
            testCreationViewModel = tcvm;
            answerTextVisibility = true;
            answerTextBoxVisibility = false;
            CompleteChangeAnswerText = new ViewModelCommand(ExecuteCompleteChangeAnswerText, CanExecuteCompleteChangeAnswerText);
            DeleteAnswer = new ViewModelCommand(ExecuteDeleteAnswer);
            ChangeAnswerText = new ViewModelCommand(ExecuteChangeAnswerText);
        }

        private void ExecuteChangeAnswerText(object obj)
        {
            AnswerTextVisibility = false;
            AnswerTextBoxVisibility = true;
            AnswerEditText = Answer;
        }

        private void ExecuteDeleteAnswer(object obj)
        {
            testCreationViewModel.MultipleAnswerAnswers.Remove(testCreationViewModel.MultipleAnswerAnswers.Where(o => o.Answer == this.Answer).First());
        }

        private bool CanExecuteCompleteChangeAnswerText(object obj)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(AnswerEditText) || testCreationViewModel.MultipleAnswerAnswers.Where(o => o.Answer == this.AnswerEditText).FirstOrDefault() != null)
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecuteCompleteChangeAnswerText(object obj)
        {
            Answer = AnswerEditText;
            AnswerTextVisibility = true;
            AnswerTextBoxVisibility = false;
        }

        private void OnAnswerTextBoxVisibilityChange()
        {
            if (!AnswerTextBoxVisibility)
            {
                AnswerTextVisibility = true;
                if (string.IsNullOrEmpty(Answer))
                {
                    testCreationViewModel.MultipleAnswerAnswers.Remove(testCreationViewModel.MultipleAnswerAnswers.Where(o => o.Answer == this.Answer).First());
                }
            }
        }

    }

}
