using NAudio.Wave;
using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    public class StudentTestCompletionViewModel : ViewModelBase, IDisposable
    {

        private bool audioVisibility;

        public bool AudioVisibility
        {
            get { return audioVisibility; }
            set { audioVisibility = value; OnPropertyChanged(nameof(AudioVisibility)); }
        }

        private string audioCurrentValue;

        public string AudioCurrentValue
        {
            get { return audioCurrentValue; }
            set { audioCurrentValue = value; OnPropertyChanged(nameof(AudioCurrentValue)); }
        }

        private string audioMaxValue;

        public string AudioMaxValue
        {
            get { return audioMaxValue; }
            set { audioMaxValue = value; OnPropertyChanged(nameof(AudioMaxValue)); }
        }

        private double audioMaxPosition;

        public double AudioMaxPosition
        {
            get { return audioMaxPosition; }
            set { audioMaxPosition = value; OnPropertyChanged(nameof(AudioMaxPosition)); }
        }

        private double selectedAudioPosition;

        public double SelectedAudioPosition
        {
            get { return selectedAudioPosition; }
            set { selectedAudioPosition = value; OnPropertyChanged(nameof(SelectedAudioPosition)); OnSelectedAudioPositionChanged(); }
        }

        

        private System.Windows.Forms.Timer audioTimer;

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





        private bool questionTextVisibility;

        public bool QuestionTextVisibility
        {
            get { return questionTextVisibility; }
            set { questionTextVisibility = value; OnPropertyChanged(nameof(QuestionTextVisibility)); }
        }

        //


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

        private ObservableCollection<StudentsOneChoicesViewModel> oneAnswerAnswers;

        public ObservableCollection<StudentsOneChoicesViewModel> OneAnswerAnswers
        {
            get { return oneAnswerAnswers; }
            set { oneAnswerAnswers = value; OnPropertyChanged(nameof(OneAnswerAnswers)); }
        }

        private int oneAnswerRightAnswer;

        public int OneAnswerRightAnswer
        {
            get
            {
                return OneAnswerAnswers.Select((value, index) => new { value, index = index + 1 })
                .Where(pair => pair.value.IsChecked == true)
                .Select(pair => pair.index)
                .FirstOrDefault() - 1;
            }
        }

        private ObservableCollection<StudentsMultipleChoicesViewModel> multipleAnswerAnswers;

        public ObservableCollection<StudentsMultipleChoicesViewModel> MultipleAnswerAnswers
        {
            get { return multipleAnswerAnswers; }
            set { multipleAnswerAnswers = value; OnPropertyChanged(nameof(MultipleAnswerAnswers)); }
        }


        private List<int> multipleAnswerRightAnswers;

        public List<int> MultipleAnswerRightAnswers
        {
            get
            {
                return MultipleAnswerAnswers.Select((value, index) => new { value, index = index + 1 })
                .Where(pair => pair.value.IsChecked == true)
                .Select(pair => pair.index).ToList().Select(o => o - 1).ToList();
            }
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
        private TestModel testModelOriginal;

        private MainViewViewModel mainViewViewModel;

        private List<QuestionModel> questions;
        private Func<string, string, string, QuestionModel> createQuestion;

        private Mp3FileReader reader;
        private WaveOut waveOut;

        private ITestRepository testRepository;
        
        public ICommand NextQuestion { get; }
        public ICommand PreviousQuestion { get; }
        public ICommand DeleteTest { get; }
        public ICommand CreateTest { get; }
        public ICommand SaveQuestion { get; }
        public ICommand PlayStopAudio { get; }
        public ICommand DragStarted { get; }
        public StudentTestCompletionViewModel(MainViewViewModel mvvm, TestModel testModelOriginal)
        {
            mainViewViewModel = mvvm;
            this.testModel = new TestModel() { Questions = new List<QuestionModel>() };
            this.testModelOriginal = testModelOriginal;
            testRepository = new TestRepository();
            audioTimer = new System.Windows.Forms.Timer();
            foreach (var question in testModelOriginal.Questions)
            {
                if (question is IOneAnswer oneAnswer)
                {
                    testModel.Questions.Add(new OneAnswerQuestionModel() { Answers = oneAnswer.Answers, RightAnswer = -1 });
                }
                else if (question is IMultipleAnswer multipleAnswer)
                {
                    testModel.Questions.Add(new MultipleAnswerQuestionModel() { Answers = multipleAnswer.Answers, RightAnswers = new List<int>() });
                }
                else if (question is IManualAnswer manualAnswer)
                {
                    testModel.Questions.Add(new ManualQuestionModel() { RightAnswer = null });
                }
            }
            questions = new List<QuestionModel>();
            NextQuestion = new ViewModelCommand(ExecuteNextQuestion, CanExecuteNextQuestion);
            PreviousQuestion = new ViewModelCommand(ExecutePreviousQuestion, CanExecutePreviousQuestion);
            DeleteTest = new ViewModelCommand(ExecuteDeleteTest);
            CreateTest = new ViewModelCommand(ExecuteCreateTest, CanExecuteCreateTest);
            SaveQuestion = new ViewModelCommand(ExecuteSaveQuestion);
            PlayStopAudio = new ViewModelCommand(ExecutePlayStopAudio);
            DragStarted = new ViewModelCommand(ExecuteDragStarted);
            CurrentQuestion = 0;
            ClearData();
            questions = testModelOriginal.Questions;
            UpdateQuestionData(questions[0], CurrentQuestion);
        }

        private void ExecuteDragStarted(object obj)
        {
            MessageBox.Show("Drag Started");
        }

        private void OnSelectedAudioPositionChanged()
        {
            if(reader != null)
            {
                reader.CurrentTime = new TimeSpan(0, 0, (int)SelectedAudioPosition);
            }
        }
        private void ExecutePlayStopAudio(object obj)
        {
            if(waveOut.PlaybackState == PlaybackState.Stopped)
            {
                audioTimer.Start();
                waveOut.Play();
            }
            else if (waveOut.PlaybackState == PlaybackState.Paused)
            {
                audioTimer.Start();
                waveOut.Resume();
            }
            else if (waveOut.PlaybackState == PlaybackState.Playing)
            {
                audioTimer.Stop();
                waveOut.Pause();
            }

        }

        private void ExecuteSaveQuestion(object obj)
        {
            if (questions[currentQuestion] is IOneAnswer oneAnswer)
            {
                (testModel.Questions[currentQuestion] as IOneAnswer).RightAnswer = OneAnswerRightAnswer;
            }
            else if (questions[currentQuestion] is IMultipleAnswer multipleAnswer)
            {
                (testModel.Questions[currentQuestion] as IMultipleAnswer).RightAnswers = MultipleAnswerRightAnswers;
            }
            else if (questions[currentQuestion] is IManualAnswer manualAnswer)
            {
                (testModel.Questions[currentQuestion] as IManualAnswer).RightAnswer = ManualAnswerRightAnswer;
            }
        }

        private bool CanExecuteCreateTest(object obj)
        {
            bool isValid = true;
            foreach (var question in testModel.Questions)
            {
                if (question is IOneAnswer oneAnswer)
                {
                    if (oneAnswer.RightAnswer == null || oneAnswer.RightAnswer == -1)
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
           ClearData();
            testRepository.SetStudentResult(CalculateResult(), mainViewViewModel.CurrentStudentAccount.Id, testModelOriginal.Id);
            mainViewViewModel.ShowTestsView.Execute(null);
        }

        private double CalculateResult()
        {
            double result = 0;
            double maxPointsPerQuestion = 100.0 / testModelOriginal.Questions.Count;
            int i = 0;
            foreach(var question in testModel.Questions)
            {
                if (question is IOneAnswer oneAnswer)
                {
                    if(oneAnswer.RightAnswer == (testModelOriginal.Questions[i] as IOneAnswer).RightAnswer)
                    {
                        result += maxPointsPerQuestion;
                    }
                }
                else if (question is IMultipleAnswer multipleAnswer)
                {
                    double rightAnswersTotal = 0;
                    foreach(int rightAnswer in multipleAnswer.RightAnswers)
                    {
                        if ((testModelOriginal.Questions[i] as IMultipleAnswer).RightAnswers.Contains(rightAnswer))
                        {
                            rightAnswersTotal++;
                        }
                        else
                        {
                            rightAnswersTotal--;
                        }
                    }
                    if(rightAnswersTotal < 0)
                    {
                        rightAnswersTotal = 0;
                    }
                    result += (rightAnswersTotal/ (testModelOriginal.Questions[i] as IMultipleAnswer).RightAnswers.Count) * maxPointsPerQuestion;
                }
                else if (question is IManualAnswer manualAnswer)
                {
                    if(manualAnswer.RightAnswer == (testModelOriginal.Questions[i] as IManualAnswer).RightAnswer)
                    {
                        result += maxPointsPerQuestion;
                    }
                }
                i++;
            }
            return result;
        }

        private void ExecuteDeleteTest(object obj)
        {
            ClearData();
            mainViewViewModel.ShowTestsView.Execute(obj);
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
            ExecuteSaveQuestion(obj);

            CurrentQuestion--;
            ClearData();
            UpdateQuestionData(questions[CurrentQuestion], CurrentQuestion);
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
            ExecuteSaveQuestion(obj);
            
            CurrentQuestion++;
            ClearData();
            UpdateQuestionData(questions[CurrentQuestion], CurrentQuestion);


        }

        private void ClearData()
        {
            if(waveOut != null)
            {
                waveOut.Dispose();
            }
            if(reader != null)
            {
                reader.Dispose();
            }
            if(audioTimer != null && audioTimer.Enabled)
            {
                audioTimer.Stop();
            }
            
            QuestionText = "Question Text";
            ImagePath = "/Images/NoImageIcon.png";
            OneAnswerAnswers = new ObservableCollection<StudentsOneChoicesViewModel>();
            MultipleAnswerAnswers = new ObservableCollection<StudentsMultipleChoicesViewModel>();
            ManualAnswerRightAnswer = string.Empty;
        }

        private void UpdateQuestionData(QuestionModel q, int questionId)
        {
            AudioVisibility = false;
            OneAnswerVisibility = false;
            MultipleAnswersVisibility = false;
            ManualAnswerVisibility = false;
            QuestionText = q.QuestionText;
            ImagePath = q.ImagePath;
            AudioPath = q.AudioPath;
            if(AudioPath != null)
            {
                reader = new Mp3FileReader(testModelOriginal.Questions[CurrentQuestion].AudioPath);
                waveOut = new WaveOut();
                waveOut.Init(reader);
                AudioVisibility = true;
                SelectedAudioPosition = 0;
                AudioCurrentValue = "00:00";
                AudioMaxPosition = reader.TotalTime.TotalSeconds;
                int min = Convert.ToInt32(Math.Floor(reader.TotalTime.TotalMinutes));
                string minStr = "";
                if(min < 10)
                {
                    minStr = $"0{min}";
                }
                else
                {
                    minStr = $"{min}";
                }
                int sec = reader.TotalTime.Seconds;
                string secStr = "";
                if (sec < 10)
                {
                    secStr = $"0{sec}";
                }
                else
                {
                    secStr = $"{sec}";
                }
                AudioMaxValue = $"{minStr}:{secStr}";
                waveOut.Volume = 1.0f;
                audioTimer.Tick += (object sender, EventArgs e) =>
                {
                    int minutes = Convert.ToInt32(Math.Floor(reader.CurrentTime.TotalMinutes));
                    int seconds = reader.CurrentTime.Seconds;
                    string minutesString = "";
                    string secondsString = "";
                    if(seconds < 10)
                    {
                        secondsString = $"0{seconds}";
                    }
                    else
                    {
                        secondsString = $"{seconds}";
                    }
                    if(minutes < 10)
                    {
                        minutesString = $"0{minutes}";
                    }
                    else
                    {
                        minutesString = $"{minutes}";
                    }
                    AudioCurrentValue = $"{minutesString}:{secondsString}";
                    selectedAudioPosition = reader.CurrentTime.Seconds;
                    OnPropertyChanged(nameof(SelectedAudioPosition));
                };
                audioTimer.Interval = 1000;
            }
            if (q is IOneAnswer ioa)
            {
                OneAnswerAnswers = new ObservableCollection<StudentsOneChoicesViewModel>();
                int i = 0;
                foreach (var answer in ioa.Answers)
                {
                    OneAnswerAnswers.Add(new StudentsOneChoicesViewModel(this) { Answer = answer, IsChecked = false });
                    if (i == (testModel.Questions[questionId] as IOneAnswer).RightAnswer)
                    {
                        OneAnswerAnswers[i].IsChecked = true;
                    }
                    i++;
                }
                OneAnswerVisibility = true;
            }
            else if (q is IMultipleAnswer ima)
            {
                MultipleAnswerAnswers = new ObservableCollection<StudentsMultipleChoicesViewModel>();
                int i = 0;
                foreach (var answer in ima.Answers)
                {
                    MultipleAnswerAnswers.Add(new StudentsMultipleChoicesViewModel(this) { Answer = answer, IsChecked = false });
                    if ((testModel.Questions[questionId] as IMultipleAnswer).RightAnswers.Contains(i))
                    {
                        MultipleAnswerAnswers[i].IsChecked = true;
                    }
                    i++;
                }
                MultipleAnswersVisibility = true;
            }
            else if (q is IManualAnswer im)
            {
                ManualAnswerRightAnswer = (testModel.Questions[questionId] as IManualAnswer).RightAnswer;
                ManualAnswerVisibility = true;
            }
        }

        private void OnCurrentQuestionChanged()
        {
            CurrentQuestionDisplay = CurrentQuestion + 1;
        }

        public void Dispose()
        {
            ClearData();
        }
    }


    public class StudentsOneChoicesViewModel : ViewModelBase
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



        private StudentTestCompletionViewModel studentTestCompletionView;

        public StudentsOneChoicesViewModel(StudentTestCompletionViewModel stcvm)
        {
            studentTestCompletionView = stcvm;
        }

       

    }


    public class StudentsMultipleChoicesViewModel : ViewModelBase
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

        

        private StudentTestCompletionViewModel studentTestCompletionViewModel;

        public StudentsMultipleChoicesViewModel(StudentTestCompletionViewModel stcvm)
        {
            studentTestCompletionViewModel = stcvm;
        }

        
    }

}


