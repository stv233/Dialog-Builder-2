using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog_Builder_2
{
    public class Page
    {
        private string pageName = "";
        private string pageText = "";
        private string pageActions = "";
        private List<Response> pageResponses = new List<Response>();

        public string Name
        {
            get
            {
                return pageName;
            }
            set
            {
                pageName = value;
            }
        }

        public string Text
        {
            get
            {
                return pageText;
            }
            set
            {
                pageText = value;
            }
        }

        public string Actions
        {
            get
            {
                return pageActions;
            }
            set
            {
                pageActions = value;
            }
        }

        public List<Response> Responses
        {
            get
            {
                return pageResponses;
            }
            set
            {
                pageResponses = value;
            }
        }

        public Page() { }

        public Page(string name)
        {
            Name = name;
        }
    }
}
