using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReportGeneration_Subbotin.Classes;
using ReportGeneration_Subbotin.Pages;

namespace ReportGeneration_Subbotin.Items
{
    /// <summary>
    /// Логика взаимодействия для Student.xaml
    /// </summary>
    public partial class Student : UserControl
    {

        public Student(StudentContext student, Main main)
        {
            InitializeComponent();
            TBFio.Text = $"{student.Lastname} {student.Firstname}";
            CBExpelled.IsChecked = student.Expelled;
            List<DisciplineContext> StudentDisciplines = Main.AllDisciplines.FindAll(x => x.IdGroup == student.IdGroup);
            int NecessarilyCount = 0;
            int WorksCount = 0;
            int DoneCount = 0;
            int MissedCount = 0;
            foreach (DisciplineContext StudentDiscipline in StudentDisciplines)
            {
                List<WorkContext> StudentWorks = Main.AllWorks.FindAll(x => (x.IdType == 1 ||
                x.IdType == 2 || x.IdType == 3) && x.IdDiscipline == StudentDiscipline.Id);
                NecessarilyCount += StudentWorks.Count;
                foreach (WorkContext StudentWork in StudentWorks)
                {
                    EvaluationContext Evaluation = Main.AllEvaluation.Find(x =>
                        x.IdWork == StudentWork.Id &&
                        x.IdStudent == student.Id);
                    if (Evaluation != null && Evaluation.Value.Trim() != "" && Evaluation.Value.Trim() != "2")
                        DoneCount++;
                }
                StudentWorks = Main.AllWorks.FindAll(x => (
                    x.IdType != 4 && x.IdType != 5) && x.IdDiscipline == StudentDiscipline.Id);
                WorksCount += StudentWorks.Count;
                foreach (WorkContext StudentWork in StudentWorks)
                {
                    EvaluationContext Evaluation = Main.AllEvaluation.Find(x =>
                        x.IdWork == StudentWork.Id &&
                        x.IdStudent == student.Id);
                    if (Evaluation != null && Evaluation.Lateness.Trim() != "")
                        MissedCount += Convert.ToInt32(Evaluation.Lateness);
                }
            }
            doneWorks.Value = (100f / (float)NecessarilyCount) * ((float)DoneCount);
            missedCount.Value = 100f / ((float)WorksCount * 90) * ((float)MissedCount);
            TBGroup.Text = Main.AllGroups.Find(x => x.Id == student.IdGroup).Name;
        }
    }
}
