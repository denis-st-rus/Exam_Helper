﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Helper.ViewsModel
{
    public class ClassForPackCreatingModel
    {

        public List<QuestionForPackCreatingModel> questions { get; set; }
        public Pack pack { get; set; }
        public List<TagForPackCreatingModel> tags { get; set; }
        
        public int Id { get; set; }

        [Required(ErrorMessage ="пожалуйста,укажите название пака")]
        [Display(Name ="название пака")]
        public string pack_name { get; set; }
    }

    public class QuestionForPackCreatingModel
    {
        public int Id { get; set; }
       
        public string Name { get; set; }

       public bool IsSelected { get; set; }
    }
    public class TagForPackCreatingModel
    {
        public int Id { get; set; }
       
        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }
}
