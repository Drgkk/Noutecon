﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_.Model
{
    public interface IClassRepository
    {
        bool ValidateClass(string uniqueId);
        void Add(ClassModel classModel);
        void UpdateUniqueIdOfClassWithId(int id, string uniqueId);
        void Remove(ClassModel classModel);
        ClassModel GetById(int id);
        ClassModel GetByUniqueId(string uniqueId);
        IEnumerable<ClassModel> GetAll();
        ObservableCollection<ClassModel> GetClassesByTeacherId(int teacherId);
        ObservableCollection<ClassModel> GetClassesByStudentId(int studentId);
        int GetId(string uniqueId);
    }
}
