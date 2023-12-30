using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noutecon__Exam_
{
    public class ClassCodeGenerator
    {
        private IClassRepository classRepository;

        private List<char> charList;

        public ClassCodeGenerator()
        {
            classRepository = new ClassRepository();
            charList = new List<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0',
                                          'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p',
                                          'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z',
                                          'x', 'c', 'v', 'b', 'n', 'm', 'Q', 'W', 'E', 'R',
                                          'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F',
                                          'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B',
                                          'N', 'M'};
        }

        public string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            while (true)
            {
                for (int i = 0; i < 6; i++)
                {
                    sb.Append(charList[rand.Next(0, charList.Count)]);
                }
                if (classRepository.GetByUniqueId(sb.ToString()) == null)
                {
                    break;
                }
                sb.Clear();
            }
            return sb.ToString();
        }


    }
}
